using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public int HP = 10;
    public int Charge = 2;
    public int protection = 0;
    public int initialCardCount = 0;
    public Card[] cardList = new Card[15];
    public int[] sideList = new int[12];
    public List<CardTypes> attackDisableList = new List<CardTypes>();
    public List<CardTypes> chargeDisableList = new List<CardTypes>();
    public GameObject myHand;
    public GameObject prefabCard;
    public GameObject prefabUlti;
    public bool enemyCreated = false;
    public int hero = -1;

    private bool startedGame = false;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        CreateTrain();
    }


    // Update is called once per frame
    void Update()
    {
        if (gameObject.name == "Player Manager" && SceneManager.GetActiveScene().buildIndex == 2 && !startedGame)
        {
            InitializeDeck();
            CreatePlayer();
            startedGame = true;
        }

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
            GameOverseer.GO.cardsToBeSent[GameOverseer.GO.cardsTBSCount] = i;
            GameOverseer.GO.cardsTBSCount++;
        }

        // Ulti
        cardList[initialCardCount] = HeroDecks.HD.heroCard(-1, initialCardCount);
        GameOverseer.GO.ultiToBeSent = initialCardCount;
    }

    public GameObject CreateCard(int index)
    {
        GameObject card = Instantiate(prefabCard, new Vector3(507f, -286.2f), Quaternion.identity);
        card.transform.parent = myHand.transform;
        card.GetComponent<CardInHand>().deckManager = card.transform.parent.GetComponent<DeckManager>();
        card.GetComponent<CardInHand>().cardIndex = index; // THIS NEEDS TO BE RANDOMIZED LATER!!!
        card.GetComponent<CardInHand>().thisCard = index;
        return card;
    }

    private void CreateUlti(int index)
    {
        GameObject card = Instantiate(prefabUlti, new Vector3(507f, -286.2f), Quaternion.identity);
        card.transform.parent = myHand.transform;
        card.GetComponent<UltimateCard>().deckManager = card.transform.parent.GetComponent<DeckManager>();
        card.GetComponent<UltimateCard>().cardIndex = 101;
        card.GetComponent<UltimateCard>().thisCard = index;
    }

    private void CreatePlayer()
    {
        Debug.Log("Creating player");

        // Creating Cards
        for (int i = 0; i < initialCardCount; i++)
        {
            CreateCard(i);
        }
        myHand.GetComponent<DeckManager>().cardAmount = initialCardCount;
        CreateUlti(initialCardCount);
    }


    // ENEMY INITIALIZATION
    void InitializeEnemyDeck()
    {
        // Initialize Deck
        for (int i = 0; i < GameOverseer.GO.cardsReceivedCount; i++)
        {
            cardList[i] = HeroDecks.HD.heroCard(-1, GameOverseer.GO.cardsReceived[i]);
        }

        // Ulti
        cardList[GameOverseer.GO.cardsReceivedCount] = HeroDecks.HD.heroCard(-1, GameOverseer.GO.ultiReceived);
        Debug.Log("Enemy Ulti: " + cardList[GameOverseer.GO.cardsReceivedCount].name);
    }

    public GameObject EnemyCreateCard(int index)
    {
        GameObject card = Instantiate(prefabCard, new Vector3(-557f, 288.1f), Quaternion.identity);
        card.transform.parent = myHand.transform;
        card.GetComponent<EnemyCardInHand>().deckManager = card.transform.parent.GetComponent<DeckManager>();
        card.GetComponent<EnemyCardInHand>().cardIndex = index; // THIS NEEDS TO BE RANDOMIZED LATER!!!
        //Debug.Log("EnemyCreateCard: " + cardList[index].id);
        card.GetComponent<EnemyCardInHand>().thisCard = index;
        return card;
    }

    void CreateEnemy()
    {
        Debug.Log("Creating enemy");

        // Creating Cards

        for (int i = 0; i < GameOverseer.GO.cardsReceivedCount; i++) {
            EnemyCreateCard(i);
        }
        myHand.GetComponent<DeckManager>().cardAmount = GameOverseer.GO.cardsReceivedCount;
        CreateUlti(GameOverseer.GO.ultiReceived);
    }


    // TRAIN CREATION
    private void CreateTrain()
    {
        Debug.Log("Creating train");
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Test"), Vector3.zero, Quaternion.identity);
    }

}
