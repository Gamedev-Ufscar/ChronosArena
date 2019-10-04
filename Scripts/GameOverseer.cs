using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameOverseer : MonoBehaviour
{
    [SerializeField]
    private Player myPlayer;
    [SerializeField]
    private Player enemyPlayer;
    [SerializeField]
    private MainUIManager UIManager;
    [SerializeField]
    private MidButton midButton;
    [SerializeField]
    private Interface interfface;

    private bool myConfirm = false;
    private bool enemyConfirm = false;

    public LayerMask layerMask;
    public GameObject viewCard;
    private GameObject boardCardReader;
    private List<GameObject> destroyList = new List<GameObject>();

    private GameState state = GameState.Purchase;

    private void Awake()
    {  
    }

    private void Start()
    {
        Debug.Log("A new Bahn");
        if (NetworkBahn.networkBahn != null)
        {
            Destroy(NetworkBahn.networkBahn.gameObject);
        }
        GameObject netObject = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Test2"), Vector3.zero, Quaternion.identity);
        netObject.GetComponent<NetworkBahn>().GiveGameOverseer(this);

        state = GameState.Purchase;
        UpdateBar();
    }

    // Hover Card
    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 3 && ((GetState() == GameState.Choice && GetEnemyPlayer().GetPredicted()) ||
            GetState() == GameState.Reaction || GetState() == GameState.Effects) && !GetMyPlayer().GetDeck().GetHoldingCard())
        {
            HoverCarding();
        }

        //Debug.Log("My Conf: " + myConfirm + ", Enemy Conf: " + enemyConfirm);
    }

    // State Stuff

    public void StateMachine() { StateMachine(false); }

    public void StateMachine(bool overrider)
    {
        if ((myConfirm && enemyConfirm) || overrider)
        {
            SetMyConfirm(false);

            switch (state)
            {
                case GameState.Purchase:
                    myPlayer.DarkenUltiCards(true);
                    enemyPlayer.DarkenUltiCards(true);
                    myPlayer.AcquireCards();
                    enemyPlayer.AcquireCards();
                    ToChoiceState();
                    break;

                case GameState.Choice:
                    myPlayer.GetBoardCard().SetWaiting(false);
                    enemyPlayer.GetBoardCard().SetWaiting(false);
                    ToInterfaceState();
                    break;

                case GameState.Interface:
                    ToReactionState();
                    break;

                case GameState.Reaction:
                    ToEffectsState();
                    break;

                case GameState.Effects:
                    myPlayer.DarkenUltiCards(false);
                    enemyPlayer.DarkenUltiCards(false);
                    Destroy(myPlayer.GetBoardCard());
                    Destroy(enemyPlayer.GetBoardCard());
                    DestroyHoverCards();
                    ToPurchaseState();
                    break;
            }

        }
    }

    private void ToChoiceState()
    {
        state = GameState.Choice;
        UIManager.DebugText("Choice");
        myPlayer.ActivateSideEffects(SEPhase.Choice, enemyPlayer);
        enemyPlayer.ActivateSideEffects(SEPhase.Choice, myPlayer);

    }

    private void ToInterfaceState()
    {
        state = GameState.Interface;
        UIManager.DebugText("Interface");
        if (myPlayer.GetCardPlayed() is Interfacer)
        {
            Interfacer inter = (Interfacer)myPlayer.GetCardPlayed();
            inter.Interfacing(myPlayer, enemyPlayer);
        } else
        {
            SetMyConfirm(true);
        }

        if (enemyPlayer.GetCardPlayed() is Interfacer)
        {
            Interfacer inter = (Interfacer)enemyPlayer.GetCardPlayed();
            inter.Interfacing(enemyPlayer, myPlayer);
        }
    }

    private void ToReactionState()
    {
        state = GameState.Reaction;
        UIManager.DebugText("Reaction");
        myPlayer.ActivateSideEffects(SEPhase.Reaction, enemyPlayer);
        enemyPlayer.ActivateSideEffects(SEPhase.Reaction, myPlayer);

        // If I can't play reactions, just skip
        if (!myPlayer.HasReactionCard())
        {
            SetMyConfirm(true);
        }
    }

    private void ToEffectsState()
    {
        state = GameState.Effects;
        UIManager.DebugText("Effects");
        // Activate Side Effects
        myPlayer.ActivateSideEffects(SEPhase.EffectsBefore, enemyPlayer);
        enemyPlayer.ActivateSideEffects(SEPhase.EffectsBefore, myPlayer);

        // Activate Effects
        ActivateCards();

        // Activate Side Effects
        myPlayer.ActivateSideEffects(SEPhase.EffectsAfter, enemyPlayer);
        enemyPlayer.ActivateSideEffects(SEPhase.EffectsAfter, myPlayer);

        // Restore Played Card
        myPlayer.RestorePlayedCard();
        enemyPlayer.RestorePlayedCard();

    }

    private void ActivateCards()
    {
        if (myPlayer != null) { Debug.Log("my player ok"); }
        if (myPlayer.GetCardPlayed() != null) { Debug.Log("cardPlayed ok"); }
        if (enemyPlayer != null) { Debug.Log("enemy player ok"); }
        if (enemyPlayer.GetCardPlayed() != null) { Debug.Log("enemy cardPlayed ok"); }

        for (int e = Mathf.Min(myPlayer.GetCardPlayed().GetMinOrMax(true) / 100, 
            enemyPlayer.GetCardPlayed().GetMinOrMax(true) / 100);
            e <= Mathf.Max(myPlayer.GetCardPlayed().GetMinOrMax(false) % 100, 
            enemyPlayer.GetCardPlayed().GetMinOrMax(false) % 100);
            e++)
        {
            if (!myPlayer.GetCardPlayed().GetIsNullified())
                myPlayer.GetCardPlayed().Effect(myPlayer, enemyPlayer, e);
            if (!enemyPlayer.GetCardPlayed().GetIsNullified())
                enemyPlayer.GetCardPlayed().Effect(enemyPlayer, myPlayer, e);
        }
    }

    private void ToPurchaseState()
    {
        state = GameState.Purchase;
        UIManager.DebugText("Purchase");

        // If I can't buy cards, just skip
        if (!myPlayer.CanBuyCards())
        {
            SetMyConfirm(true);
        }
        
    }

    // Healthbar & Chargebar
    public void UpdateBar()
    {
        UIManager.UpdateBar(GetMyPlayer(), GetEnemyPlayer());
    }

    // Interfacing
    public void Interfacing(Card[] cardList, Card invoker)
    {
        interfface.gameObject.SetActive(true);
        interfface.Setup(cardList, invoker);
    }

    public void Interfacing(Card baseCard, string[] textList, Card invoker)
    {
        interfface.gameObject.SetActive(true);
        interfface.Setup(baseCard, textList, invoker);
    }

    public void HoverCarding()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        Physics.Raycast(mouseRay, out hitInfo, 300, layerMask, QueryTriggerInteraction.Collide);

        if (hitInfo.collider != null)
        {
            if (hitInfo.collider.transform.parent != null)
            {
                Debug.Log("HoverCard");
                GameObject cardInBoard = hitInfo.collider.transform.parent.gameObject;

                boardCardReader = Instantiate(viewCard, Camera.main.WorldToScreenPoint(hitInfo.collider.transform.position), Quaternion.identity, myPlayer.GetDeck().transform);
                destroyList.Add(boardCardReader);
                boardCardReader.GetComponent<HoverCard>().ConstructHoverCard(cardInBoard);
            }
        }
        else if (boardCardReader != null)
        {
            DestroyHoverCards();
        }
    }

    public void DestroyHoverCards()
    {
        foreach (GameObject destroyed in destroyList)
        {
            Destroy(destroyed);
        }
    }


    // Getter
    public GameState GetState()
    {
        return state;
    }

    public Player GetMyPlayer()
    {
        return myPlayer;
    }

    public Player GetEnemyPlayer()
    {
        return enemyPlayer;
    }

    public bool GetMyConfirm()
    {
        return myConfirm;
    }

    // Setter
    public void SetMyConfirm(bool myConfirm)
    {
        this.myConfirm = myConfirm;
        UIManager.SetPlayerHue(myConfirm);
        SendConfirm(myConfirm);
        midButton.SetImageColor(myConfirm);
        StateMachine();
    }

    public void SetEnemyConfirm(bool enemyConfirm)
    {
        this.enemyConfirm = enemyConfirm;
        UIManager.SetEnemyHue(enemyConfirm);
        StateMachine();
    }

    public void InvertMyConfirm()
    {
        SetMyConfirm(!myConfirm);
    }

    // Network Sender
    public void SendSummonCard(int cardID)
    {
        NetworkBahn.networkBahn.SummonCard(cardID);
    }

    public void SendConfirm(bool confirm)
    {
        NetworkBahn.networkBahn.SendConfirm(confirm);
    }

    public void SendShuffle(int[] cardIndexes)
    {
        NetworkBahn.networkBahn.SendShuffle(cardIndexes);
    }

    public void SendInterfaceSignal(int interfaceSignal)
    {
        NetworkBahn.networkBahn.SendInterfaceSignal(interfaceSignal);
    }

    public void SendCardPosition(int id, Vector2 position)
    {
        NetworkBahn.networkBahn.SendCardPosition(id, position);
    }

    public void SendCardPositionStop()
    {
        NetworkBahn.networkBahn.SendCardPositionStop();
    }

    public void SendUltiPosition(int id, Player playerHovered)
    {
        bool hoveringMyself;
        hoveringMyself = (playerHovered == myPlayer) ? true : false;
        NetworkBahn.networkBahn.SendUltiHover(id, hoveringMyself);
    }

    public void SendUltiStop(Player playerHovered)
    {
        bool hoveringMyself;
        hoveringMyself = (playerHovered == myPlayer) ? true : false;
        NetworkBahn.networkBahn.SendUltiStop(hoveringMyself);
    }

    public void SendUltiPurchase(int cardID, bool bought, int charge)
    {
        NetworkBahn.networkBahn.SendUltiPurchase(cardID, bought, charge);
    }

    // Network Receiver
    public void ReceiveShuffle(int[] receivedCardIndexes)
    {
        enemyPlayer.ReceiveShuffle(receivedCardIndexes);
    }

    public void ReceiveSummon(int cardID) {
        enemyPlayer.ReceiveSummon(cardID);
    }

    public void ReceiveCardPosition(int hoverCard, Vector2 hoverPos)
    {
        enemyPlayer.ReceiveCardPosition(hoverCard, hoverPos);
    }

    public void ReceiveCardPositionStop()
    {
        enemyPlayer.ReceiveCardPosition(null, new Vector2(0f, 0f));
    }

    public void ReceiveUltiHover(int? hoverCard, bool hoveringMyself)
    {
        if (hoveringMyself)
            enemyPlayer.ReceiveUltiHover(hoverCard);
        else
            myPlayer.ReceiveUltiHover(hoverCard);
    }

    public void ReceiveUltiPurchase(int cardID, bool bought, int charge)
    {
        GetEnemyPlayer().ReceiveUltiPurchase(cardID, bought, charge);
    }

}
