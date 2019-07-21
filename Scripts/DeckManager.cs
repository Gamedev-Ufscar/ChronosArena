using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public PlayerManager playerManager;
    public GameObject[] deckList = new GameObject[10];
    public List<Vector2> cardLocations = new List<Vector2>();
    public Vector2 ultiLocation = new Vector2();
    public bool holdingCard = false;

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
                    }
                }
                else if (gameObject == HeroDecks.HD.enemyManager.myHand) {
                    if (deckList[i].GetComponent<EnemyCardInHand>().cardIndex > cardIndex) {
                        deckList[i].GetComponent<EnemyCardInHand>().cardIndex--;
                    }
                    }
                }
            }
        }
}
