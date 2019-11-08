using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkBahn : MonoBehaviour
{
    public static NetworkBahn networkBahn;
    [SerializeField]
    private GameOverseer gameOverseer;
    [SerializeField]
    private PhotonView PV;

    private bool wasSelection = false;

    // Constructor
    public void GiveGameOverseer(GameOverseer gameOverseer)
    {
        this.gameOverseer = gameOverseer;
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
            NetworkBahn.networkBahn = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == (int)SceneList.Game && gameOverseer == null)
        {
            FindGameOverseer();
        }
    }

    public void FindGameOverseer()
    {
        gameOverseer = GameObject.Find("Game Overseer").GetComponent<GameOverseer>();
    }


    // Send functions
    // CONFIRM
    public void SendConfirm(bool myConfirm)
    {
        // Send Confirm
        PV.RPC("RPC_SendConfirm", RpcTarget.OthersBuffered, myConfirm);
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

    public void SendUltiStop(bool hoveringMyself)
    {
        PV.RPC("RPC_ultiHoverStop", RpcTarget.OthersBuffered, hoveringMyself);
    }

    // ULTI PURCHASE
    public void SendUltiPurchase(int cardID, bool bought, int charge)
    {
        // Send Ulti Purchase
        PV.RPC("RPC_ultiStuff", RpcTarget.OthersBuffered, (byte)cardID, bought, (byte)charge);
    }

    // SUMMON
    public void UnleashCard(int cardID)
    {
        // Summon Card
        PV.RPC("RPC_unleashedCard", RpcTarget.OthersBuffered, (byte)cardID);
        Debug.Log("Sent Card");

    }

    // INTERFACE
    public void SendInterfaceSignal(int interfaceSignalSent)
    {
        PV.RPC("RPC_sendInterfaceSignal", RpcTarget.OthersBuffered, (byte)interfaceSignalSent);
    }



    // RPC functions
    [PunRPC]
    public void RPC_SendConfirm(bool sentButton)
    {
        gameOverseer.SetEnemyConfirm(sentButton);
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
        if (gameOverseer != null)
            gameOverseer.ReceiveCardPositionStop();
    }

    [PunRPC]
    public void RPC_ultiHover(byte hoverCard, bool hoveringMyself)
    {
        gameOverseer.ReceiveUltiHover((int)hoverCard, hoveringMyself);
    }
    [PunRPC]
    public void RPC_ultiHoverStop(bool hoveringMyself)
    {
        if (gameOverseer != null)
            gameOverseer.ReceiveUltiHover(null, hoveringMyself);
    }

    [PunRPC]
    public void RPC_ultiStuff(byte cardID, bool bought, byte charge)
    {
        if (SceneManager.GetActiveScene().buildIndex == (int)SceneList.Game)
        {
            gameOverseer.ReceiveUltiPurchase(cardID, bought, charge);
            Debug.Log("Received purchase");
        }
    }

    [PunRPC]
    public void RPC_unleashedCard(byte cardID)
    {
        gameOverseer.ReceiveUnleash(cardID);
    }

    [PunRPC]
    public void RPC_sendInterfaceSignal(byte signalSent)
    {
        if (gameOverseer.GetEnemyPlayer().GetCardPlayed() != null && gameOverseer.GetEnemyPlayer().GetCardPlayed() is Interfacer)
        {
            Interfacer cc = (Interfacer)gameOverseer.GetEnemyPlayer().GetCardPlayed();
            cc.interfaceSignal = (int)signalSent;
            Debug.Log("Signal received");
        }
    }
}