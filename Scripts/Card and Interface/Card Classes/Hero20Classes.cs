using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Dexterity : Card, Interfacer
{
    public Sprite[] interfaceList { get; set; }
    public Card[] cardList { get; set; }
    public int interfaceSignal { get; set; }

    int[] discardedCardList = new int[10]; // Lists the deckList indexes of discarded cards

    bool bugCatcher = true;

    public Dexterity(HeroEnum hero, string name, int cardID, Sprite image, string text, CardTypes type, int minmax) :
        base(hero, name, cardID, image, text, type, minmax)
    { }

    // Run through deckList, if card not active, add it to Interface List
    public void interfacing() {
        interfaceList = new Sprite[HeroDecks.HD.myManager.cardList.Length];
        cardList = new Card[HeroDecks.HD.myManager.cardList.Length];
        int discardedCount = 0;


        // Run through Deck List, check if it's a disabled card - if yes, then add to discard card list
        for (int i = 0; i < HeroDecks.HD.myManager.myHand.GetComponent<DeckManager>().deckList.Length; i++) {
            if (HeroDecks.HD.myManager.cardList[i] != null && HeroDecks.HD.myManager.myHand.GetComponent<DeckManager>().deckList != null) {
                if (HeroDecks.HD.myManager.cardList[HeroDecks.HD.myManager.myHand.GetComponent<DeckManager>().deckList[i].GetComponent<CardInHand>().thisCard] != null)
                {
                    if (HeroDecks.HD.myManager.cardList[HeroDecks.HD.myManager.myHand.GetComponent<DeckManager>().deckList[i].GetComponent<CardInHand>().thisCard] != this &&
                        HeroDecks.HD.myManager.myHand.GetComponent<DeckManager>().deckList[i].activeInHierarchy == false)
                    {
                        interfaceList[discardedCount] = HeroDecks.HD.myManager.cardList[HeroDecks.HD.myManager.myHand.GetComponent<DeckManager>().deckList[i].GetComponent<CardInHand>().thisCard].image;
                        cardList[discardedCount] = HeroDecks.HD.myManager.cardList[HeroDecks.HD.myManager.myHand.GetComponent<DeckManager>().deckList[i].GetComponent<CardInHand>().thisCard];
                        discardedCardList[discardedCount] = i;
                        discardedCount++;
                        bugCatcher = false;
                    }
                }
            }
        }

        // Interface script setup
        if (!bugCatcher) {
            interfacingSetup(discardedCount, interfaceList, cardList);
        }
    }

    public override void effect(Player user, Player enemy, int priority)
    {
        switch (priority)
        {
            case 18:
                // Setup discarded list for enemy
                if (user == HeroDecks.HD.enemyManager) {
                    int discardedCount = 0;
                    for (int i = 0; i < HeroDecks.HD.enemyManager.myHand.GetComponent<DeckManager>().deckList.Length; i++) {
                        if (HeroDecks.HD.enemyManager.cardList[i] != null && HeroDecks.HD.enemyManager.myHand.GetComponent<DeckManager>().deckList != null) {
                            if (HeroDecks.HD.enemyManager.cardList[HeroDecks.HD.enemyManager.myHand.GetComponent<DeckManager>().deckList[i].GetComponent<EnemyCardInHand>().thisCard] != null)
                            {
                                if (HeroDecks.HD.enemyManager.cardList[HeroDecks.HD.enemyManager.myHand.GetComponent<DeckManager>().deckList[i].GetComponent<EnemyCardInHand>().thisCard] != this &&
                                HeroDecks.HD.enemyManager.myHand.GetComponent<DeckManager>().deckList[i].activeInHierarchy == false)
                                {
                                    discardedCardList[discardedCount] = i;
                                    discardedCount++;
                                    bugCatcher = false;
                                }
                            }
                        }
                    }
                }

                if (!bugCatcher) { 
                    user.RestoreCard(discardedCardList[interfaceSignal]);
                    Debug.Log("Discarded: " + discardedCardList[interfaceSignal]);
                }
                break;
        }
    }
}