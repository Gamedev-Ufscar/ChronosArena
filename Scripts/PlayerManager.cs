using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int HP = 10;
    public int Charge = 0;
    public int protection = 0;
    public int initialCardCount = 0;
    public List<Card> cardList = new List<Card>();
    public List<CardTypes> attackDisableList = new List<CardTypes>();
    public List<CardTypes> chargeDisableList = new List<CardTypes>();
    public GameObject myHand;
    public GameObject prefabCard;

    // Start is called before the first frame update
    void Start()
    {
        InitializeDeck();
        CreatePlayer();
        CreateTrain(); 
    }

    void InitializeDeck()
    {
        // Initialize Deck
        for (int i = 0; i < initialCardCount; i++)
        {
            cardList.Add(HeroDecks.HD.RobotoDeck(i));
            GameOverseer.GO.cardsToBeSent[GameOverseer.GO.cardsTBSCount] = cardList[i].id;
            GameOverseer.GO.cardsTBSCount++;
        }
    }

    private void CreateCard(int index)
    {
        GameObject card = Instantiate(prefabCard, Vector3.zero, Quaternion.identity);
        card.transform.parent = myHand.transform;
        card.GetComponent<CardInHand>().deckManager = card.transform.parent.GetComponent<DeckManager>();
        card.GetComponent<CardInHand>().cardIndex = index;
        //card.GetComponent<CardInHand>().thisCard = cardList[index];
    }

    private void UpdateHand()
    {

    }

    private void CreatePlayer()
    {
        Debug.Log("Creating player");

        // Creating Cards
        for (int i = 0; i < initialCardCount; i++)
        {
            CreateCard(i);
        }
    }

    private void CreateTrain()
    {
        Debug.Log("Creating train");
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Test"), Vector3.zero, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
