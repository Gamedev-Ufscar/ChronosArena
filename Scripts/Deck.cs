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

    private DeckCard[] cardsInDeck;
    //private int[] cardIndexes = new int[10] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9};
    private bool holdingCard = false;

    // Current Card
    private int? sendingID = null;
    private Vector2 sendingPosition;
    private float time = 0f;

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

    // Constructing, Creating and Summoning
    public void CreateDeck(HeroEnum hero, int cardCount, int ultiCount, int passiveCount)
    {
        cardsInDeck = new DeckCard[Constants.maxCardAmount];

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
        Debug.Log("Start positions");
    }

    public GameObject CreateCard(HeroEnum hero, int i)
    {
        // Instantiate
        GameObject card = Instantiate(cardPrefab, new Vector3(507f, -286.2f), Quaternion.identity);
        card.transform.parent = transform;
        AddToDeck(card.GetComponent<DeckCard>(), i);

        // Add Plato Card
        Card platoCard = CardMaker.CM.MakeCard(hero, i);
        card.GetComponent<DeckCard>().SetupCard(platoCard);


        return card;
    }

    public void AddToDeck(DeckCard card, int id)
    {
        card.SetIndex(id);
        card.SetID(id);
        card.SetDeck(this);
        cardsInDeck[id] = card;
    }

    public void UnleashedCard(HandCard handCard)
    {
        player.UnleashCard(handCard);
    }

    // Manipulate position
    public void UpdateCardPositions(int[] cardPositions)
    {
        Debug.Log("UpdateCardPositions");
        for (int i = 0; i < Constants.maxHandSize; i++)
        {
            if (cardsInDeck[i] != null && !cardsInDeck[i].GetBeingHeld())
            {
                cardsInDeck[i].ChangePosition(cardLocations[cardPositions[i]]);
                cardsInDeck[i].SetIndex(cardPositions[i]);
            }
        }
    }

    public void UpdateCardPositions()
    {
        for (int i = 0; i < Constants.maxHandSize; i++)
        {
            if (cardsInDeck[i] != null && cardLocations[i] != null && !cardsInDeck[i].GetBeingHeld())
            {
                cardsInDeck[i].ChangePosition(cardLocations[cardsInDeck[i].GetIndex()]);
                //Debug.Log("Updating: " + cardsInDeck[i].GetCard().GetName());
            }
        }
    }

    public void UpdateCardPosition(DeckCard card)
    {
        if (card.isActiveAndEnabled && !card.GetIsReaction())
            card.ChangePosition(cardLocations[card.GetIndex()]);
    }

    public void Shuffle()
    {
        int helper, rando = 0;
        int[] cardIndexes;

        for (int i = 0; i < Constants.maxHandSize; i++) {
            rando = Random.Range(0, Constants.maxHandSize);
            if (cardsInDeck[i] != null && cardsInDeck[i].isActiveAndEnabled && !cardsInDeck[i].GetIsReaction() && !cardsInDeck[i].GetIsReaction()) {
                if (cardsInDeck[rando] != null && cardsInDeck[rando].isActiveAndEnabled && !cardsInDeck[rando].GetIsReaction() && !cardsInDeck[i].GetIsReaction()) {
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

        cardIndexes = new int[Constants.maxCardAmount];

        for (int i = 0; i < Constants.maxCardAmount; i++)
        {
            if (cardsInDeck[i] != null)
                cardIndexes[i] = cardsInDeck[i].GetIndex();
        }
        player.SendShuffle(cardIndexes);

    }

    public void RecedeDeck(int cardIndex)
    {
        for (int i = 0; i < cardsInDeck.Length; i++)
        {
            if (cardsInDeck[i] != null && cardsInDeck[i].isActiveAndEnabled && !cardsInDeck[i].GetIsReaction()) {
                if (cardsInDeck[i].GetIndex() > cardIndex) {
                    cardsInDeck[i].RecedeIndex();
                    AudioManager.AM.CardSound();
                }
            }
        }

        UpdateCardPositions();
    }

    // Getters
    public bool GetHoldingCard()
    {
        return holdingCard;
    }

    public DeckCard GetDeckCard(int id)
    {
        if (cardsInDeck != null && cardsInDeck[id] != null)
        {
            return cardsInDeck[id];
        } else
        {
            return null;
        }
    }

    public DeckCard[] GetHandCards()
    {
        return cardsInDeck;
    }

    public Vector2 GetCardLocation(int id)
    {
        return cardLocations[id];
    }

    public Vector2 GetReactionLocation(int index)
    {
        return reactionLocations[index];
    }

    // Setters
    public void SetHoldingCard(bool holdingCard)
    {
        this.holdingCard = holdingCard;
    }

    // Sender
    public void SendCardPosition(int? id, Vector2 position)
    {
        player.SetCardPosition(id, position);
    }

    public void SendCardPosition(HandCard card)
    {
        player.SetCardPosition(card.GetID(), card.GetTargetPosition());
    }


    // Receiver
    public void ReceiveCardPosition(int? hoverCard, Vector2 hoverPos)
    {
        // All other cards return
        foreach (DeckCard dc in cardsInDeck)
        {
            if (dc is EnemyCard)
            {
                EnemyCard ec = (EnemyCard)dc;
                ec.EnemyRelease();
            }
        }

        // Sent card updates position
        if (hoverCard != null)
        {
            EnemyCard ec = (EnemyCard)GetDeckCard((int)hoverCard);
            if (ec != null)
            {
                ec.SetBeingHeld(true);
                ec.EnemyHold(new Vector2(-hoverPos.x, -hoverPos.y + 2f));
            }
        }
    }
}
