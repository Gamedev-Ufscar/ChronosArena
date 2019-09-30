using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Dexterity : Card, Interfacer
{
    public Sprite[] interfaceList { get; set; }
    public Card[] cardList { get; set; }
    public int interfaceSignal { get; set; }

    public void SetSignal(int interfaceSignal)
    {
        this.interfaceSignal = interfaceSignal;
    }

    int[] discardedCardList = new int[Constants.maxHandSize]; // Lists the deckList indexes of discarded cards

    bool bugCatcher = true;

    public Dexterity(HeroEnum hero, string name, int cardID, Sprite image, string text, CardTypes type, int minmax, int cost) :
        base(hero, name, cardID, image, text, type, minmax, false, cost)
    { }

    // Run through deckList, if card not active, add it to Interface List
    public void Interfacing(Player user, Player enemy) {
        cardList = new Card[Constants.maxHandSize];
        int discardedCount = 0;


        // Run through Deck List, check if it's a disabled skill or null card - if yes, then add to discard card list
        for (int i = 0; i < Constants.maxHandSize; i++) {
            if (user.GetCard(i) != null && user.GetCard(i) != this && !user.GetDeckCard(i).isActiveAndEnabled &&
                (user.GetCard(i).GetCardType() == CardTypes.Nullification || user.GetCard(i).GetCardType() == CardTypes.Skill)) { 
                cardList[discardedCount] = user.GetCard(i);
                discardedCardList[discardedCount] = i;
                discardedCount++;
                bugCatcher = false;
            }
        }

        // Interface script setup
        if (!bugCatcher) {
            user.Interfacing(cardList, this);
        }
    }

    public override void Effect(Player user, Player enemy, int priority)
    {
        switch (priority)
        {
            case 18:
                if (!bugCatcher) { 
                    user.RestoreCard(discardedCardList[interfaceSignal]);
                    Debug.Log("Discarded: " + discardedCardList[interfaceSignal]);
                    // CHECK LATER
                }
                break;
        }
    }
}