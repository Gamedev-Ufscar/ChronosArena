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
    public int hoveringCard = -1;
    public Vector3 hoveringCardPos = Vector3.zero;
    public Vector3 hoveringCardLocalPos = Vector3.zero;
    public bool sentCard = false;
    public int enemyHoveringCard = -1;
    public Vector3 enemyHoveringCardPos = Vector3.zero;
    public Vector3 enemyHoveringCardLocalPos = Vector3.zero;
    public bool enemySentCard = false;

    // Deck transfer
    public int[] cardsToBeSent = new int[15];
    public int cardsTBSCount = 0;
    public int[] cardsReceived = new int[15];
    public int cardsReceivedCount = 0;

    // Playing cards
    public Card myCardPlayed;
    public Card enemyCardPlayed;

    // Singleton management (THERE CAN BE ONLY ONE!!!)
    private void OnEnable()
    {
        if (GameOverseer.GO == null)
        {
            GameOverseer.GO = this;
        } else
        {
            if (GameOverseer.GO != this)
            {
                Destroy(GameOverseer.GO);
                GameOverseer.GO = this;
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
                    GameOverseer.GO.myCardPlayed.priority = cardPriority(GameOverseer.GO.myCardPlayed);
                    GameOverseer.GO.enemyCardPlayed.priority = cardPriority(GameOverseer.GO.enemyCardPlayed);
                    if (GameOverseer.GO.myCardPlayed.priority < GameOverseer.GO.enemyCardPlayed.priority)
                    {
                        GameOverseer.GO.enemyCardPlayed.effect(HeroDecks.HD.enemyManager, HeroDecks.HD.myManager);
                        if (GameOverseer.GO.myCardPlayed.isPlayable) 
                            GameOverseer.GO.myCardPlayed.effect(HeroDecks.HD.myManager, HeroDecks.HD.enemyManager);
                    } else
                    {
                        GameOverseer.GO.myCardPlayed.effect(HeroDecks.HD.myManager, HeroDecks.HD.enemyManager);
                        if (GameOverseer.GO.enemyCardPlayed.isPlayable)
                            GameOverseer.GO.enemyCardPlayed.effect(HeroDecks.HD.enemyManager, HeroDecks.HD.myManager);
                    }
                    break;

            }

            myConfirm = false;
            changingStates = true;

        }

    }

    int cardPriority(Card card)
    {
        if (card.type == CardTypes.Nullification)
        {
            return 3;
        } else if (card.type == CardTypes.Defense)
        {
            return 2;
        } else
        {
            return 0;
        }
    }
}