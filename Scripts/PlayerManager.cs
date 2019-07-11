using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int HP = 10;
    public int Charge = 0;
    public int protection = 0;
    public int initialCardCount = 0;
    public Card[] cardList = new Card[15];
    public List<CardTypes> attackDisableList = new List<CardTypes>();
    public List<CardTypes> chargeDisableList = new List<CardTypes>();
    public GameObject myHand;
    public GameObject prefabCard;
    public bool enemyCreated = false;

    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.name == "Player Manager")
        {
            InitializeDeck();
            CreatePlayer();
            CreateTrain();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.name == "Enemy Manager" && GameOverseer.GO.cardsReceivedCount > 0 && enemyCreated == false)
        {
            Debug.Log("Activating enemy");
            InitializeEnemyDeck();
            CreateEnemy();
            enemyCreated = true;
        }
    }

    // PLAYER INITIALIZATION
    void InitializeDeck()
    {
        // Initialize Deck
        for (int i = 0; i < initialCardCount; i++)
        {
            cardList[i] = HeroDecks.HD.heroCard(-1, i);
            //Debug.Log(cardList[i].name + " = " + i);
            GameOverseer.GO.cardsToBeSent[GameOverseer.GO.cardsTBSCount] = cardList[i].id;
            GameOverseer.GO.cardsTBSCount++;
        }
    }

    private void CreateCard(int index)
    {
        GameObject card = Instantiate(prefabCard, new Vector3(507f, -286.2f), Quaternion.identity);
        card.transform.parent = myHand.transform;
        card.GetComponent<CardInHand>().deckManager = card.transform.parent.GetComponent<DeckManager>();
        card.GetComponent<CardInHand>().cardIndex = index; // THIS NEEDS TO BE RANDOMIZED LATER!!!
        card.GetComponent<CardInHand>().thisCard = cardList[index].id;
    }

    private void CreatePlayer()
    {
        Debug.Log("Creating player");

        // Creating Cards
        for (int i = 0; i < initialCardCount; i++)
        {
            CreateCard(i);
        }
    }


    // ENEMY INITIALIZATION
    void InitializeEnemyDeck()
    {
        // Initialize Deck
        for (int i = 0; i < GameOverseer.GO.cardsReceivedCount; i++)
        {
            cardList[i] = HeroDecks.HD.heroCard(-1, GameOverseer.GO.cardsReceived[i]);
        }
    }

    private void EnemyCreateCard(int index)
    {
        GameObject card = Instantiate(prefabCard, new Vector3(-557f, 288.1f), Quaternion.identity);
        card.transform.parent = myHand.transform;
        card.GetComponent<EnemyCardInHand>().deckManager = card.transform.parent.GetComponent<DeckManager>();
        card.GetComponent<EnemyCardInHand>().cardIndex = index; // THIS NEEDS TO BE RANDOMIZED LATER!!!
        //Debug.Log("EnemyCreateCard: " + cardList[index].id);
        card.GetComponent<EnemyCardInHand>().thisCard = cardList[index].id;
    }

    void CreateEnemy()
    {
        Debug.Log("Creating enemy");

        // Creating Cards

        for (int i = 0; i < GameOverseer.GO.cardsReceivedCount; i++)
        {
            EnemyCreateCard(i);
        }

    }


    // TRAIN CREATION
    private void CreateTrain()
    {
        Debug.Log("Creating train");
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Test"), Vector3.zero, Quaternion.identity);
    }
}
