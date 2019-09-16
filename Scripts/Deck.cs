using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    [SerializeField]
    private Player player;
    [SerializeField]
    private GameObject cardPrefab;
    [SerializeField]
    private Vector2[] cardLocations = new Vector2[Constants.maxHandSize];
    [SerializeField]
    private Vector2[] reactionLocations = new Vector2[4];

    private HandCard[] cardsInDeck = new HandCard[Constants.maxCardAmount];
    //private int[] cardIndexes = new int[10] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9};
    private bool holdingCard = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Deck(Player player)
    {
        this.player = player;
    }

    public void UpdateCardPositions(int[] cardPositions)
    {

        for (int i = 0; i < cardsInDeck.Length; i++)
        {
            if (cardsInDeck[i] != null)
                cardsInDeck[i].ChangePosition(cardLocations[cardPositions[i]]);
        }
    }

    public void UpdateCardPositions()
    {
        for (int i = 0; i < cardsInDeck.Length; i++)
        {
            if (cardsInDeck[i] != null && cardLocations[i] != null)
                cardsInDeck[i].ChangePosition(cardLocations[cardsInDeck[i].GetIndex()]);
        }
    }

    public void Shuffle()
    {
        int helper, rando = 0;
        int[] cardIndexes = new int[14];

        for (int i = 0; i < cardsInDeck.Length; i++) {
            rando = Random.Range(0, cardsInDeck.Length);
            if (cardsInDeck[i] != null && cardsInDeck[i].gameObject.activeInHierarchy) {
                if (cardsInDeck[rando] != null && cardsInDeck[rando].gameObject.activeInHierarchy) {
                    // Swap randomly
                    helper = cardsInDeck[i].GetIndex();
                    cardsInDeck[i].SetIndex(cardsInDeck[rando].GetIndex());
                    cardsInDeck[rando].SetIndex(helper);

                    // Sound
                    AudioManager.AM.CardSound();
                }
            }
        }

        UpdateCardPositions();

        for (int i = 0; i < cardsInDeck.Length; i++)
        {
            cardIndexes[i] = cardsInDeck[i].GetIndex();
        }
        player.SendShuffle(cardIndexes);

        for (int i = 0; i < cardsInDeck.Length; i++) {
            if (cardsInDeck[i] != null) { }
                // CHECK LATER
                //gameOverseer.sentDeckList[i] = cardsInDeck[i].GetComponent<UICard>().getCard().id;
        }
    }

    public void RecedeDeck(int cardIndex)
    {
        for (int i = 0; i < cardsInDeck.Length; i++)
        {
            if (cardsInDeck[i] != null && cardsInDeck[i].gameObject.activeInHierarchy) {
                if (cardsInDeck[i].GetIndex() > cardIndex) {
                    cardsInDeck[i].RecedeIndex();
                    AudioManager.AM.CardSound();
                }
            }
        }
    }

    // Getters
    public bool GetHoldingCard()
    {
        return holdingCard;
    }

    public HandCard GetHandCard(int id)
    {
        return cardsInDeck[id];
    }

    public HandCard[] GetHandCards()
    {
        return cardsInDeck;
    }

    // Setters
    public void SetHoldingCard(bool holdingCard)
    {
        this.holdingCard = holdingCard;
    }

    // Summoning
    public void CreateDeck(HeroEnum hero, int cardCount, int ultiCount, int passiveCount)
    {
        for (int i = 0; i < cardCount; i++)
        {
            CreateCard(hero, i);
        }
        for (int i = cardCount; i < cardCount + ultiCount + passiveCount; i++)
        {
            GameObject card = CreateCard(hero, i);
            card.SetActive(false);
        }

        UpdateCardPositions();
    }

    public GameObject CreateCard(HeroEnum hero, int i)
    {
        // Instantiate
        GameObject card = Instantiate(cardPrefab, new Vector3(507f, -286.2f), Quaternion.identity);
        card.transform.parent = transform;
        card.GetComponent<HandCard>().SetIndex(i);
        cardsInDeck[i] = card.GetComponent<HandCard>();

        // Add Plato Card
        Card platoCard = CardMaker.CM.MakeCard(hero, i);
        card.GetComponent<HandCard>().SetCard(platoCard);

        return card;
    }

    public void UnleashedCard(HandCard handCard)
    {
        player.SummonCard(handCard);
    }
}
