using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Dexterity : Card, Interfacer
{
    public Sprite[] interfaceList { get; set; }
    public int interfaceSignal { get; set; }

    int[] discardedCardList = new int[10]; // Lists the deckList indexes of discarded cards

    // Run through deckList, if card not active, add it to Interface List
    public void interfacing() {
        interfaceList = new Sprite[HeroDecks.HD.myManager.cardList.Length];
        int discardedCount = 0;

        // Run through Deck List, check if it's a disabled card
        for (int i = 0; i < HeroDecks.HD.myManager.myHand.GetComponent<DeckManager>().deckList.Length; i++) {
            if (HeroDecks.HD.myManager.cardList[i] != null && HeroDecks.HD.myManager.myHand.GetComponent<DeckManager>().deckList != null) {
                if (HeroDecks.HD.myManager.cardList[HeroDecks.HD.myManager.myHand.GetComponent<DeckManager>().deckList[i].GetComponent<CardInHand>().thisCard] != this &&
                    HeroDecks.HD.myManager.myHand.GetComponent<DeckManager>().deckList[i].activeInHierarchy == false) {
                    interfaceList[discardedCount] = HeroDecks.HD.myManager.cardList[HeroDecks.HD.myManager.myHand.GetComponent<DeckManager>().deckList[i].GetComponent<CardInHand>().thisCard].image;
                    discardedCardList[discardedCount] = i;
                    discardedCount++;
                    Debug.Log("raise discardedCount");
                }
            }
        }
        HeroDecks.HD.interfaceScript.cardAmount = discardedCount;
        HeroDecks.HD.interfaceScript.interfaceList = interfaceList;
        HeroDecks.HD.interfaceScript.invoker = this;
        Debug.Log("Interface script setup");
        HeroDecks.HD.interfaceScript.gameObject.SetActive(true);
    }

    public override void effect(PlayerManager user, PlayerManager enemy, int priority)
    {
        switch (priority)
        {
            case 18:
                HeroDecks.HD.myManager.RestoreCard(discardedCardList[interfaceSignal]);
                break;
        }
    }
}