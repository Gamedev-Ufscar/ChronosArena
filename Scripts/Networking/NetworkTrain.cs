using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkTrain : MonoBehaviour
{
    PhotonView PV;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        // Send State
        if (GameOverseer.GO.changingStates == true) {
            PV.RPC("RPC_SendState", RpcTarget.OthersBuffered, GameOverseer.GO.state);
            GameOverseer.GO.changingStates = false;
        }

        PV.RPC("RPC_SendClick", RpcTarget.OthersBuffered, GameOverseer.GO.myConfirm);
        PV.RPC("RPC_hoverPos", RpcTarget.OthersBuffered, GameOverseer.GO.sentCard, GameOverseer.GO.hoveringCard, 
                                                        GameOverseer.GO.hoveringCardPos, GameOverseer.GO.hoveringCardLocalPos);

        if (GameOverseer.GO.cardIndex != -1)
        {
            PV.RPC("RPC_sendCard", RpcTarget.OthersBuffered, GameOverseer.GO.cardToBeSent, GameOverseer.GO.cardIndex, GameOverseer.GO.isOver);
        }

        // Reset bool signals
        GameOverseer.GO.sentCard = false;
    }

    [PunRPC]
    public void RPC_SendState(GameState myState)
    {
        GameOverseer.GO.state = myState;
        GameOverseer.GO.myConfirm = false;
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
    public void RPC_sendCard(Card packageCard, int cardIndex, bool isOver)
    {
        
    }
}
