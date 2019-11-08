using Photon.Pun;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    [SerializeField]
    private TutorialOverseer tutorialOverseer;
    [SerializeField]
    private VictoryAnimation victAnim;

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
        if (SceneManager.GetActiveScene().buildIndex == (int)SceneList.Game)
        {
            if (NetworkBahn.networkBahn != null)
            {
                Destroy(NetworkBahn.networkBahn.gameObject);
            }
            GameObject netObject = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Test2"), Vector3.zero, Quaternion.identity);
            netObject.GetComponent<NetworkBahn>().GiveGameOverseer(this);
        }

        state = GameState.Choice;
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
                    // Restore played card
                    myPlayer.RestorePlayedCard();
                    enemyPlayer.RestorePlayedCard();

                    // Restore reaction cards
                    myPlayer.RestoreReactionCard();
                    enemyPlayer.RestoreReactionCard();

                    // Revoke nullification
                    myPlayer.GetCardPlayed().SetIsNullified(false);
                    enemyPlayer.GetCardPlayed().SetIsNullified(false);
                    // Remove protection
                    myPlayer.RemoveProtection();
                    enemyPlayer.RemoveProtection();

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

    private void ToPurchaseState()
    {
        state = GameState.Purchase;
        UIManager.DebugText("Compra");

        // If I can't buy cards, just skip
        if (!myPlayer.CanBuyCards())
        {
            SetMyConfirm(true);
            if (SceneManager.GetActiveScene().buildIndex == (int)SceneList.Tutorial)
                SetEnemyConfirm(true);
        }

        CheckVictory();

    }

    private void ToChoiceState()
    {
        state = GameState.Choice;
        UIManager.DebugText("Escolha");
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
        UIManager.DebugText("Decisão");

        // Activate interface if card has one, otherwise skip
        if (myPlayer.GetCardPlayed() is Interfacer)
        {
            Interfacer inter = (Interfacer)myPlayer.GetCardPlayed();
            inter.Interfacing(myPlayer, enemyPlayer, true);
        } else
        {
            SetMyConfirm(true);
            if (SceneManager.GetActiveScene().buildIndex == (int)SceneList.Tutorial)
                SetEnemyConfirm(true);
        }

        if (enemyPlayer.GetCardPlayed() is Interfacer)
        {
            Interfacer inter = (Interfacer)enemyPlayer.GetCardPlayed();
            inter.Interfacing(enemyPlayer, myPlayer, false);
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
        UIManager.DebugText("Reação");

        myPlayer.ActivateSideEffects(SEPhase.Reaction, enemyPlayer);
        enemyPlayer.ActivateSideEffects(SEPhase.Reaction, myPlayer);

        // If I can't play reactions, just skip
        if (!myPlayer.HasReactionCard())
        {
            SetMyConfirm(true);
            if (SceneManager.GetActiveScene().buildIndex == (int)SceneList.Tutorial)
                SetEnemyConfirm(true);
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
        UIManager.DebugText("Efeitos");
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
        ReduceUnplayability(myPlayer);
        ReduceUnplayability(enemyPlayer);

        // Reset Card Limit (zB Attack, Charge)
        ResetLimit(myPlayer);
        ResetLimit(enemyPlayer);
    }

    public void ReduceUnplayability(Player player)
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

    public void ResetLimit(Player player)
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

        for (int e = Mathf.Min(myPlayer.GetCardPlayed().GetMinOrMax(true), 
            enemyPlayer.GetCardPlayed().GetMinOrMax(true));
            e <= Mathf.Max(myPlayer.GetCardPlayed().GetMinOrMax(false), 
            enemyPlayer.GetCardPlayed().GetMinOrMax(false));
            e++)
        {
            if (!myPlayer.GetCardPlayed().GetIsNullified())
                myPlayer.GetCardPlayed().Effect(myPlayer, enemyPlayer, e);
            if (!enemyPlayer.GetCardPlayed().GetIsNullified())
                enemyPlayer.GetCardPlayed().Effect(enemyPlayer, myPlayer, e);
        }
    }

    private void CheckVictory()
    {
        if (GetMyPlayer().GetCharge() >= 10 && GetMyPlayer().GetCharge() > GetEnemyPlayer().GetCharge())
        {
            victAnim.gameObject.SetActive(true);
            victAnim.ActivateAnim("Vitória");
            midButton.gameObject.SetActive(false);
        } else if (GetEnemyPlayer().GetCharge() >= 10 && GetEnemyPlayer().GetCharge() > GetMyPlayer().GetCharge()) {
            victAnim.gameObject.SetActive(true);
            victAnim.ActivateAnim("Derrota");
            midButton.gameObject.SetActive(false);
        }
        else if (GetMyPlayer().GetHP() <= 0 || GetEnemyPlayer().GetHP() <= 0)
        {
            if (GetMyPlayer().GetHP() > GetEnemyPlayer().GetHP())
            {
                victAnim.gameObject.SetActive(true);
                victAnim.ActivateAnim("Vitória");
                midButton.gameObject.SetActive(false);
            } else if (GetMyPlayer().GetHP() < GetEnemyPlayer().GetHP())
            {
                victAnim.gameObject.SetActive(true);
                victAnim.ActivateAnim("Derrota");
                midButton.gameObject.SetActive(false);
            } else
            {
                victAnim.gameObject.SetActive(true);
                victAnim.ActivateAnim("Empate");
                midButton.gameObject.SetActive(false);
            }
        }
    }

    public void EndGame()
    {
        Destroy(AudioManager.AM.gameObject);
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene((int)SceneList.Menu);
    }

    public void ReactionEffect(Player player, Card card)
    {
        if (player == myPlayer)
            card.Effect(myPlayer, enemyPlayer, 0);
        else
            card.Effect(enemyPlayer, myPlayer, 0);
    }

    // Healthbar & Chargebar
    public void UpdateBar()
    {
        UIManager.UpdateBar(GetMyPlayer(), GetEnemyPlayer());
    }

    // Force Hand Card Hover
    public void ForceHandCardHover(int id)
    {
        myPlayer.GetDeckCard(id).OnHover(Constants.cardBigSize, Constants.cardRiseHeight);
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

    public void Summary(Card[] cardList, Card invoker, int cardAmount, Player player)
    {
        interfface.gameObject.SetActive(true);
        if (player == GetMyPlayer()) { interfface.GetPlayerBackButton().SetActive(true); }
        else if (player == GetEnemyPlayer()) { interfface.GetEnemyBackButton().SetActive(true); }
        interfface.Setup(cardList, invoker, cardAmount);
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

    // Network/Tutorial Sender
    public void SendUnleashCard(int cardID)
    {
        if (NetworkBahn.networkBahn != null)
            NetworkBahn.networkBahn.UnleashCard(cardID);

        if (SceneManager.GetActiveScene().buildIndex == (int)SceneList.Tutorial)
        {
            tutorialOverseer.ReceiveCardPlayed(cardID);
            Debug.Log("Send Summon Card");
        }
    }

    public void SendConfirm(bool confirm)
    {
        if (NetworkBahn.networkBahn != null)
            NetworkBahn.networkBahn.SendConfirm(confirm);

        if (SceneManager.GetActiveScene().buildIndex == (int)SceneList.Tutorial)
            tutorialOverseer.ReceiveCardPlayed(1000);
    }

    public void SendShuffle(int[] cardIndexes)
    {
        if (NetworkBahn.networkBahn != null)
            NetworkBahn.networkBahn.SendShuffle(cardIndexes);
    }

    public void SendInterfaceSignal(int interfaceSignal)
    {
        if (NetworkBahn.networkBahn != null)
            NetworkBahn.networkBahn.SendInterfaceSignal(interfaceSignal);
    }

    public void SendCardPosition(int id, Vector2 position)
    {
        if (NetworkBahn.networkBahn != null)
            NetworkBahn.networkBahn.SendCardPosition(id, position);
    }

    public void SendCardPositionStop()
    {
        if (NetworkBahn.networkBahn != null)
            NetworkBahn.networkBahn.SendCardPositionStop();
    }

    public void SendUltiPosition(int id, Player playerHovered)
    {
        if (NetworkBahn.networkBahn != null)
        {
            bool hoveringMyself;
            hoveringMyself = (playerHovered == myPlayer) ? true : false;
            NetworkBahn.networkBahn.SendUltiHover(id, hoveringMyself);
        }
    }

    public void SendUltiStop(Player playerHovered)
    {
        if (NetworkBahn.networkBahn != null)
        {
            bool hoveringMyself;
            hoveringMyself = (playerHovered == myPlayer) ? true : false;
            NetworkBahn.networkBahn.SendUltiStop(hoveringMyself);
        }
    }

    public void SendUltiPurchase(int cardID, bool bought, int charge)
    {
        if (NetworkBahn.networkBahn != null)
            NetworkBahn.networkBahn.SendUltiPurchase(cardID, bought, charge);

        if (SceneManager.GetActiveScene().buildIndex == (int)SceneList.Tutorial)
            tutorialOverseer.ReceiveCardPlayed(3000);
    }

    // Network Receiver
    public void ReceiveShuffle(int[] receivedCardIndexes)
    {
        enemyPlayer.ReceiveShuffle(receivedCardIndexes);
    }

    public void ReceiveUnleash(int cardID) {
        enemyPlayer.ReceiveUnleash(cardID);
        Debug.Log("Receive Summon2");
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
