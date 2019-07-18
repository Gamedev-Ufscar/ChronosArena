using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkTrain : MonoBehaviour
{
    PhotonView PV;
    float time = 0f;

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
            if (GameOverseer.GO.hoveringCard != -1) {
            PV.RPC("RPC_hoverPos", RpcTarget.OthersBuffered, (byte)GameOverseer.GO.hoveringCard, GameOverseer.GO.amIHoveringMyself,
                                     (Vector2)GameOverseer.GO.hoveringCardPos, (Vector2)GameOverseer.GO.hoveringCardLocalPos);
            } else {
                PV.RPC("RPC_ecoHoverPos", RpcTarget.OthersBuffered);
            }

            // Summon Card
            if (GameOverseer.GO.sentCard > 0 && GameOverseer.GO.state == GameState.Choice)
            {
                for (int i = 1; i <= 8; i++)
                {
                    PV.RPC("RPC_sentCard", RpcTarget.OthersBuffered);
                    Debug.Log("Sent Card");
                }
            }

            // Send Ulti Purchase
            if (GameOverseer.GO.state == GameState.Purchase && SceneManager.GetActiveScene().buildIndex == 3) {
                PV.RPC("RPC_ultiStuff", RpcTarget.OthersBuffered, GameOverseer.GO.ultiBuy, (byte)HeroDecks.HD.myManager.Charge);
            }

            // Send State
            if (GameOverseer.GO.changingStates == true)
            {
                PV.RPC("RPC_SendState", RpcTarget.OthersBuffered, (byte)GameOverseer.GO.state);
                GameOverseer.GO.changingStates = false;
            }

            // Send Confirm
            PV.RPC("RPC_SendClick", RpcTarget.OthersBuffered, GameOverseer.GO.myConfirm);


            // Send chosen Hero
            PV.RPC("RPC_SendHero", RpcTarget.OthersBuffered, (byte)GameOverseer.GO.myHero);

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
        GameOverseer.GO.enemyHoveringCard = -1;
    }

    [PunRPC]
    public void RPC_sentCard() {
        Debug.Log("Received enemySentCard");
        GameOverseer.GO.enemySentCard = true;
    }

    [PunRPC]
    public void RPC_SendHero(byte myHero)
    {
        GameOverseer.GO.enemyHero = (int)myHero;
    }


    [PunRPC]
    public void RPC_ultiStuff(bool ultiBuy, byte charge)
    {
        GameOverseer.GO.enemyUltiBuy = ultiBuy;
        HeroDecks.HD.enemyManager.Charge = (int)charge;
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
        GameOverseer.GO.enemyConfirm = sentButton;
    }
}