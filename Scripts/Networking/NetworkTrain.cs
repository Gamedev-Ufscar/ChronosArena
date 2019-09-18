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
    }

    
    // Send functions
    public void SendConfirm(bool myConfirm)
    {
        // Send Confirm
        PV.RPC("RPC_SendClick", RpcTarget.OthersBuffered, myConfirm);
    }

    public void SummonCard(int cardID)
    {
        // Summon Card
        PV.RPC("RPC_summonedCard", RpcTarget.OthersBuffered, cardID);
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

    public void SendShuffle(int[] cardIndexes)
    {
        // Send shuffle
        PV.RPC("RPC_Shuffled", RpcTarget.OthersBuffered, cardIndexes);
    }

    public void SendInterfaceSignal(int interfaceSignalSent)
    {
        
        PV.RPC("RPC_sendInterfaceSignal", RpcTarget.OthersBuffered, (byte)interfaceSignalSent);
        
    }

    public void SendCardPosition(int id, bool hoveringMyself, Vector2 position, Vector2 localPosition)
    {
        PV.RPC("RPC_hoverPos", RpcTarget.OthersBuffered, (byte)id, hoveringMyself, (Vector2)position, (Vector2)localPosition);
    }

    public void SendPositionStop()
    {
        PV.RPC("RPC_ecoHoverPos", RpcTarget.OthersBuffered);
    }

    public void SendUltiPurchase(int cardID, bool bought, int charge)
    {
        // Send Ulti Purchase
            PV.RPC("RPC_ultiStuff", RpcTarget.OthersBuffered, cardID, bought, (byte)charge);
    }

    // RPC functions
    [PunRPC]
    public void RPC_hoverPos(byte hoverCard, bool amIHoveringMyself, Vector2 hoverPos, Vector2 hoverLocalPos)
    {
        // TODO
    }
    [PunRPC]
    public void RPC_ecoHoverPos()
    {
        
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
    public void RPC_summonedCard(byte cardID) {
        gameOverseer.ReceiveSummon();
    }

    [PunRPC]
    public void RPC_SendHero(int myHero)
    {
        selectionOverseer.SetEnemyHero(myHero);
    }

    [PunRPC]
    public void RPC_sendInterfaceSignal(byte signalSent) {
        if (gameOverseer.GetEnemyPlayer().GetCardPlayed() != null) {
            if (gameOverseer.GetEnemyPlayer().GetCardPlayed() is Interfacer) {
                Interfacer cc = (Interfacer)gameOverseer.GetEnemyPlayer().GetCardPlayed();
                cc.interfaceSignal = (int)signalSent;
            }
        }
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
    public void RPC_Shuffled(int[] receivedCardIndexes)
    {
        gameOverseer.ReceiveShuffle(receivedCardIndexes);
    }

    [PunRPC]
    public void RPC_SendClick(bool sentButton)
    {
        selectionOverseer.SetEnemyConfirm(sentButton);
    }
}