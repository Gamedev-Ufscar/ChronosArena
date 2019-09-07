using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkTrain : MonoBehaviour
{
    GameOverseer gameOverseer;
    SelectionOverseer selectionOverseer;
    PhotonView PV;
    float time = 0f;

    // Constructor
    public NetworkTrain(GameOverseer gameOverseer)
    {
        this.gameOverseer = gameOverseer;
    }

    public NetworkTrain(SelectionOverseer selectionOverseer)
    {
        this.selectionOverseer = selectionOverseer;
    }

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        DontDestroyOnLoad(this.gameObject);
    }

    private void Awake()
    {
        Application.runInBackground = true;
    }

    // Update is called once per frame
    void Update()
    {

        if (time >= 0.2f)
        {
            // Send shuffle
            if (GameOverseer.GO.shuffled > 0) {
                PV.RPC("RPC_Shuffled", RpcTarget.OthersBuffered, GameOverseer.GO.sentDeckList);
            }


            // Send hovering card
            if (GameOverseer.GO.hoveringCard != 200) {
            PV.RPC("RPC_hoverPos", RpcTarget.OthersBuffered, (byte)GameOverseer.GO.hoveringCard, GameOverseer.GO.amIHoveringMyself,
                                     (Vector2)GameOverseer.GO.hoveringCardPos, (Vector2)GameOverseer.GO.hoveringCardLocalPos);
            } else {
                PV.RPC("RPC_ecoHoverPos", RpcTarget.OthersBuffered);
            }

            // Send Interface
            if (GameOverseer.GO.interfaceSignalSent != 200 && GameOverseer.GO.state == GameState.Revelation) {
                if (HeroDecks.HD.myManager.cardList[GameOverseer.GO.myCardPlayed] is Interfacer)
                    PV.RPC("RPC_sendInterfaceSignal", RpcTarget.OthersBuffered, (byte)GameOverseer.GO.interfaceSignalSent);
            }

            // Send Ulti Purchase
            if (GameOverseer.GO.state == GameState.Purchase && SceneManager.GetActiveScene().buildIndex == 3) {
                PV.RPC("RPC_ultiStuff", RpcTarget.OthersBuffered, GameOverseer.GO.ultiBuy,
                    (byte)HeroDecks.HD.myManager.Charge);
            }

            // Send State
            if (GameOverseer.GO.changingStates == true)
            {
                PV.RPC("RPC_SendState", RpcTarget.OthersBuffered, (byte)GameOverseer.GO.state);
                GameOverseer.GO.changingStates = false;
            }

            // Reset bool signals
            if (GameOverseer.GO.sentCard > 0) {
                GameOverseer.GO.sentCard--;
            }
            if (GameOverseer.GO.shuffled > 0) {
                GameOverseer.GO.shuffled--;
            }

            // Time stuff
            time = 0f;
        } else {
            time += Time.deltaTime;
        }
    }

    
    // Send functions
    public void SendConfirm(bool myConfirm)
    {
        // Send Confirm
        PV.RPC("RPC_SendClick", RpcTarget.OthersBuffered, myConfirm);
    }

    public void SendCard()
    {
        // Summon Card
        PV.RPC("RPC_sentCard", RpcTarget.OthersBuffered);
        Debug.Log("Sent Card");

    }

    public void SendChosenHero(int heroIndex)
    {
        // Send chosen Hero
        PV.RPC("RPC_SendHero", RpcTarget.OthersBuffered, (byte)heroIndex);
    }

    public void SendHoverHero(int heroIndex)
    {
            // Send hovering hero
            PV.RPC("RPC_heroHover", RpcTarget.OthersBuffered, heroIndex);
                PV.RPC("RPC_ecoHeroHover", RpcTarget.OthersBuffered);
    }

    public void StopHoverHero()
    {
        // Stop hovering hero
        PV.RPC("RPC_stopHeroHover", RpcTarget.OthersBuffered);
    }

    // RPC functions
    [PunRPC]
    public void RPC_hoverPos(byte hoverCard, bool amIHoveringMyself, Vector2 hoverPos, Vector2 hoverLocalPos)
    {
        GameOverseer.GO.enemyHoveringCard = (int)hoverCard;
        GameOverseer.GO.isEnemyHoveringHimself = amIHoveringMyself;
        //Debug.Log("Is Enemy HH? " + GameOverseer.GO.isEnemyHoveringHimself + ", " + GameOverseer.GO.enemyHoveringCard);

        if (hoverCard < 100)
        {
            GameOverseer.GO.enemyHoveringCardPos = (Vector3)hoverPos;
            GameOverseer.GO.enemyHoveringCardLocalPos = (Vector3)hoverLocalPos;
        }
    }
    [PunRPC]
    public void RPC_ecoHoverPos()
    {
        GameOverseer.GO.enemyHoveringCard = 200;
    }

    [PunRPC]
    public void RPC_heroHover(int heroHover)
    {
        selectionOverseer.EnemyHoverHero(heroHover);
    }
    [PunRPC]
    public void RPC_stopHeroHover()
    {
        selectionOverseer.EnemyHoverHero(200);
    }

    [PunRPC]
    public void RPC_sentCard() {
        if (!GameOverseer.GO.alreadyReceived)
        {
            Debug.Log("Received enemySentCard");
            GameOverseer.GO.enemySentCard = true;
        }
    }

    [PunRPC]
    public void RPC_SendHero(int myHero)
    {
        selectionOverseer.SetEnemyHero(myHero);
    }

    [PunRPC]
    public void RPC_sendInterfaceSignal(byte signalSent) {
        if (GameOverseer.GO.enemyCardPlayed != 200) {
            Debug.Log("Enemy Card: " + GameOverseer.GO.enemyCardPlayed + ", signal sent: " + signalSent);
            if (HeroDecks.HD.enemyManager.cardList[GameOverseer.GO.enemyCardPlayed] is Interfacer) {
                Interfacer cc = (Interfacer)HeroDecks.HD.enemyManager.cardList[GameOverseer.GO.enemyCardPlayed];
                cc.interfaceSignal = (int)signalSent;
                HeroDecks.HD.enemyManager.cardList[GameOverseer.GO.enemyCardPlayed] = (Card)cc;
            }
        }
    }

    [PunRPC]
    public void RPC_ultiStuff(bool[] ultiBuy, byte charge)
    {
        if (SceneManager.GetActiveScene().buildIndex == 3) {
            for (int i = 0; i < ultiBuy.Length; i++)
            {
                GameOverseer.GO.enemyUltiBuy[i] = ultiBuy[i];
            }
            HeroDecks.HD.enemyManager.Charge = (int)charge;
        }
    }

    [PunRPC]
    public void RPC_SendState(byte myState)
    {
        GameOverseer.GO.myConfirm = false;
        if (GameOverseer.GO.state != (GameState)myState) {
            GameOverseer.GO.receivedAState = true;
        }
    }

    [PunRPC]
    public void RPC_Shuffled(int[] sentDeckList)
    {
        GameOverseer.GO.receivedDeckList = sentDeckList;
        GameOverseer.GO.enemyShuffled = true;
    }

    [PunRPC]
    public void RPC_SendClick(bool sentButton)
    {
        selectionOverseer.SetEnemyConfirm(sentButton);
    }
}