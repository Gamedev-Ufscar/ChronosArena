using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Deck deck;
    [SerializeField]
    private GameObject cardPrefab;
    [SerializeField]
    private GameOverseer gameOverseer;

    private int HP = 10;
    private int Charge = 4;
    private int protection = 0;
    private HeroEnum hero;

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

    // Change Variables
    public void DealDamage(int damage)
    {
        if (damage - protection > 0)
            HP -= (damage- protection);
    }

    public void DealDamageUnblock(int damage)
    {
        HP -= damage;
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
    }

    public void Heal(int heal)
    {
        HP += heal;
        if (HP > 10) { HP = 10; }
    }

    // Getters
    public Deck GetDeck()
    {
        return deck;
    }


    // Summon
    /*public void SummonCard(HandCard handCard)
    {
        // Summon card
        if (gameOverseer.GetState() == GameState.Choice && !handCard.GetCard().isReaction)
        {
            gameOverseer.sendCard();

            // Invoke physical card
            Vector3 v = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, zValue));
            GameObject g = Instantiate(cardPrefab, new Vector3(v.x, v.y, v.z), Quaternion.LookRotation(Vector3.back, Vector3.down));

            // Setup variables
            g.GetComponent<CardInBoard>().thisCard = thisCard;
            g.GetComponent<CardInBoard>().owner = HeroDecks.HD.myManager;
            deckManager.holdingCard = false;

            // Setup text
            g.GetComponent<CardInBoard>().cardSprite = cardSprite;
            g.transform.GetChild(6).GetComponent<Renderer>().material.mainTexture = ImageStash.IS.textureFromSprite(cardSprite);
            g.transform.GetChild(0).GetComponent<TextMesh>().text = HeroDecks.HD.myManager.cardList[thisCard].name;
            g.transform.GetChild(1).GetComponent<TextMesh>().text = HeroDecks.HD.myManager.cardList[thisCard].typeString(HeroDecks.HD.myManager.cardList[thisCard].type);
            g.transform.GetChild(2).GetComponent<TextMesh>().text = HeroDecks.HD.myManager.cardList[thisCard].text.Replace("\\n", "\n");
            g.transform.GetChild(3).GetComponent<TextMesh>().text = HeroDecks.HD.value(HeroDecks.HD.myManager.cardList[thisCard], 1);
            g.transform.GetChild(4).GetComponent<TextMesh>().text = HeroDecks.HD.value(HeroDecks.HD.myManager.cardList[thisCard], 2);
            g.transform.GetChild(5).GetComponent<TextMesh>().text = HeroDecks.HD.myManager.cardList[thisCard].heroString(HeroDecks.HD.myManager.cardList[thisCard].hero);

            // Activate slot
            if (GameOverseer.GO.predicted == false) { g.GetComponent<CardInBoard>().Activate(SlotsOnBoard.PlayerCard, false); }
            else { g.GetComponent<CardInBoard>().Activate(SlotsOnBoard.PlayerCard, true); }

            // Preparing to turn this on later
            g.GetComponent<CardInBoard>().thisCardInHand = gameObject;
            if (HeroDecks.HD.myManager.cardList[GameOverseer.GO.myCardPlayed].type == CardTypes.Ultimate)
            {
                g.GetComponent<CardInBoard>().thisUltimateCard = ultiCard;
            }
            gameObject.SetActive(false);
        }
        // Reveal reaction
        else if (gameOverseer.GetState() == GameState.Revelation && handCard.GetCard().isReaction)
        {
            handCard.SetIsReaction(true);
        }
    }*/
}
