using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkTrain : MonoBehaviour
{
    public static NetworkTrain networkTrain;
    [SerializeField]
    private GameOverseer gameOverseer;
    [SerializeField]
    private SelectionOverseer selectionOverseer;
    [SerializeField]
    private PhotonView PV;

    private bool wasSelection = false;

    // Constructor
    public void GiveGameOverseer(GameOverseer gameOverseer)
    {
        this.gameOverseer = gameOverseer;
    }

    public void GiveSelectionOverseer(SelectionOverseer selectionOverseer)
    {
        this.selectionOverseer = selectionOverseer;
        wasSelection = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
    }

    private void Awake()
    {
        Application.runInBackground = true;
        PV = GetComponent<PhotonView>();

        if (PV.IsMine)
        {
            NetworkTrain.networkTrain = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (SceneManager.GetActiveScene().buildIndex == 2 && selectionOverseer == null)
        {
            selectionOverseer = GameObject.Find("Selection Overseer").GetComponent<SelectionOverseer>();
        } else if (SceneManager.GetActiveScene().buildIndex == 3 && gameOverseer == null)
        {
            gameOverseer = GameObject.Find("Game Overseer").GetComponent<GameOverseer>();
        }
        */
        if (wasSelection && SceneManager.GetActiveScene().buildIndex == 3) {
            Destroy(gameObject);
        }
    }


    // Send functions
    // CONFIRM
    public void SendConfirm(bool myConfirm)
    {
        // Send Confirm
        PV.RPC("RPC_SendClick", RpcTarget.OthersBuffered, myConfirm);
    }

    // CHOOSE HERO
    public void SendChosenHero(int heroIndex)
    {
        // Send chosen Hero
        PV.RPC("RPC_SendHero", RpcTarget.OthersBuffered, (byte)heroIndex);
    }


    // HERO HOVER
    public void SendHoverHero(int heroIndex)
    {
        // Send hovering hero
        PV.RPC("RPC_heroHover", RpcTarget.OthersBuffered, heroIndex);
    }

    public void SendHoverHeroStop(int heroIndex)
    {
        PV.RPC("RPC_stopHeroHover", RpcTarget.OthersBuffered, heroIndex);
    }

    // SHUFFLE
    public void SendShuffle(int[] cardIndexes)
    {
        // Send shuffle
        PV.RPC("RPC_Shuffled", RpcTarget.OthersBuffered, cardIndexes);
    }

    // CARD POSITION
    public void SendCardPosition(int id, Vector2 position)
    {
        PV.RPC("RPC_cardPos", RpcTarget.OthersBuffered, (byte)id, (Vector2)position);
    }

    public void SendCardPositionStop()
    {
        PV.RPC("RPC_cardPosStop", RpcTarget.OthersBuffered);
    }

    // ULTI HOVER
    public void SendUltiHover(int id, bool hoveringMyself)
    {
        PV.RPC("RPC_ultiHover", RpcTarget.OthersBuffered, (byte)id, hoveringMyself);
    }

    public void SendUltiStop()
    {
        PV.RPC("RPC_ultiHoverStop", RpcTarget.OthersBuffered);
    }

    // ULTI PURCHASE
    public void SendUltiPurchase(int cardID, bool bought, int charge)
    {
        // Send Ulti Purchase
            PV.RPC("RPC_ultiStuff", RpcTarget.OthersBuffered, cardID, bought, (byte)charge);
    }

    // SUMMON
    public void SummonCard(int cardID)
    {
        // Summon Card
        PV.RPC("RPC_summonedCard", RpcTarget.OthersBuffered, cardID);
        Debug.Log("Sent Card");

    }

    // INTERFACE
    public void SendInterfaceSignal(int interfaceSignalSent)
    {

        PV.RPC("RPC_sendInterfaceSignal", RpcTarget.OthersBuffered, (byte)interfaceSignalSent);

    }



    // RPC functions
    [PunRPC]
    public void RPC_SendClick(bool sentButton)
    {
        selectionOverseer.SetEnemyConfirm(sentButton);
    }

    [PunRPC]
    public void RPC_SendHero(byte myHero)
    {
        selectionOverseer.SetEnemyHero((int)myHero);
    }

    [PunRPC]
    public void RPC_heroHover(int heroHover)
    {
        selectionOverseer.EnemyHeroHover(heroHover);
    }
    [PunRPC]
    public void RPC_stopHeroHover(int heroHover)
    {
        selectionOverseer.EnemyHeroHoverStop(heroHover);
    }

    [PunRPC]
    public void RPC_Shuffled(int[] receivedCardIndexes)
    {
        gameOverseer.ReceiveShuffle(receivedCardIndexes);
    }

    [PunRPC]
    public void RPC_cardPos(byte hoverCard, Vector2 hoverPos)
    {
        gameOverseer.ReceiveCardPosition((int)hoverCard, hoverPos);
    }
    [PunRPC]
    public void RPC_cardPosStop()
    {
        gameOverseer.ReceiveCardPositionStop();
    }

    [PunRPC]
    public void RPC_ultiHover(byte hoverCard, bool hoveringMyself)
    {
        gameOverseer.ReceiveUltiHover((int)hoverCard, hoveringMyself);
    }
    [PunRPC]
    public void RPC_ultiHoverStop()
    {
        gameOverseer.ReceiveUltiHoverStop();
    }

    [PunRPC]
    public void RPC_ultiStuff(int cardID, bool bought, byte charge)
    {
        if (SceneManager.GetActiveScene().buildIndex == 3) {
            gameOverseer.GetEnemyPlayer().GetUltiCard(cardID).SetBought(bought);
            gameOverseer.GetEnemyPlayer().SetCharge((int)charge);
        }
    }

    [PunRPC]
    public void RPC_summonedCard(byte cardID)
    {
        gameOverseer.ReceiveSummon();
    }

    [PunRPC]
    public void RPC_sendInterfaceSignal(byte signalSent)
    {
        if (gameOverseer.GetEnemyPlayer().GetCardPlayed() != null)
        {
            if (gameOverseer.GetEnemyPlayer().GetCardPlayed() is Interfacer)
            {
                Interfacer cc = (Interfacer)gameOverseer.GetEnemyPlayer().GetCardPlayed();
                cc.interfaceSignal = (int)signalSent;
            }
        }
    }
}