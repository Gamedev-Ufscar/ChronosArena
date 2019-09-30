using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Deck deck;
    [SerializeField]
    private UltiArea ultiArea;
    [SerializeField]
    private GameObject ultiPrefab;
    [SerializeField]
    private GameObject boardPrefab;
    [SerializeField]
    private GameOverseer gameOverseer;

    private int HP = 10;
    private int Charge = 4;
    private int protection = 0;
    private HeroEnum hero;
    private List<CardTypes> attackDisableList = new List<CardTypes>();
    private List<CardTypes> chargeDisableList = new List<CardTypes>() { CardTypes.Charge };
    private SideEffect[] sideList = new SideEffect[Constants.maxSideListSize];
    private BoardCard cardPlayed = null;
    private bool predicted = false;

    [SerializeField]
    private SlotsOnBoard boardSlot;
    [SerializeField]
    private Profile profile;

    private int? sentCardID = null;
    private Vector2 sentCardPosition;
    private int? formerCardID = null;
    private Vector2 formerCardPosition;

    private int? sentUltiID = null;
    private bool sentUlti = false;

    private float time = 0f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    // Sending card positions
    void Update()
    {
        if (time >= 0.2f)
        {
            // Check if there's an update
            if (sentCardID != formerCardID || sentCardPosition != formerCardPosition)
            {
                // Send 'card position'
                if (sentCardID != null)
                {
                    SendCardPosition((int)sentCardID, sentCardPosition);
                }
                // Send 'no card selected'
                else
                {
                    SendCardPositionStop();
                }

                formerCardID = sentCardID;
                formerCardPosition = sentCardPosition;
            }

            // Send 'ulti hover'
            if (sentUlti && sentUltiID != null)
            {
                SendUltiHover((int)sentUltiID);
                sentUlti = false;
            }

            // Send 'ulti stop'
            else
            {
                SendUltiStop(this);
            }

            // Time stuff
            time = 0f;
        } else {
            time += Time.deltaTime;
        }
    }

    // Constructor
    public Player(GameOverseer gameOverseer)
    {
        this.gameOverseer = gameOverseer;
    }

    // Creator
    public void CreatePlayer(HeroEnum hero, int cardCount, int ultiCount, int passiveCount, List<CardTypes> attackDisableList, Sprite profile)
    {
        this.hero = hero;
        this.attackDisableList = attackDisableList;
        this.profile.SetImage(profile);
        deck.CreateDeck(hero, cardCount, ultiCount, passiveCount);
        ultiArea.CreateUltiArea(hero, cardCount, ultiCount);
        CreateSummary();
    }

    public void CreateSummary()
    {

    }

    public void AcquireCards()
    {
        for (int i = 0; i < Constants.maxUltiAreaSize; i++)
        {
            if (ultiArea.GetUltiCard(i).GetBought())
            {
                deck.GetDeckCard(ultiArea.GetCard(i).GetID()).gameObject.SetActive(true);
                ultiArea.GetUltiCard(i).gameObject.SetActive(false);
                ultiArea.RecedeUlti(i);
            }

            // CHECK LATER
            //GameOverseer.GO.enemyUltiBuy[staticCardIndex - 100] = false;
            AudioManager.AM.CardSound();
        }
    }

    // Summon
    public void SummonCard(DeckCard deckCard)
    {
        if (gameOverseer.GetState() == GameState.Choice)
        {
            // Instantiate Board Card
            Vector3 v = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 12f));
            GameObject g = Instantiate(boardPrefab, new Vector3(v.x, v.y, v.z), Quaternion.LookRotation(Vector3.back, Vector3.down));

            // Set as Card Played
            SetCardPlayed(g.GetComponent<BoardCard>());

            // Modify and Initiate variables
            if (deckCard.GetCard().GetCardType() == CardTypes.Ultimate)
            {
                cardPlayed.ConstructBoardCard(deckCard.GetCard(), this, deckCard.GetUltiCard());
            }
            else
            {
                cardPlayed.ConstructBoardCard(deckCard.GetCard(), this, deckCard.gameObject);
            }

            deck.SetHoldingCard(false);

            // Activate slot
            if (predicted == false) { g.GetComponent<BoardCard>().Activate(boardSlot, false); }
            else { g.GetComponent<BoardCard>().Activate(boardSlot, true); }

            // Networking info
            gameOverseer.SummonCard(deckCard.GetID());
        }
    }

    // Restore Card
    public void RestoreCard(int cardId)
    {
        GetDeckCard(cardId).gameObject.SetActive(true);
        GetDeckCard(cardId).SetIndex(GetActiveCardCount());
    }

    public void RestoreUlti(int cardId)
    {
        GetDeckCard(cardId).GetUltiCard().SetActive(true);
        GetDeckCard(cardId).GetUltiCardScript().SetIndex(ultiArea.PlaceUltimate(GetDeckCard(cardId).GetUltiCardScript().GetCard().GetID()));
    }

    // Discard Card
    public void DiscardCard(int cardId)
    {
        if (GetCard(cardId).GetCardType() == CardTypes.Ultimate)
        {
            RestoreUlti(cardId);
        }
        GetDeckCard(cardId).gameObject.SetActive(false);
        deck.RecedeDeck(cardId);
    }

    // Change Variables
    public void DealDamage(int damage, bool isUnblockable)
    {
        if (isUnblockable) {
            HP -= damage;
        } else {
            if (damage - protection > 0)
                HP -= (damage - protection);
        }
    }

    public void Protect(int protection)
    {
        if (protection > this.protection)
        {
            this.protection = protection;
        }
    }

    public void RaiseCharge(int charge)
    {
        Charge += charge;
        if (Charge < 0) { Charge = 0; }
    }

    public void Heal(int heal)
    {
        HP += heal;
        if (HP > 10) { HP = 10; }
    }

    // Getters
    public int GetHP()
    {
        return HP;
    }

    public int GetCharge()
    {
        return Charge;
    }

    public List<CardTypes> GetAttackDisable()
    {
        return attackDisableList;
    }

    public List<CardTypes> GetChargeDisable()
    {
        return chargeDisableList;
    }

    public Deck GetDeck()
    {
        return deck;
    }

    public UltiArea GetUltiArea()
    {
        return ultiArea;
    }

    public HeroEnum GetHero()
    {
        return hero;
    }

    public DeckCard GetDeckCard(int cardID)
    {
        return GetDeck().GetDeckCard(cardID);
    }

    public Card GetCard(int cardID)
    {
        if (GetDeckCard(cardID) == null)
        {
            return null;
        }
        else
        {
            return GetDeckCard(cardID).GetCard();
        }
    }

    public Card GetCardPlayed()
    {
        if (cardPlayed == null)
        {
            return null;
        }
        else
        {
            return cardPlayed.GetCardPlayed();
        }
    }

    public UltimateCard GetUltiCard(int i)
    {
        return ultiArea.GetUltiCard(i);
    }

    public SideEffect GetSideEffect(int index)
    {
        return sideList[index];
    }

    public int GetSideEffectValue(int index)
    {
        if (sideList[index] is SideEffectTimed)
        {
            SideEffectTimed set = (SideEffectTimed)sideList[index];
            return set.GetTimer();

        } else if (sideList[index] is SideEffectVariable) {

            SideEffectVariable sev = (SideEffectVariable)sideList[index];
            return sev.GetVariable();

        } else {
            return 0;
        }
    }

    public bool GetPredicted()
    {
        return predicted;
    }

    public bool HasReactionCard()
    {
        for (int i = 0; i <= 10; i++)
        {
            if (GetCard(i).GetIsReaction() == true)
            {
                return true;
            }
        }
        return false;
    }

    public int GetActiveCardCount()
    {
        int count = 0;
        for (int i = 0; i < Constants.maxCardAmount; i++)
        {
            if (GetDeckCard(i) != null && GetDeckCard(i).GetIndex() > count)
                count = GetDeckCard(i).GetIndex();
        }

        return count+1;
    }

    public bool GetHoldingCard()
    {
        return deck.GetHoldingCard();
    }

    // Setters
    public void SetHP(int HP)
    {
        this.HP = HP;
    }

    public void SetCharge(int Charge)
    {
        this.Charge = Charge;
    }

    public void SetCard(Card card, int cardID)
    {
        if (GetDeckCard(cardID) == null)
        {
            GetDeckCard(cardID).SetCard(card);
        }
    }

    public void SetCardPlayed(BoardCard cardPlayed)
    {
        this.cardPlayed = cardPlayed;
        gameOverseer.UpdatedCardPlayed(this, true);
    }

    public void SetPredicted(bool predicted)
    {
        this.predicted = predicted;
    }

    public void SetSideEffect(int index, int value)
    {
        if (sideList[index] is SideEffectTimed) {
            SideEffectTimed set = (SideEffectTimed)sideList[index];
            set.SetTimer(value);
            sideList[index] = (SideEffect)set;
        } else if (sideList[index] is SideEffectVariable)
        {
            SideEffectVariable sev = (SideEffectVariable)sideList[index];
            sev.SetVariable(value);
            sideList[index] = (SideEffect)sev;
        }
    }

    public void SetCardPosition(int? id, Vector2 position)
    {
        sentCardID = id;
        if (sentCardID != null)
            sentCardPosition = position;
    }

    public void SetUltiToSend(int id)
    {
        sentUltiID = id;
        sentUlti = true;
    }

    // Side Effects
    public void ActivateSideEffects(SEPhase phase, Player enemy)
    {
        foreach (SideEffect SE in sideList)
        {
            if (SE is SideEffectTimed) {
                SideEffectTimed SET = (SideEffectTimed)SE;
                if (SET.GetTimer() > 0) {
                    if (SE.GetPhase() == phase) {
                        SET.Effect(this, enemy);
                    }
                }
            }
        }
    }

    // Interfacing
    public void Interfacing(Card[] cardList, Card invoker)
    {
        gameOverseer.Interfacing(cardList, invoker);
    }

    public void Interfacing(Card baseCard, string[] textList, Card invoker)
    {
        gameOverseer.Interfacing(baseCard, textList, invoker);
    }

    // Shuffle
    public void OnShufflePress()
    {
        deck.Shuffle();
    }

    // Summary
    public void InvokeSummary()
    {
        Card[] cardList = new Card[Constants.maxCardAmount];
        for (int i = 0; i < Constants.maxCardAmount; i++)
        {
            cardList[i] = GetDeckCard(i).GetCard();
        }
        gameOverseer.Interfacing(cardList, null);
    }

    // Purchase
    public void PurchaseCards()
    {

    }

    // Sender
    public void SendShuffle(int[] cardIndexes)
    {
        gameOverseer.SendShuffle(cardIndexes);
    }

    public void SendCardPosition(int? id, Vector2 position)
    {
        if (id != null)
            gameOverseer.SendCardPosition((int)id, position);
        else
            gameOverseer.SendCardPositionStop();
    }

    public void SendCardPositionStop()
    {
        gameOverseer.SendCardPositionStop();
    }

    public void SendUltiHover(int id)
    {
        gameOverseer.SendUltiPosition(id, this);
    }

    public void SendUltiPurchase(int cardID, bool bought)
    {
        gameOverseer.SendUltiPurchase(cardID, bought, GetCharge());
    }

    public void SendUltiStop(Player player)
    {
        gameOverseer.SendUltiStop(player);
    }

    // Network Receiver
    public void ReceiveShuffle(int[] receivedCardIndexes)
    {
        deck.UpdateCardPositions(receivedCardIndexes);
    }

    public void ReceiveSummon()
    {
        SummonCard(GetDeckCard((int)sentCardID));
    }

    public void ReceiveCardPosition(int? hoverCard, Vector2 hoverPos)
    {
        deck.ReceiveCardPosition(hoverCard, hoverPos);
    }

    public void ReceiveUltiHover(int? cardID)
    {
        UltimateCard uc = ultiArea.GetUltiCard(cardID);
        if (uc != null)
            uc.OnHover();
    }
}
