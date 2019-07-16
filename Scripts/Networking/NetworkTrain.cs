using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkTrain : MonoBehaviour
{
    PhotonView PV;
    float time = 0f;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
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
            if (GameOverseer.GO.state == GameState.Purchase) {
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

            // Send initial cards
            if (GameOverseer.GO.cardsTBSCount > 0) {
                PV.RPC("RPC_sendCard", RpcTarget.OthersBuffered, GameOverseer.GO.cardsToBeSent, (byte)GameOverseer.GO.cardsTBSCount,
                                                                 (byte)GameOverseer.GO.ultiToBeSent);
            }

            if (GameOverseer.GO.cardsReceivedCount > 0) {
                PV.RPC("RPC_confirmedSendCard", RpcTarget.OthersBuffered);
                GameOverseer.GO.cardsReceivedCount = 0;
            }

            // Reset bool signals
            if (GameOverseer.GO.sentCard > 0)
            {
                GameOverseer.GO.sentCard--;
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
    public void RPC_SendClick(bool sentButton)
    {
        GameOverseer.GO.enemyConfirm = sentButton;
    }

    [PunRPC]
    public void RPC_sendCard(int[] cardsSent, byte count, byte ultiSent)
    {
        GameOverseer.GO.cardsReceived = cardsSent;
        GameOverseer.GO.cardsReceivedCount = count;
        GameOverseer.GO.ultiReceived = ultiSent;
    }
    [PunRPC]
    public void RPC_confirmedSendCard()
    {
        GameOverseer.GO.cardsTBSCount = 0;
    }
}