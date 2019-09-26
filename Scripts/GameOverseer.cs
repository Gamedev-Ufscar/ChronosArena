using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverseer : MonoBehaviour
{
    [SerializeField]
    private Player myPlayer;
    [SerializeField]
    private Player enemyPlayer;
    [SerializeField]
    private MainUIManager UIManager;
    //[SerializeField]
    //private NetworkTrain networkTrain;
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
        Debug.Log("A new Train");
        if (NetworkTrain.networkTrain == null)
        {
            GameObject netObject = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Test"), Vector3.zero, Quaternion.identity);
            netObject.GetComponent<NetworkTrain>().GiveGameOverseer(this);
        }
    }

    // Hover Card
    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 3&& ((GetState() == GameState.Choice && GetEnemyPlayer().GetPredicted()) ||
            GetState() == GameState.Reaction || GetState() == GameState.Effects) && !GetMyPlayer().GetDeck().GetHoldingCard())
        {
            HoverCarding();
        }
    }

    // State Stuff
    public void UpdatedCardPlayed(Player player, bool cardPlayed)
    {
        if (state == GameState.Choice)
        {
            if (player == myPlayer) { SetMyConfirm(cardPlayed); }
            else if (player == enemyPlayer) { SetEnemyConfirm(cardPlayed); }
        }
    }

    public void StateMachine() { StateMachine(false); }

    public void StateMachine(bool overrider)
    {
        if ((myConfirm && enemyConfirm) || overrider)
        {
            switch (state)
            {
                case GameState.Purchase:
                    ToChoiceState();
                    myPlayer.PurchaseCards();
                    enemyPlayer.PurchaseCards();
                    break;

                case GameState.Choice:
                    if (myPlayer.GetCardPlayed() is Interfacer || enemyPlayer.GetCardPlayed() is Interfacer)
                    {
                        ToInterfaceState();
                    } else if (myPlayer.HasReactionCard() || enemyPlayer.HasReactionCard())
                    {
                        ToReactionState();
                    } else
                    {
                        ToEffectsState();
                    }
                    break;

                case GameState.Reaction:
                    ToEffectsState();
                    break;

                case GameState.Effects:
                    ToPurchaseState();
                    break;
            }

            myConfirm = false;
            enemyConfirm = false;
        }
    }

    private void ToChoiceState()
    {
        state = GameState.Choice;
        myPlayer.ActivateSideEffects(SEPhase.Choice, enemyPlayer);
        enemyPlayer.ActivateSideEffects(SEPhase.Choice, myPlayer);

    }

    private void ToInterfaceState()
    {
        state = GameState.Interface;
        if (myPlayer.GetCardPlayed() is Interfacer)
        {
            Interfacer inter = (Interfacer)myPlayer.GetCardPlayed();
            inter.Interfacing(myPlayer, enemyPlayer);
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
        myPlayer.ActivateSideEffects(SEPhase.Reaction, enemyPlayer);
        enemyPlayer.ActivateSideEffects(SEPhase.Reaction, myPlayer);
    }

    private void ToEffectsState()
    {
        state = GameState.Effects;
        // Activate Side Effects
        myPlayer.ActivateSideEffects(SEPhase.EffectsBefore, enemyPlayer);
        enemyPlayer.ActivateSideEffects(SEPhase.EffectsBefore, myPlayer);

        // Activate Effects
        ActivateCards();

        // Activate Side Effects
        myPlayer.ActivateSideEffects(SEPhase.EffectsAfter, enemyPlayer);
        enemyPlayer.ActivateSideEffects(SEPhase.EffectsAfter, myPlayer);
    }

    private void ActivateCards()
    {
        for (int e = Mathf.Min(myPlayer.GetCardPlayed().GetMinOrMax(true) / 100, enemyPlayer.GetCardPlayed().GetMinOrMax(true) / 100);
            e <= Mathf.Max(myPlayer.GetCardPlayed().GetMinOrMax(false) % 100, enemyPlayer.GetCardPlayed().GetMinOrMax(false) % 100);
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
        DestroyHoverCards();
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

        Debug.Log("I'm alive");

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

    // Setter
    public void SetMyConfirm(bool myConfirm)
    {
        this.myConfirm = myConfirm;
        UIManager.SetPlayerHue(myConfirm);
        StateMachine();
    }

    public void SetEnemyConfirm(bool enemyConfirm)
    {
        this.enemyConfirm = enemyConfirm;
        UIManager.SetEnemyHue(myConfirm);
        StateMachine();
    }

    // Network Sender
    public void SummonCard(int cardID)
    {
        NetworkTrain.networkTrain.SummonCard(cardID);
    }

    public void SendShuffle(int[] cardIndexes)
    {
        NetworkTrain.networkTrain.SendShuffle(cardIndexes);
    }

    public void SendInterfaceSignal(int interfaceSignal)
    {
        NetworkTrain.networkTrain.SendInterfaceSignal(interfaceSignal);
    }

    public void SendCardPosition(int id, Vector2 position)
    {
        NetworkTrain.networkTrain.SendCardPosition(id, position);
    }

    public void SendCardPositionStop()
    {
        NetworkTrain.networkTrain.SendCardPositionStop();
    }

    public void SendUltiPosition(int id, Player playerHovered)
    {
        bool hoveringMyself;
        hoveringMyself = (playerHovered == myPlayer) ? true : false;
        NetworkTrain.networkTrain.SendUltiHover(id, hoveringMyself);
    }

    public void SendUltiStop()
    {
        NetworkTrain.networkTrain.SendUltiStop();
    }

    public void SendUltiPurchase(int cardID, bool bought, int charge)
    {
        if (state == GameState.Purchase && SceneManager.GetActiveScene().buildIndex == 3)
            NetworkTrain.networkTrain.SendUltiPurchase(cardID, bought, charge);
    }

    // Network Receiver
    public void ReceiveShuffle(int[] receivedCardIndexes)
    {
        enemyPlayer.ReceiveShuffle(receivedCardIndexes);
    }

    public void ReceiveSummon() {
        enemyPlayer.ReceiveSummon();
    }

    public void ReceiveCardPosition(int hoverCard, Vector2 hoverPos)
    {
        enemyPlayer.ReceiveCardPosition(hoverCard, hoverPos);
    }

    public void ReceiveCardPositionStop()
    {
        enemyPlayer.ReceiveCardPositionStop();
    }

    public void ReceiveUltiHover(int hoverCard, bool hoveringMyself)
    {
        if (hoveringMyself)
            enemyPlayer.GetUltiArea().BeingHighlighted(hoverCard);
        else
            myPlayer.GetUltiArea().BeingHighlighted(hoverCard);
    }

    public void ReceiveUltiHoverStop()
    {
        enemyPlayer.ReceiveUltiHoverStop();
    }

}
