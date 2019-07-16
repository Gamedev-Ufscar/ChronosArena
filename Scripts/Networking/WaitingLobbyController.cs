using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingLobbyController : MonoBehaviour
{
    public int networkSceneIndex;
    public int gameSceneIndex;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.PlayerList.Length >= 2)
        {
            StartGame();
        }
    }

    private void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Opening game");
            PhotonNetwork.LoadLevel(gameSceneIndex);
        }
    }

    public void LobbyCancel() // Paired to Quick Cancel Button
    {
        PhotonNetwork.LoadLevel(networkSceneIndex);
        PhotonNetwork.LeaveRoom();
    }
}
