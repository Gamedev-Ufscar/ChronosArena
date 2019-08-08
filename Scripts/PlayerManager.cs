using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public int HP = 10;
    public int Charge = 0;
    public int protection = 0;
    public int initialCardCount = 0;
    public int ultiCount = 1;
    public Card[] cardList = new Card[15];
    public int[] sideList = new int[12];
    public List<CardTypes> attackDisableList = new List<CardTypes>();
    public List<CardTypes> chargeDisableList = new List<CardTypes>();
    public GameObject myHand;
    public GameObject prefabCard;
    public GameObject prefabUlti;
    public HeroEnum hero = HeroEnum.None;

    private bool startedGame = false;

    // Start is called before the first frame update
    void Start()
    {
        startedGame = false;
        CreateTrain();
    }


    // Update is called once per frame
    void Update()
    {

        // Game Initialization
        if (gameObject.name == "Player Manager" && startedGame == false && hero != HeroEnum.None) {
            startedGame = true;
            GameOverseer.GO.myConfirm = false;
            HeroDecks.HD.myManager = this;
            myHand = GameObject.Find("Player Cards");
            InitializeDeck();
            CreatePlayer();
        }

        if (gameObject.name == "Enemy Manager" && startedGame == false && hero != HeroEnum.None) {
            startedGame = true;
            GameOverseer.GO.enemyConfirm = false;
            HeroDecks.HD.enemyManager = this;
            myHand = GameObject.Find("Enemy Cards");
            InitializeDeck();
            CreateEnemy();
        }
    }

    // GENERAL INITIALIZATION
    public void DiscardCard(int id) // Descarta thisCard
    {
        // Se for Ultimate, restaura a carta de compra
        if (this == HeroDecks.HD.myManager) {
            if (HeroDecks.HD.myManager.cardList[HeroDecks.HD.myManager.myHand.GetComponent<DeckManager>().deckList[id].GetComponent<CardInHand>().thisCard].type == CardTypes.Ultimate)
            {
                myHand.GetComponent<DeckManager>().deckList[id].GetComponent<CardInHand>().ultiCard.SetActive(true);
            }
        }
        else if (this == HeroDecks.HD.enemyManager)
        {
            if (HeroDecks.HD.enemyManager.cardList[HeroDecks.HD.enemyManager.myHand.GetComponent<DeckManager>().deckList[id].GetComponent<EnemyCardInHand>().thisCard].type == CardTypes.Ultimate)
            {
                myHand.GetComponent<DeckManager>().deckList[id].GetComponent<EnemyCardInHand>().ultiCard.SetActive(true);
            }
        }

        // Desativa carta
        myHand.GetComponent<DeckManager>().deckList[id].SetActive(false);

        // Recede Deck
        if (this == HeroDecks.HD.myManager) {
            myHand.GetComponent<DeckManager>().recedeDeck(myHand.GetComponent<DeckManager>().deckList[id].GetComponent<CardInHand>().cardIndex);
        } else if (this == HeroDecks.HD.enemyManager) {
            myHand.GetComponent<DeckManager>().recedeDeck(myHand.GetComponent<DeckManager>().deckList[id].GetComponent<EnemyCardInHand>().cardIndex);
        }

        // Diminui a quantidade de cartas ativas
        myHand.GetComponent<DeckManager>().activeCardCount--;
    }

    public void RestoreCard(int id) // Restaura thisCard
    {
        // Ativa carta, coloca-a no final da fila
        myHand.GetComponent<DeckManager>().deckList[id].SetActive(true);
        if (this == HeroDecks.HD.myManager)
            myHand.GetComponent<DeckManager>().deckList[id].GetComponent<CardInHand>().cardIndex = myHand.GetComponent<DeckManager>().activeCardCount;
        else if (this == HeroDecks.HD.enemyManager)
            myHand.GetComponent<DeckManager>().deckList[id].GetComponent<EnemyCardInHand>().cardIndex = myHand.GetComponent<DeckManager>().activeCardCount;

        // Aumenta a quantidade de cartas ativas
        myHand.GetComponent<DeckManager>().activeCardCount++;
    }

    void InitializeDeck()
    {
        //GameOverseer.GO.cardsTBSCount = 0;
        // Initialize Deck
        for (int i = 0; i < initialCardCount+ultiCount; i++)
        {
            cardList[i] = HeroDecks.HD.heroCard(hero, i);
        }
    }

    private GameObject CreateUlti(int index)
    {
        GameObject card = Instantiate(prefabUlti, new Vector3(507f, -286.2f), Quaternion.identity);
        card.transform.parent = myHand.transform;
        card.GetComponent<UltimateCard>().deckManager = card.transform.parent.GetComponent<DeckManager>();
        card.GetComponent<UltimateCard>().staticCardIndex = index + 100;
        card.GetComponent<UltimateCard>().cardIndex = card.GetComponent<UltimateCard>().staticCardIndex;
        card.GetComponent<UltimateCard>().thisCard = index + initialCardCount;
        return card;
    }


    // PLAYER INITIALIZATION
    public GameObject CreateCard(int cardIndex, int thisCard)
    {
        GameObject card = Instantiate(prefabCard, new Vector3(507f, -286.2f), Quaternion.identity);
        card.transform.parent = myHand.transform;
        card.GetComponent<CardInHand>().deckManager = card.transform.parent.GetComponent<DeckManager>();
        card.GetComponent<CardInHand>().cardIndex = cardIndex; // THIS NEEDS TO BE RANDOMIZED LATER!!!
        card.GetComponent<CardInHand>().thisCard = thisCard;
        return card;
    }

    private void CreatePlayer()
    {
        // Creating Cards
        for (int i = 0; i < initialCardCount; i++) {
            myHand.GetComponent<DeckManager>().deckList[i] = CreateCard(i, i);
        }
        myHand.GetComponent<DeckManager>().cardTotalCount = initialCardCount;
        myHand.GetComponent<DeckManager>().activeCardCount = initialCardCount;
        for (int i = 0; i < ultiCount; i++) {
            myHand.GetComponent<DeckManager>().ultiList[i] = CreateUlti(i);
        }
    }


    // ENEMY INITIALIZATION
    public GameObject EnemyCreateCard(int cardIndex, int thisCard)
    {
        GameObject card = Instantiate(prefabCard, new Vector3(-557f, 288.1f), Quaternion.identity);
        card.transform.parent = myHand.transform;
        card.GetComponent<EnemyCardInHand>().deckManager = card.transform.parent.GetComponent<DeckManager>();
        card.GetComponent<EnemyCardInHand>().cardIndex = cardIndex; // THIS NEEDS TO BE RANDOMIZED LATER!!!
        //Debug.Log("EnemyCreateCard: " + cardList[index].id);
        card.GetComponent<EnemyCardInHand>().thisCard = thisCard;
        return card;
    }

    void CreateEnemy()
    {
        // Creating Cards

        for (int i = 0; i < initialCardCount; i++) {
            myHand.GetComponent<DeckManager>().deckList[i] = EnemyCreateCard(i, i);
        }
        myHand.GetComponent<DeckManager>().cardTotalCount = initialCardCount;
        myHand.GetComponent<DeckManager>().activeCardCount = initialCardCount;
        for (int i = 0; i < ultiCount; i++) {
            CreateUlti(i);
        }
    }


    // TRAIN CREATION
    private void CreateTrain()
    {
        Debug.Log("Creating train");
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Test"), Vector3.zero, Quaternion.identity);
    }

}
