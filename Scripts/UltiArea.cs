using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UltiArea : MonoBehaviour
{
    [SerializeField]
    Player player;
    [SerializeField]
    private Vector2[] ultiLocations = new Vector2[Constants.maxUltiAreaSize];
    [SerializeField]
    private GameObject ultiPrefab;


    private int handCount;
    private UltimateCard[] cardsInArea;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Constructing and Creating
    public void CreateUltiArea(HeroEnum hero, int cardCount, int ultiCount)
    {
        cardsInArea = new UltimateCard[Constants.maxUltiAreaSize];

        handCount = cardCount;
        for (int i = cardCount; i < cardCount + ultiCount; i++)
        {
            CreateUlti(hero, i);
        }

        UpdateUltiPositions();
    }

    public GameObject CreateUlti(HeroEnum hero, int i)
    {
        // Instantiate
        GameObject card = Instantiate(ultiPrefab, new Vector3(507f, -286.2f), Quaternion.identity);
        card.transform.parent = transform;
        AddToArea(card.GetComponent<UltimateCard>(), i);

        // Add Plato Card
        Card platoCard = CardMaker.CM.MakeCard(hero, i);
        card.GetComponent<UltimateCard>().SetupCard(platoCard);

        return card;
    }

    public void AddToArea(UltimateCard card, int id)
    {
        card.SetIndex(id - handCount);
        card.SetID(id);
        card.SetUltiArea(this);
        cardsInArea[id - handCount] = card;
    }

    // Manipulate Position
    public void UpdateUltiPositions()
    {
        Debug.Log("UpdateUltiPositions");
        for (int i = 0; i < Constants.maxUltiAreaSize; i++)
        {
            if (cardsInArea[i] != null && ultiLocations[i] != null)
            {
                cardsInArea[i].ChangePosition(ultiLocations[cardsInArea[i].GetIndex()]);
                Debug.Log("Ultipdating: " + cardsInArea[i].GetCard().GetName());
            }
        }
    }

    public void RecedeUlti(int cardIndex)
    {
        for (int i = 0; i < cardsInArea.Length; i++)
        {
            if (cardsInArea[i] != null && cardsInArea[i].GetIsActive()) {
                if (cardsInArea[i].GetIndex() > cardIndex) {
                    cardsInArea[i].RecedeIndex();
                }
            }
        }
    }

    public int PlaceUltimate(int ultimateID)
    {
        // Find a position for the ultimate
        int myCardIndex = 200;

        for (int i = 0; i < cardsInArea.Length; i++)
        {
            if (cardsInArea[i] != null) { 
                // If this is my card or is offline...
                if (!cardsInArea[i].GetIsActive() || cardsInArea[i].GetComponent<UltimateCard>().GetCard().GetID() == ultimateID) {
                    // Choose this place if haven't yet
                    if (myCardIndex == 200) { myCardIndex = 100+i; }
                    //break;


                // If this is supposed to be further right than me...
                } else if (cardsInArea[i].GetIndex() > ultimateID) {
                    // Choose this place if haven't yet, push current to right
                    if (myCardIndex == 200) { myCardIndex = cardsInArea[i].GetIndex(); }
                    cardsInArea[i].PushIndex();
                }
            }
        }
        if (myCardIndex == 200) { myCardIndex = 100 + cardsInArea.Length - 1; }

        return myCardIndex;
    }

    // Getter
    public UltimateCard GetUltiCard(int? i)
    {
        if (i != null)
            return cardsInArea[(int)i];
        else
            return null;
    }

    public Card GetCard(int i)
    {
        if (cardsInArea[i] != null)
            return cardsInArea[i].GetCard();
        else
        {
            return null;
        }
    }

    public bool GetHoldingCard()
    {
        return player.GetHoldingCard();
    }

    // Setter
    public void SetSibling(bool isLast)
    {
        if (isLast) { transform.SetAsLastSibling(); }
        else { transform.SetAsFirstSibling(); }
    }

    // Sender
    public void SendUltiPurchase(int cardID, bool bought)
    {
        player.SendUltiPurchase(cardID, bought);
    }

    
}
