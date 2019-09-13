using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    [SerializeField]
    private Player player;
    [SerializeField]
    private Vector2[] cardLocations = new Vector2[10];
    [SerializeField]
    private Vector2[] reactionLocations = new Vector2[4];
    [SerializeField]
    private GameObject prefabCard;

    private HandCard[] cardsInDeck = new HandCard[10];
    private int[] cardIndexes = new int[10] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9};
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

    public void UpdateCardPositions()
    {
        for (int i = 0; i < cardsInDeck.Length; i++)
        {
            if (cardsInDeck[i] != null)
                cardsInDeck[i].ChangePosition(cardLocations[cardIndexes[i]]);
        }
    }

    public void UpdateCardPositions(int[] newCardPositions)
    {
        cardIndexes = newCardPositions;

        for (int i = 0; i < cardsInDeck.Length; i++)
        {
            if (cardsInDeck[i] != null)
                cardsInDeck[i].ChangePosition(cardLocations[cardIndexes[i]]);
        }
    }

    public void Shuffle()
    {
        int helper, rando = 0;
        for (int i = 0; i < cardsInDeck.Length; i++) {
            rando = Random.Range(0, cardsInDeck.Length);
            if (cardsInDeck[i] != null && cardsInDeck[i].gameObject.activeInHierarchy) {
                if (cardsInDeck[rando] != null && cardsInDeck[rando].gameObject.activeInHierarchy) {
                    helper = cardIndexes[i];
                    cardIndexes[i] = cardIndexes[rando];
                    cardIndexes[rando] = helper;
                    AudioManager.AM.CardSound();
                }
            }
        }

        UpdateCardPositions();
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
                if (cardIndexes[i] > cardIndex) {
                    cardIndexes[i]--;
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

    // Setters
    public void SetHoldingCard(bool holdingCard)
    {
        this.holdingCard = holdingCard;
    }

    // Summoning
    public GameObject CreateCard(Card card)
    {
        GameObject cardObject = Instantiate(prefabCard, new Vector3(507f, -286.2f), Quaternion.identity);
        cardObject.transform.parent = transform;
        UICard cardComponent = cardObject.AddComponent<UICard>();
        cardComponent = new UICard(card, this);

        UpdateCardPositions();

        return cardObject;
    }

    public void AcquireUICard(Card card)
    {
        GameObject cardObject = CreateCard(card);
        cardObject.transform.parent = transform;
        // CHECK LATER
        //GameOverseer.GO.enemyUltiBuy[staticCardIndex - 100] = false;
        //recedeUlti(cardIndex);
        AudioManager.AM.CardSound();
    }

    public void UnleashedCard(HandCard handCard)
    {
        //player.SummonCard(handCard);
    }
}
