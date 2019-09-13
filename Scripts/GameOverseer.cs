using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverseer : MonoBehaviour
{
    public Player myPlayer;
    public Player enemyPlayer;
    public NetworkTrain networkTrain;

    private GameState state = GameState.Purchase;

    private void Awake()
    {
        GameObject netObject = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Test"), Vector3.zero, Quaternion.identity);
        networkTrain = netObject.AddComponent<NetworkTrain>();
        networkTrain = new NetworkTrain(this);
    }

    // Getter
    public GameState GetState()
    {
        return state;
    }

    // Network Sender
    public void SendCard()
    {
        networkTrain.SendCard();
    }

    public void SendShuffle(int[] cardIndexes)
    {
        networkTrain.SendShuffle(cardIndexes);
    }

    // Network Receiver
    public void ReceiveShuffle(int[] receivedCardIndexes)
    {
        enemyPlayer.ReceiveShuffle(receivedCardIndexes);
    }

}
