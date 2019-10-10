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
    private int charge = 4;
    private int protection = 0;
    private HeroEnum hero;
    private List<CardTypes> attackDisableList = new List<CardTypes>();
    private List<CardTypes> chargeDisableList = new List<CardTypes>() { CardTypes.Charge };
    private SideEffect[] sideList = new SideEffect[Constants.maxSideListSize];
    private int totalCardCount = 0;
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
        HP = 10;
        charge = 4;
        sideList = new SideEffect[Constants.maxSideListSize];
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
    public void CreatePlayer(HeroEnum hero, int cardCount, int ultiCount, int passiveCount, int sideCount, List<CardTypes> attackDisableList, Sprite profile)
    {
        this.hero = hero;
        this.attackDisableList = attackDisableList;
        this.profile.SetImage(profile);
        this.totalCardCount = cardCount + ultiCount + passiveCount;
        deck.CreateDeck(hero, cardCount, ultiCount, passiveCount);
        ultiArea.CreateUltiArea(hero, cardCount, ultiCount);
        CreateSideEffects(hero, cardCount+ultiCount+passiveCount, sideCount);
        CreateSummary();
    }

    public void CreateSideEffects(HeroEnum hero, int startWithID, int count)
    {
        for (int i = 0; i < count; i++)
            sideList[i] = CardMaker.CM.MakeSideEffect(hero, startWithID+i);
    }

    public void CreateSummary()
    {

    }

    public void AcquireCards()
    {
        for (int i = 0; i < Constants.maxUltiAreaSize; i++)
        {
            if (ultiArea.GetUltiCard(i) != null && ultiArea.GetUltiCard(i).GetBought())
            {
                // Acquire DeckCard
                DeckCard correspondingDeckCard = deck.GetDeckCard(ultiArea.GetCard(i).GetID());
                correspondingDeckCard.gameObject.SetActive(true);
                correspondingDeckCard.SetUltiCard(ultiArea.GetUltiCard(i));

                // Disable UltiCard
                ultiArea.GetUltiCard(i).SetBought(false);
                ultiArea.GetUltiCard(i).gameObject.SetActive(false);
                ultiArea.RecedeUlti(i);
            }

            AudioManager.AM.CardSound();
        }
    }

    // Summon
    public void SummonCard(DeckCard deckCard) { SummonCard(deckCard, false); }

    public void SummonCard(DeckCard deckCard, bool received)
    {
        if (gameOverseer.GetState() == GameState.Choice)
        {
            Debug.Log("Summoned Card");

            // Instantiate Board Card
            Vector3 v = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 12f));
            GameObject g = Instantiate(boardPrefab, new Vector3(v.x, v.y, v.z), Quaternion.LookRotation(Vector3.back, Vector3.down));

            // Modify and Initiate variables
            if (deckCard.GetCard().GetCardType() == CardTypes.Ultimate)
            {
                g.GetComponent<BoardCard>().ConstructBoardCard(deckCard.GetCard(), this, deckCard, deckCard.GetUltiCard());
            }
            else
            {
                g.GetComponent<BoardCard>().ConstructBoardCard(deckCard.GetCard(), this, deckCard);
            }

            // Set as Card Played
            SetCardPlayed(g.GetComponent<BoardCard>());

            deck.SetHoldingCard(false);

            // Disable HandCard
            deckCard.gameObject.SetActive(false);

            // Activate slot
            if (predicted == false) { g.GetComponent<BoardCard>().RevealAnimation(-1); }
            else { g.GetComponent<BoardCard>().RevealAnimation(-2); }

            // Networking info
            if (received == false)
            {
                gameOverseer.SendSummonCard(deckCard.GetID());

                // Set confirm to true
                gameOverseer.SetMyConfirm(true);
            }

            
        }
    }

    // Ulti Buy
    public void UltiBuy(UltimateCard uc)
    {
        if (gameOverseer.GetState() == GameState.Purchase && this == gameOverseer.GetMyPlayer() && GetCharge() >= uc.GetCard().GetCost())
        {
            if (!uc.GetBought())
            {
                uc.SetBought(true);
                RaiseCharge(-uc.GetCard().GetCost());
            }
            else
            {
                uc.SetBought(false);
                RaiseCharge(uc.GetCard().GetCost());
            }

            uc.UpdateColor();
        }

        gameOverseer.SendUltiPurchase(uc.GetID(), uc.GetBought(), GetCharge());
    }

    // Restore Card
    public void RestorePlayedCard()
    {
        bool returning = false;

        // When will I return?
        if (cardPlayed.GetCardPlayed().GetCardType() == CardTypes.Nullification)
        {
            NullInterface cc = (NullInterface)cardPlayed.GetCardPlayed();
            if (cc.wronged == false)
            {
                returning = true;
            }
        } else if (cardPlayed.GetCardPlayed().GetCardType() != CardTypes.Skill)
        {
            returning = true;
        }

        // ---------------------------

        // If not returning, recede deck
        if (!returning)
        {
            GetDeck().RecedeDeck(cardPlayed.GetThisDeckCard().GetIndex());
        }

        // If returning ulti, restore ulti card and recede Deck
        else if (cardPlayed.GetCardPlayed().GetCardType() == CardTypes.Ultimate)
        {
            GetDeck().RecedeDeck(cardPlayed.GetThisDeckCard().GetIndex());

            UltimateCard ultiCard = cardPlayed.GetThisUltiCard();

            ultiCard.gameObject.SetActive(true);
            ultiCard.SetTempIndex(GetUltiArea().PlaceUltimate(ultiCard.GetID()));
        }

        // If returning card, restore normal card
        else
        {
            cardPlayed.GetThisDeckCard().gameObject.SetActive(true);
            cardPlayed.GetThisDeckCard().OutHover();
            cardPlayed.GetThisDeckCard().UpdateCardPosition();
        }
    }

    public void RestoreCard(int cardId)
    {
        GetDeckCard(cardId).gameObject.SetActive(true);
        GetDeckCard(cardId).SetIndex(GetActiveCardCount());
    }

    public void RestoreUlti(int cardId)
    {
        GetDeckCard(cardId).GetUltiCard().gameObject.SetActive(true);
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

    // Darken Cards


    // Change Variables
    public void DealDamage(int damage, bool isUnblockable)
    {
        if (isUnblockable) {
            SetHP(HP - damage);
        } else {
            if (damage - protection > 0)
                SetHP(HP - (damage - protection));
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
        if (this.charge + charge < 0) { SetCharge(0); }
        else { SetCharge(this.charge + charge); }
    }

    public void Heal(int heal)
    {
        if (HP + heal > 10) { SetHP(10); }
        else { SetHP(HP + heal); }
    }

    // Checkers
    public bool CanBuyCards()
    {
        for (int i = 0; i < Constants.maxUltiAreaSize; i++)
        {
            if (GetUltiCard(i) != null && GetUltiCard(i).GetCard().GetCost() <= GetCharge())
            {
                return true;
            }
        }

        return false;
    }

    public bool HasReactionCard()
    {
        for (int i = 0; i <= Constants.maxHandSize; i++)
        {
            if (GetCard(i) != null && GetCard(i).GetIsReaction() == true)
            {
                return true;
            }
        }
        return false;
    }

    // Getters
    public int GetHP()
    {
        return HP;
    }

    public int GetCharge()
    {
        return charge;
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

    public BoardCard GetBoardCard()
    {
        return cardPlayed;
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

    public int GetTotalCardCount()
    {
        return totalCardCount;
    }

    public bool GetHoldingCard()
    {
        return deck.GetHoldingCard();
    }

    // Setters
    public void SetHP(int HP)
    {
        this.HP = HP;
        gameOverseer.UpdateBar();
    }

    public void SetCharge(int charge)
    {
        this.charge = charge;
        gameOverseer.UpdateBar();
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
        for(int i = 0; i < Constants.maxSideListSize; i++)
        {
            if (sideList[i] != null && sideList[i] is SideEffectTimed) {
                SideEffectTimed SET = (SideEffectTimed)sideList[i];
                if (SET.GetTimer() > 0 && sideList[i].GetPhase() == phase) {
                    SET.Effect(this, enemy);
                }
            }
        }
    }

    // Interfacing
    public void Interfacing(Card[] cardList, Card invoker, int cardAmount)
    {
        gameOverseer.Interfacing(cardList, invoker, cardAmount);
    }

    public void Interfacing(Card baseCard, string[] textList, Card invoker, int cardAmount)
    {
        gameOverseer.Interfacing(baseCard, textList, invoker, cardAmount);
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
            if (GetDeckCard(i) != null)
                cardList[i] = GetDeckCard(i).GetCard();
        }
        gameOverseer.Interfacing(cardList, null, totalCardCount);
    }

    // Darken Ulti cards
    public void DarkenUltiCards(bool darkened)
    {
        ultiArea.DarkenUltiCards(darkened);
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

    public void ReceiveSummon(int cardID)
    {
        SummonCard(GetDeckCard(cardID), true);
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

    public void ReceiveUltiPurchase(int cardID, bool bought, int charge)
    {
        GetUltiCard(cardID).SetBought(bought);
        SetCharge(charge);
    }
}
