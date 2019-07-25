using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

// Game Overseer será o meu Singleton

public class GameOverseer : MonoBehaviour
{
    public static GameOverseer GO;

    // Hero selection
    public int myHero = 200;
    public int enemyHero = 200;
    public int myheroHover = 200;
    public int enemyheroHover = 200;

    // State stuff
    public GameState state;
    public bool changingStates = false;
    public bool receivedAState = false;
    public bool myConfirm = false;
    public bool enemyConfirm = false;
    private float stateClock = 0f;

    // Network card position
    public bool amIHoveringMyself = false;
    public int hoveringCard = -1;
    public Vector3 hoveringCardPos = Vector3.zero;
    public Vector3 hoveringCardLocalPos = Vector3.zero;
    public int sentCard = 0;

    public bool isEnemyHoveringHimself = false;
    public int enemyHoveringCard = -1;
    public Vector3 enemyHoveringCardPos = Vector3.zero;
    public Vector3 enemyHoveringCardLocalPos = Vector3.zero;
    public bool enemySentCard = false;

    // Predict stuff
    public bool predicted = false;
    public bool enemyPredicted = false;

    // Interface stuff
    public int interfaceSignalSent = 200;

    // Ulti stuff
    public int ultiBuy = 200;
    public int enemyUltiBuy = 200;

    // Deck shuffle
    public int shuffled = 0;
    public int[] sentDeckList = new int[10];
    public bool enemyShuffled = false;
    public int[] receivedDeckList = new int[10];

    // Playing cards
    [HideInInspector]
    public int myCardPlayed = 200;
    [HideInInspector]
    public int enemyCardPlayed = 200;

    // Singleton management (THERE CAN BE ONLY ONE!!!)
    private void Awake()
    {
        GameOverseer.GO = this;
    }

    private void OnEnable()
    {
        if (GO == null) {
            GO = this;
        } else {
            if (GO != this) {
                Destroy(GO);
                GO = this;
            }
        }
        enemySentCard = false;
    }


    // Start is called before the first frame update
    void Start()
    {
        state = GameState.Purchase;
        enemySentCard = false;
        DontDestroyOnLoad(this.gameObject);

        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Test"), Vector3.zero, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {

        if (SceneManager.GetActiveScene().buildIndex == 3) { 
            stateStuff();
        } else if (SceneManager.GetActiveScene().buildIndex == 2) {
            heroSelectionStuff();
        }

        if (!PhotonNetwork.IsConnectedAndReady || !PhotonNetwork.IsConnected)
        {
            Debug.Log("DISCONNECTED!!!");
            PhotonNetwork.Disconnect();
            PhotonNetwork.LoadLevel(0);
        }
    }


    // Hero Selection Stuff
    void heroSelectionStuff()
    {
        if ((myConfirm && enemyConfirm) && GO.myHero != 200 && GO.enemyHero != 200)
        {
            myConfirm = false;
            enemyConfirm = false;
            SceneManager.LoadScene(3);
        }
    }

    // State Stuff
    void stateStuff()
    {
        // State stuff
        if ((myConfirm && enemyConfirm) || receivedAState)
        {
            switch (state)
            {
                case GameState.Purchase:
                    state = GameState.Choice;
                    HeroSideEffects.HSE.executeSideEffect(0, HeroDecks.HD.myManager, 200);
                    HeroSideEffects.HSE.executeSideEffect(0, HeroDecks.HD.enemyManager, 200);
                    Debug.Log("Choice state");
                    break;

                case GameState.Choice:
                    state = GameState.Revelation;
                    Debug.Log("Revelation state");
                    GameObject.Find("Main UI").GetComponent<MainUIManager>().enemyRevealedCard = HeroDecks.HD.enemyManager.cardList[enemyCardPlayed].name;

                    HeroSideEffects.HSE.executeSideEffect(1, HeroDecks.HD.myManager, GO.myCardPlayed);
                    HeroSideEffects.HSE.executeSideEffect(1, HeroDecks.HD.enemyManager, GO.enemyCardPlayed);

                    if (HeroDecks.HD.myManager.cardList[GO.myCardPlayed] is Interfacer) {
                        Interfacer cc = (Interfacer)HeroDecks.HD.myManager.cardList[GO.myCardPlayed];
                        cc.interfacing();
                    }

                    break;

                case GameState.Revelation:
                    state = GameState.Effects;
                    Debug.Log("Effects state");

                    HeroSideEffects.HSE.executeSideEffect(2, HeroDecks.HD.myManager, GO.myCardPlayed);
                    HeroSideEffects.HSE.executeSideEffect(2, HeroDecks.HD.enemyManager, GO.enemyCardPlayed);
                    everyTurn();
                    activateCards();
                    HeroSideEffects.HSE.executeSideEffect(3, HeroDecks.HD.myManager, GO.myCardPlayed);
                    HeroSideEffects.HSE.executeSideEffect(3, HeroDecks.HD.enemyManager, GO.enemyCardPlayed);

                    // Reset stuff
                    HeroDecks.HD.myManager.protection = 0;
                    HeroDecks.HD.enemyManager.protection = 0;
                    HeroDecks.HD.myManager.cardList[GO.myCardPlayed].isNullified = false;
                    HeroDecks.HD.enemyManager.cardList[GO.enemyCardPlayed].isNullified = false;
                    myCardPlayed = 200;
                    enemyCardPlayed = 200;
                    break;

                case GameState.Effects:
                    state = GameState.Purchase;
                    Debug.Log("Purchase state");
                    GameObject.Find("Main UI").GetComponent<MainUIManager>().enemyRevealedCard = "";

                    // Card Reset stuff -> CardInBoard
                    GO.sentCard = 0;
                    GO.enemySentCard = false;
                    GO.interfaceSignalSent = 0;
                    break;

            }

            if (myConfirm && enemyConfirm) { changingStates = true; }
            receivedAState = false;
            myConfirm = false;

        }

    }


    // Every turn stuff
    public void everyTurn()
    {
        // Reduce Unplayability
        reduceUnplayability(HeroDecks.HD.myManager.cardList);
        reduceUnplayability(HeroDecks.HD.enemyManager.cardList);

        // Reset Card Limit (zB Attack, Charge)
        resetLimit(HeroDecks.HD.myManager.cardList, myCardPlayed);
        resetLimit(HeroDecks.HD.enemyManager.cardList, enemyCardPlayed);
    }

    public void reduceUnplayability(Card[] cardList)
    {
        Debug.Log("Reduce Unplayability");
        for (int i = 0; i < cardList.Length; i++)
        {
            if (cardList[i] != null)
            {
                if (cardList[i].turnsTillPlayable > 0)
                {
                    cardList[i].turnsTillPlayable--;
                    Debug.Log(cardList[i].name + " Usável");
                }
            }
        }
    }

    public void resetLimit(Card[] cardList, int cardPlayed)
    {
        for (int i = 0; i < cardList.Length; i++)
        {
            if (cardList[i] as Limit != null)
            {
                if (cardList[i].id != cardPlayed)
                {
                    Limit cc = cardList[i] as Limit;
                    cc.limit = 0;
                    cardList[i] = cc as Card;
                    //Debug.Log("Card Limit reset: " + cardList[i].name);
                }
            }
        }
    }


    // Effects State
    public void activateCards()
    {
        for (int e = Mathf.Min(HeroDecks.HD.myManager.cardList[GO.myCardPlayed].minmax / 100, HeroDecks.HD.enemyManager.cardList[GO.enemyCardPlayed].minmax / 100); 
            e <= Mathf.Max(HeroDecks.HD.myManager.cardList[GO.myCardPlayed].minmax % 100, HeroDecks.HD.enemyManager.cardList[GO.enemyCardPlayed].minmax % 100);
            e++) {
            if (!HeroDecks.HD.myManager.cardList[GO.myCardPlayed].isNullified)
                HeroDecks.HD.myManager.cardList[GO.myCardPlayed].effect(HeroDecks.HD.myManager, HeroDecks.HD.enemyManager, e);
            if (!HeroDecks.HD.enemyManager.cardList[GO.enemyCardPlayed].isNullified)
                HeroDecks.HD.enemyManager.cardList[GO.enemyCardPlayed].effect(HeroDecks.HD.enemyManager, HeroDecks.HD.myManager, e);
        }

        
        
    }
}
