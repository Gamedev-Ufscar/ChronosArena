using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public PlayerManager playerManager;
    public GameObject[] deckList = new GameObject[10];
    public GameObject[] ultiList = new GameObject[3];
    public List<Vector2> cardLocations = new List<Vector2>();
    public List<Vector2> ultiLocations = new List<Vector2>();
    public List<Vector2> reactionLocations = new List<Vector2>();
    public bool holdingCard = false;
    public int hoveringUlti = 200;

    [HideInInspector]
    public int cardTotalCount;
    public int activeCardCount;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameOverseer.GO.enemyShuffled && gameObject == HeroDecks.HD.enemyManager.myHand) {
            GameOverseer.GO.enemyShuffled = false;
            for (int i = 0; i < deckList.Length; i++) {
                if (deckList[i] != null) {
                    deckList[i].GetComponent<EnemyCardInHand>().cardIndex = GameOverseer.GO.receivedDeckList[i];
                    Debug.Log("i: " + i + ", Card Index: " + GameOverseer.GO.receivedDeckList[i]);
                }
            }
        }
    }

    public void Shuffle()
    {
        int helper = 0, rando = 0;
        for (int i = 0; i < deckList.Length; i++) {
            rando = Random.Range(0, deckList.Length);
            if (deckList[i] != null && deckList[i].activeInHierarchy) {
                if (deckList[rando] != null && deckList[rando].activeInHierarchy) {
                    helper = deckList[i].GetComponent<CardInHand>().cardIndex;
                    deckList[i].GetComponent<CardInHand>().cardIndex = deckList[rando].GetComponent<CardInHand>().cardIndex;
                    deckList[rando].GetComponent<CardInHand>().cardIndex = helper;
                    HeroDecks.HD.audioManager.CardSound();
                }
            }
        }

        for (int i = 0; i < deckList.Length; i++) {
            if (deckList[i] != null)
                GameOverseer.GO.sentDeckList[i] = deckList[i].GetComponent<CardInHand>().cardIndex;
        }
        GameOverseer.GO.shuffled = 3;
    }

    public void recedeDeck(int cardIndex)
   {
        for (int i = 0; i < deckList.Length; i++)
        {
            if (deckList[i] != null && deckList[i].activeInHierarchy) {
                if (gameObject == HeroDecks.HD.myManager.myHand) {
                    if (deckList[i].GetComponent<CardInHand>().cardIndex > cardIndex) {
                        deckList[i].GetComponent<CardInHand>().cardIndex--;
                        HeroDecks.HD.audioManager.CardSound();
                    }
                }
                else if (gameObject == HeroDecks.HD.enemyManager.myHand) {
                    if (deckList[i].GetComponent<EnemyCardInHand>().cardIndex > cardIndex) {
                        deckList[i].GetComponent<EnemyCardInHand>().cardIndex--;
                        HeroDecks.HD.audioManager.CardSound();
                    }
                }
            }
        }
   }

    public void recedeUlti(int cardIndex)
    {
        for (int i = 0; i < ultiList.Length; i++)
        {
            if (ultiList[i] != null && ultiList[i].activeInHierarchy) {
                if (ultiList[i].GetComponent<UltimateCard>().cardIndex > cardIndex) {
                    ultiList[i].GetComponent<UltimateCard>().cardIndex--;
                    HeroDecks.HD.audioManager.CardSound();
                }
            }
        }
    }

    public int placeUltimate(int staticCardIndex)
    {
        // Find a position for the ultimate
        int myCardIndex = 200;

        for (int i = 0; i < ultiList.Length; i++)
        {
            if (ultiList[i] != null) { 
                // If this is my card or is offline...
                if (ultiList[i].activeInHierarchy == false || ultiList[i].GetComponent<UltimateCard>().staticCardIndex == staticCardIndex) {
                    // Choose this place if haven't yet
                    if (myCardIndex == 200) { myCardIndex = ultiList[i].GetComponent<UltimateCard>().cardIndex; }
                    //break;
                // If this is supposed to be further right than me...
                } else if (ultiList[i].GetComponent<UltimateCard>().staticCardIndex > staticCardIndex) {
                    // Choose this place if haven't yet, push current to right
                    if (myCardIndex == 200) { myCardIndex = ultiList[i].GetComponent<UltimateCard>().cardIndex; }
                    ultiList[i].GetComponent<UltimateCard>().cardIndex++;
                }
            }
        }
        if (myCardIndex == 200) { myCardIndex = 100 + ultiList.Length - 1; }

        return myCardIndex;
    }
}
