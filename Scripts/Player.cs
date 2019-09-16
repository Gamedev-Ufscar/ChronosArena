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
    private Profile profile;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
        CreateSummary();
        deck.CreateDeck(hero, cardCount, ultiCount, passiveCount);
        ultiArea.CreateUltiArea(hero, cardCount, ultiCount);
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
                deck.GetHandCard(ultiArea.GetCard(i).GetID()).gameObject.SetActive(true);
                ultiArea.GetUltiCard(i).gameObject.SetActive(false);
                ultiArea.RecedeUlti(i);
            }

            // CHECK LATER
            //GameOverseer.GO.enemyUltiBuy[staticCardIndex - 100] = false;
            AudioManager.AM.CardSound();
        }
    }

    // Summon
    public void SummonCard(HandCard handCard)
    {
        // Instantiate Board Card
        Vector3 v = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 12f));
        GameObject g = Instantiate(boardPrefab, new Vector3(v.x, v.y, v.z), Quaternion.LookRotation(Vector3.back, Vector3.down));

        // Set as Card Played
        SetCardPlayed(g.GetComponent<BoardCard>());

        // Modify and Initiate variables
        if (handCard.GetCard().GetCardType() == CardTypes.Ultimate)
        {
            cardPlayed.ConstructBoardCard(handCard.GetCard(), this, handCard.GetUltiCard());
        }
        else
        {
            cardPlayed.ConstructBoardCard(handCard.GetCard(), this, handCard.gameObject);
        }

        deck.SetHoldingCard(false);

        // Activate slot
        if (predicted == false) { g.GetComponent<CardInBoard>().Activate(SlotsOnBoard.PlayerCard, false); }
        else { g.GetComponent<CardInBoard>().Activate(SlotsOnBoard.PlayerCard, true); }
    }

    // Restore Card
    public void RestoreCard(int cardId)
    {
        GetHandCard(cardId).gameObject.SetActive(true);
        GetHandCard(cardId).SetIndex(GetActiveCardCount());
    }

    public void RestoreUlti(int cardId)
    {
        GetHandCard(cardId).GetUltiCard().SetActive(true);
        GetHandCard(cardId).GetUltiCardScript().SetIndex(ultiArea.PlaceUltimate(GetHandCard(cardId).GetUltiCardScript().GetCard().GetID()));
    }

    // Discard Card
    public void DiscardCard(int cardId)
    {
        if (GetCard(cardId).GetCardType() == CardTypes.Ultimate)
        {
            RestoreUlti(cardId);
        }
        GetHandCard(cardId).gameObject.SetActive(false);
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

    public HeroEnum GetHero()
    {
        return hero;
    }

    public HandCard GetHandCard(int cardID)
    {
        return GetDeck().GetHandCard(cardID);
    }

    public Card GetCard(int cardID)
    {
        if (GetHandCard(cardID) == null)
        {
            return null;
        }
        else
        {
            return GetHandCard(cardID).GetCard();
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
            if (GetHandCard(i) != null && GetHandCard(i).GetIndex() > count)
                count = GetHandCard(i).GetIndex();
        }

        return count+1;
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
        if (GetHandCard(cardID) == null)
        {
            GetHandCard(cardID).SetCard(card);
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
            cardList[i] = GetHandCard(i).GetCard();
        }
        gameOverseer.Interfacing(cardList, null);
    }

    // Purchase
    public void PurchaseCards()
    {

    }

    // Network Sender
    public void SendShuffle(int[] cardIndexes)
    {
        gameOverseer.SendShuffle(cardIndexes);
    }

    // Network Receiver
    public void ReceiveShuffle(int[] receivedCardIndexes)
    {
        deck.UpdateCardPositions(receivedCardIndexes);
    }
}
