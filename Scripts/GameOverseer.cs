using Photon.Pun;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

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

    [SerializeField]
    private LayerMask layerMask;

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
        // When to hoverCard: (Choice + Predicted Card, Interface, Reaction, Effects) + Not Holding Stuff
        if (((GetState() == GameState.Choice && GetEnemyPlayer().GetPredicted()) || GetState() == GameState.Interface ||
            GetState() == GameState.Reaction || GetState() == GameState.Effects) && !GetMyPlayer().GetDeck().GetHoldingCard() &&
            !interfface.gameObject.activeInHierarchy)
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
            SetEnemyConfirm(false);

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
                    myPlayer.GetBoardCard().RevealAnimation(0);
                    enemyPlayer.GetBoardCard().RevealAnimation(0);
                    ToInterfaceState();
                    break;

                case GameState.Interface:
                    ToReactionState();
                    break;

                case GameState.Reaction:
                    ToEffectsState();
                    break;

                case GameState.Effects:
                    // Restore Played Card
                    myPlayer.RestorePlayedCard();
                    enemyPlayer.RestorePlayedCard();
                    // Darken Ulti Cards
                    myPlayer.DarkenUltiCards(false);
                    enemyPlayer.DarkenUltiCards(false);
                    // Destroy Board Cards
                    Destroy(myPlayer.GetBoardCard().gameObject);
                    Destroy(enemyPlayer.GetBoardCard().gameObject);
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

        // Color
        for (int i = 0; i < Constants.maxCardAmount; i++)
        {
            if (myPlayer.GetCard(i) != null && myPlayer.GetDeckCard(i) != null &&
                myPlayer.GetCard(i).GetTurnsTill() <= 0)
            {
                myPlayer.GetDeckCard(i).SetDarkened(false);
            }
        }
    }

    private void ToInterfaceState()
    {
        state = GameState.Interface;
        UIManager.DebugText("Interface");

        // Activate interface if card has one, otherwise skip
        if (myPlayer.GetCardPlayed() is Interfacer)
        {
            Interfacer inter = (Interfacer)myPlayer.GetCardPlayed();
            inter.Interfacing(myPlayer, enemyPlayer);
        } else
        {
            SetMyConfirm(true);
        }
        
        // Color
        for (int i = 0; i < Constants.maxCardAmount; i++)
        {
            if (myPlayer.GetCard(i) != null && myPlayer.GetDeckCard(i) != null)
            {
                myPlayer.GetDeckCard(i).SetDarkened(true);
            }
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
        } else
        {
            // Color
            for (int i = 0; i < Constants.maxCardAmount; i++)
            {
                if (myPlayer.GetCard(i) != null && myPlayer.GetDeckCard(i) != null &&
                    myPlayer.GetCard(i).GetIsReaction())
                {
                    myPlayer.GetDeckCard(i).SetDarkened(false);
                }
            }
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
        EveryTurn();
        ActivateCards();

        // Activate Side Effects
        myPlayer.ActivateSideEffects(SEPhase.EffectsAfter, enemyPlayer);
        enemyPlayer.ActivateSideEffects(SEPhase.EffectsAfter, myPlayer);

        // Color
        for (int i = 0; i < Constants.maxCardAmount; i++)
        {
            if (myPlayer.GetCard(i) != null && myPlayer.GetDeckCard(i) != null)
            {
                myPlayer.GetDeckCard(i).SetDarkened(true);
            }
        }

    }

    // Every turn stuff
    private void EveryTurn()
    {
        // Reduce Unplayability
        reduceUnplayability(myPlayer);
        reduceUnplayability(enemyPlayer);

        // Reset Card Limit (zB Attack, Charge)
        resetLimit(myPlayer);
        resetLimit(enemyPlayer);
    }

    public void reduceUnplayability(Player player)
    {
        for (int i = 0; i < Constants.maxCardAmount; i++)
        {
            if (player.GetCard(i) != null)
            {
                if (player.GetCard(i).GetTurnsTill() > 0)
                {
                    player.GetCard(i).ReduceTurnsTill();
                }
            }
        }
    }

    public void resetLimit(Player player)
    {
        for (int i = 0; i < Constants.maxCardAmount; i++)
        {
            if (player.GetCard(i) as Limit != null)
            {
                if (player.GetCard(i).GetID() != player.GetCardPlayed().GetID())
                {
                    Limit cc = player.GetCard(i) as Limit;
                    cc.limit = 0;
                    player.SetCard(cc as Card, i);
                    //Debug.Log("Card Limit reset: " + cardList[i].name);
                }
            }
        }
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
    public void Interfacing(Card[] cardList, Card invoker, int cardAmount)
    {
        interfface.gameObject.SetActive(true);
        interfface.Setup(cardList, invoker, cardAmount);
    }

    public void Interfacing(Card baseCard, string[] textList, Card invoker, int cardAmount)
    {
        interfface.gameObject.SetActive(true);
        interfface.Setup(baseCard, textList, invoker, cardAmount);
    }

    public void HoverCarding()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        Physics.Raycast(mouseRay, out hitInfo, 300, layerMask, QueryTriggerInteraction.Collide);

        if (hitInfo.collider != null)
        {
            if (hitInfo.collider.transform.parent != null && destroyList.Count <= 0)
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
            destroyList.Clear();
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
