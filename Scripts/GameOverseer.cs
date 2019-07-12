using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Game Overseer será o meu Singleton

public class GameOverseer : MonoBehaviour
{
    public static GameOverseer GO;

    // State stuff
    public GameState state;
    public bool changingStates = false;
    public bool myConfirm = false;
    public bool enemyConfirm = false;
    private float stateClock = 0f;

    // Network card position
    public bool amIHoveringMyself = false;
    public int hoveringCard = -1;
    public Vector3 hoveringCardPos = Vector3.zero;
    public Vector3 hoveringCardLocalPos = Vector3.zero;
    public bool sentCard = false;

    public bool isEnemyHoveringHimself = false;
    public int enemyHoveringCard = -1;
    public Vector3 enemyHoveringCardPos = Vector3.zero;
    public Vector3 enemyHoveringCardLocalPos = Vector3.zero;
    public bool enemySentCard = false;

    // Ulti stuff
    public bool ultiBuy = false;
    public bool enemyUltiBuy = false;

    // Deck transfer
    public int[] cardsToBeSent = new int[15];
    public int cardsTBSCount = 0;
    public int ultiToBeSent;
    public int[] cardsReceived = new int[15];
    public int cardsReceivedCount = 0;
    public int ultiReceived;

    // Playing cards
    public int myCardPlayed;
    public int enemyCardPlayed;

    // Singleton management (THERE CAN BE ONLY ONE!!!)
    private void OnEnable()
    {
        if (GO == null)
        {
            GO = this;
        } else
        {
            if (GO != this)
            {
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
    }

    // Update is called once per frame
    void Update()
    {
        stateStuff();

        if (!PhotonNetwork.IsConnectedAndReady || !PhotonNetwork.IsConnected)
        {
            Debug.Log("DISCONNECTED!!!");
            PhotonNetwork.Disconnect();
            PhotonNetwork.LoadLevel(0);
        }
    }

    void stateStuff()
    {
        // State stuff
        if (myConfirm && enemyConfirm)
        {
            switch (state)
            {
                case GameState.Purchase:
                    state = GameState.Choice;
                    Debug.Log("Choice state");
                    break;

                case GameState.Choice:
                    state = GameState.Revelation;
                    Debug.Log("Revelation state");
                    break;

                case GameState.Revelation:
                    state = GameState.Effects;
                    Debug.Log("Effects state");
                    everyTurn();
                    activateCards();
                    break;

                case GameState.Effects:
                    state = GameState.Purchase;
                    Debug.Log("Purchase state");
                    break;

            }

            myConfirm = false;
            changingStates = true;

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
        foreach (Card c in cardList) {
            if (c != null) {
                if (c.turnsTillPlayable > 0)
                    c.turnsTillPlayable--;
            }
        }
    }

    public void resetLimit(Card[] cardList, int cardPlayed)
    {
        foreach (Card c in cardList) {
            if (c != null) {
                if (c.id != cardPlayed) {
                    if (c is Limit) {
                        Limit cc = (Limit)c;
                        cc.limit = 0;
                    }
                }
            }
        }
    }


    // Effects State
    public void activateCards()
    {
        // Set priorities
        HeroDecks.HD.myManager.cardList[GO.myCardPlayed].priority = cardPriority(HeroDecks.HD.myManager.cardList[GO.myCardPlayed]);
        HeroDecks.HD.enemyManager.cardList[GO.enemyCardPlayed].priority = cardPriority(HeroDecks.HD.enemyManager.cardList[GO.enemyCardPlayed]);

        // Activate in order Enemy -> Player
        if (HeroDecks.HD.myManager.cardList[GO.myCardPlayed].priority < HeroDecks.HD.enemyManager.cardList[GO.enemyCardPlayed].priority) {
            HeroDecks.HD.enemyManager.cardList[GO.enemyCardPlayed].effect(HeroDecks.HD.enemyManager, HeroDecks.HD.myManager);
            if (!HeroDecks.HD.myManager.cardList[GO.myCardPlayed].isNullified)
                HeroDecks.HD.myManager.cardList[GO.myCardPlayed].effect(HeroDecks.HD.myManager, HeroDecks.HD.enemyManager);
        }

        // Activate in order Player -> Enemy
        else {
            HeroDecks.HD.myManager.cardList[GO.myCardPlayed].effect(HeroDecks.HD.myManager, HeroDecks.HD.enemyManager);
            if (!HeroDecks.HD.enemyManager.cardList[GO.enemyCardPlayed].isNullified)
                HeroDecks.HD.enemyManager.cardList[GO.enemyCardPlayed].effect(HeroDecks.HD.enemyManager, HeroDecks.HD.myManager);
        }

        HeroDecks.HD.myManager.cardList[GO.myCardPlayed].isNullified = false;
        HeroDecks.HD.enemyManager.cardList[GO.enemyCardPlayed].isNullified = false;
    }

    int cardPriority(Card card)
    {
        if (card.type == CardTypes.Nullification) {
            return 3;
        } else if (card.type == CardTypes.Defense) {
            return 2;
        } else {
            return 0;
        }
    }
}