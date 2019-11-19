using System;
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
        charge = 0;
        sideList = new SideEffect[Constants.maxSideListSize];
    }

    // Update is called once per frame
    // Sending card positions
    void Update()
    {
        if (time >= 0.25f)
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
        this.chargeDisableList = new List<CardTypes>() { CardTypes.Charge };
        this.profile.SetImage(profile);
        this.totalCardCount = cardCount + ultiCount + passiveCount;
        deck.CreateDeck(hero, cardCount, ultiCount, passiveCount);
        ultiArea.CreateUltiArea(hero, cardCount, ultiCount);
        CreateSideEffects(hero, cardCount+ultiCount+passiveCount, sideCount);

        Debug.Log("p created");
    }

    public void CreateSideEffects(HeroEnum hero, int startWithID, int count)
    {
        for (int i = 0; i < count; i++)
            sideList[i] = CardMaker.CM.MakeSideEffect(hero, startWithID+i);
    }

    // Acquire
    public void AcquireCards()
    {
        for (int i = 0; i < Constants.maxUltiAreaSize; i++)
        {
            if (ultiArea.GetUltiCard(i) != null && ultiArea.GetUltiCard(i).GetBought())
            {
                // Acquire DeckCard
                DeckCard correspondingDeckCard = deck.GetDeckCard(ultiArea.GetCard(i).GetID());
                correspondingDeckCard.SetUltiCard(ultiArea.GetUltiCard(i));
                RestoreCard(ultiArea.GetCard(i).GetID());

                // Disable UltiCard
                ultiArea.GetUltiCard(i).SetBought(false);
                ultiArea.GetUltiCard(i).gameObject.SetActive(false);
                ultiArea.RecedeUlti(i);
            }
        }
    }

    // Summon
    public void UnleashCard(DeckCard deckCard, bool received)
    {
        if ((deckCard.GetOutOfHand() || received) && deckCard.GetCard().GetTurnsTill() <= 0)
            if (!deckCard.GetCard().GetIsReaction() && gameOverseer.GetState() == GameState.Choice)
                SummonCard(deckCard, received);
            else if (gameOverseer.GetState() == GameState.Reaction)
                SummonReaction(deckCard, received);
    }

    public void UnleashCard(DeckCard deckCard) { UnleashCard(deckCard, false); }

    public void SummonCard(DeckCard deckCard, bool received)
    {
        if (gameOverseer.GetState() == GameState.Choice && GetCardPlayed() == null)
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
            else {
                g.GetComponent<BoardCard>().RevealAnimation(-2);
            }

            // Networking info
            if (!received)
            {
                gameOverseer.SendUnleashCard(deckCard.GetID());

                // Set confirm to true
                gameOverseer.SetMyConfirm(true);
            }
        }
    }

    public void SummonReaction(DeckCard deckCard, bool received)
    {
        deckCard.OutHover(1f, Constants.cardRiseHeight(deckCard.GetIsMobile()));
        if (received) 
            deckCard.ChangePosition(deck.GetReactionLocation(0));
        else
            deckCard.ChangePosition(deck.GetReactionLocation(0) + new Vector2(0f, Constants.cardRiseHeight(deckCard.GetIsMobile())));
        deckCard.SetIsReaction(true);
        gameOverseer.ReactionEffect(this, deckCard.GetCard());

        // Networking info
        if (!received)
            gameOverseer.SendUnleashCard(deckCard.GetID());
    }

    // Ulti Buy
    public void UltiBuy(UltimateCard uc)
    {
        if (gameOverseer.GetState() == GameState.Purchase && this == gameOverseer.GetMyPlayer())
        {
            if (!uc.GetBought() && GetCharge() >= uc.GetCard().GetCost())
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
            gameOverseer.SendUltiPurchase(uc.GetID(), uc.GetBought(), GetCharge());
        }
    }

    // Restore Card
    public void RestorePlayedCard()
    {
        int RestoringCard() {
            cardPlayed.GetThisDeckCard().gameObject.SetActive(true);
            cardPlayed.GetThisDeckCard().OutHover(1f, Constants.cardRiseHeight(cardPlayed.GetThisDeckCard().GetIsMobile()));
            cardPlayed.GetThisDeckCard().UpdateCardPosition();
            AudioManager.AM.CardSound();
            return 0;
        }

        CheckAndRestore(cardPlayed.GetThisDeckCard(), RestoringCard);
    }

    public void RestoreReactionCard()
    {
        for (int i = 0; i < Constants.maxCardAmount; i++)
        {
            if (deck.GetDeckCard(i) != null && deck.GetDeckCard(i).gameObject.activeInHierarchy && GetCard(i).GetIsReaction())
            {
                int RestoringCard()
                {
                    if (deck.GetDeckCard(i) != null)
                    {
                        deck.GetDeckCard(i).SetIsReaction(false);
                        deck.GetDeckCard(i).UpdateCardPosition();
                    }
                    return 0;
                }

                CheckAndRestore(deck.GetDeckCard(i), RestoringCard);
            }
        }
    }   

    void CheckAndRestore(DeckCard deckCard, Func<int> RestoringCard)
    {
        bool returning = isReturning(deckCard.GetCard());

        // If not returning, recede deck
        if (!returning)
            GetDeck().RecedeDeck(deckCard.GetIndex());

        // If returning ulti, restore ulti card and recede Deck
        else if (deckCard.GetCard().GetCardType() == CardTypes.Ultimate)
        {
            // Recede and Discard
            GetDeck().RecedeDeck(deckCard.GetIndex());
            if (deckCard.GetIsReaction())
            {
                RestoringCard();
                deckCard.gameObject.SetActive(false);
            }

            // Return Ulti
            UltimateCard ultiCard = deckCard.GetUltiCard();

            ultiCard.gameObject.SetActive(true);
            ultiCard.SetTempIndex(GetUltiArea().PlaceUltimate(ultiCard.GetID()));
        }

        // If returning card, restore normal card
        else
        {
            RestoringCard();
        }
    }

    public void RestoreCard(int cardID)
    {
        GetDeckCard(cardID).SetIndex(GetHighestIndex());
        GetDeckCard(cardID).gameObject.SetActive(true);
        GetDeckCard(cardID).UpdateCardPosition();
        AudioManager.AM.CardSound();
    }

    public void RestoreUlti(int cardID)
    {
        GetDeckCard(cardID).GetUltiCard().gameObject.SetActive(true);
        GetDeckCard(cardID).GetUltiCardScript().SetIndex(ultiArea.PlaceUltimate(GetDeckCard(cardID).GetUltiCardScript().GetCard().GetID()));
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

    public bool isReturning(Card card)
    {
        if (card.GetCardType() == CardTypes.Nullification)
        {
            NullInterface cc = (NullInterface)cardPlayed.GetCardPlayed();
            if (cc.wronged == false)
            {
                return true;
            }
        }
        else if (card.GetCardType() != CardTypes.Skill)
        {
            return true;
        }

        return false;
    }

    // Create Card Reader
    public void CreateCardReader(DeckCard card)
    {
        gameOverseer.CreateCardReader(card);
    }

    public void DestroyReader()
    {
        gameOverseer.DestroyReader();
    }

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
        this.protection += protection;
    }

    public void RemoveProtection()
    {
        protection = 0;
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
            if (GetUltiCard(i) != null && GetUltiCard(i).gameObject.activeInHierarchy && GetUltiCard(i).GetCard().GetCost() <= GetCharge())
            {
                return true;
            }
        }

        return false;
    }

    public bool HasReactionCard()
    {
        for (int i = Constants.maxCardAmount-1; i >= 0; i--)
        {
            if (GetCard(i) != null && deck.GetDeckCard(i).isActiveAndEnabled && GetCard(i).GetIsReaction())
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
        if (sideList[index] is SideEffect)
        {
            return sideList[index].GetValue();

        } else {
            return 0;
        }
    }

    public bool GetPredicted()
    {
        return predicted;
    }

    public int GetHighestIndex()
    {
        int index = 0;
        for (int i = 0; i < Constants.maxCardAmount; i++)
        {
            if (GetDeckCard(i) != null && GetDeckCard(i).gameObject.activeInHierarchy && !GetDeckCard(i).GetIsReaction() && GetDeckCard(i).GetIndex() >= index)
                index = GetDeckCard(i).GetIndex()+1;
        }

        return index;
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
        sideList[index].SetValue(value);
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
        for (int i = 0; i < Constants.maxSideListSize; i++)
        {
            if (sideList[i] != null && sideList[i] is Effecter) {
                Effecter SET = (Effecter)sideList[i];
                if (SET.phase == phase) {
                    Debug.Log("Side Effected: " + i);
                    SET.Effect(this, enemy);
                    sideList[i] = (SideEffect)SET;
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
        if (gameOverseer.GetMyPlayer() == this)
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
        gameOverseer.Summary(cardList, null, totalCardCount, this);
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

    public void ReceiveUnleash(int cardID)
    {
        UnleashCard(GetDeckCard(cardID), true);
        Debug.Log("Enemy Player3");
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
