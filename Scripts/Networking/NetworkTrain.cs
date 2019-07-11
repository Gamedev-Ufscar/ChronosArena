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
        // Send hovering card
        if (GameOverseer.GO.hoveringCard != -1 || GameOverseer.GO.sentCard) {
            Debug.Log("HoveringCard: " + GameOverseer.GO.hoveringCard + ", sentCard: " + GameOverseer.GO.sentCard);
            PV.RPC("RPC_hoverPos", RpcTarget.OthersBuffered, GameOverseer.GO.sentCard, GameOverseer.GO.hoveringCard,
                                                            GameOverseer.GO.hoveringCardPos, GameOverseer.GO.hoveringCardLocalPos);
        }
        else {
            PV.RPC("RPC_ecoHoverPos", RpcTarget.OthersBuffered);
        }

        if (time >= 0.1f) {
            // Send State
            if (GameOverseer.GO.changingStates == true)
            {
                PV.RPC("RPC_SendState", RpcTarget.OthersBuffered, GameOverseer.GO.state);
                GameOverseer.GO.changingStates = false;
            }

            // Send Confirm
            PV.RPC("RPC_SendClick", RpcTarget.OthersBuffered, GameOverseer.GO.myConfirm);

            // Send initial cards
            if (GameOverseer.GO.cardsTBSCount > 0)
            {
                PV.RPC("RPC_sendCard", RpcTarget.OthersBuffered, GameOverseer.GO.cardsToBeSent, GameOverseer.GO.cardsTBSCount);
            }

            if (GameOverseer.GO.cardsReceivedCount > 0)
            {
                PV.RPC("RPC_confirmedSendCard", RpcTarget.OthersBuffered);
                GameOverseer.GO.cardsReceivedCount = 0;
            }

            // Time stuff
            time = 0f;
        } else {
            time += Time.deltaTime;
        }

        // Reset bool signals
        GameOverseer.GO.sentCard = false;
    }

    [PunRPC]
    public void RPC_SendState(GameState myState)
    {
        GameOverseer.GO.state = myState;
        GameOverseer.GO.myConfirm = false;
        if (myState == GameState.Effects) { GameOverseer.GO.activateCards(); }
    }

    [PunRPC]
    public void RPC_SendClick(bool sentButton)
    {
        GameOverseer.GO.enemyConfirm = sentButton;
    }

    [PunRPC]
    public void RPC_hoverPos(bool sentCard, int hoverCard, Vector3 hoverPos, Vector3 hoverLocalPos)
    {
        GameOverseer.GO.enemyHoveringCard = hoverCard;

        if (sentCard)
        {
            Debug.Log("Received enemySentCard, it's " + hoverCard);
            GameOverseer.GO.enemySentCard = sentCard;
        }

        if (hoverCard != -1)
        {
            //Debug.Log("Enemy Hover Card: " + GameOverseer.GO.enemyHoveringCard);
            GameOverseer.GO.enemyHoveringCardPos = hoverPos;
            GameOverseer.GO.enemyHoveringCardLocalPos = hoverLocalPos;
        }
    }

    [PunRPC]
    public void RPC_ecoHoverPos()
    {
        GameOverseer.GO.enemyHoveringCard = -1;
    }

    [PunRPC]
    public void RPC_sendCard(int[] cardsSent, int count)
    {
        GameOverseer.GO.cardsReceived = cardsSent;
        GameOverseer.GO.cardsReceivedCount = count;
    }

    [PunRPC]
    public void RPC_confirmedSendCard()
    {
        GameOverseer.GO.cardsTBSCount = 0;
    }
}