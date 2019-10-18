using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkTrain : MonoBehaviour
{
    public static NetworkTrain networkTrain;
    [SerializeField]
    private SelectionOverseer selectionOverseer;
    [SerializeField]
    private PhotonView PV;

    private bool wasSelection = false;

    // Constructor
    public void GiveSelectionOverseer(SelectionOverseer selectionOverseer)
    {
        this.selectionOverseer = selectionOverseer;
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
        if (SceneManager.GetActiveScene().buildIndex == (int)SceneList.Selection && selectionOverseer == null)
        {
            FindSelectionOverseer();
        }
        
        if (SceneManager.GetActiveScene().buildIndex == (int)SceneList.Game) {
            Destroy(gameObject);
        }
    }

    public void FindSelectionOverseer()
    {
        selectionOverseer = GameObject.Find("Selection Overseer").GetComponent<SelectionOverseer>();
    }


    // Send functions
    // CONFIRM
    public void SendConfirm(bool myConfirm)
    {
        // Send Confirm
        PV.RPC("RPC_SendConfirm", RpcTarget.OthersBuffered, myConfirm);
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



    // RPC functions
    [PunRPC]
    public void RPC_SendConfirm(bool sentButton)
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
        if (selectionOverseer != null)
            selectionOverseer.EnemyHeroHover(heroHover);
    }
    [PunRPC]
    public void RPC_stopHeroHover(int heroHover)
    {
        if (selectionOverseer != null)
            selectionOverseer.EnemyHeroHoverStop(heroHover);
    }
}