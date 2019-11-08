using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Dexterity : Card, Interfacer
{
    public Sprite[] interfaceList { get; set; }
    public Card[] cardList { get; set; }
    public int? interfaceSignal { get; set; }

    public void SetSignal(int interfaceSignal)
    {
        this.interfaceSignal = interfaceSignal;
    }

    int[] discardedCardList = new int[Constants.maxCardAmount]; // Lists the deckList indexes of discarded cards
    int discardedCount = 0;


    public Dexterity(HeroEnum hero, string name, int cardID, Sprite image, string text, CardTypes type, int minmax, int cost) :
        base(hero, name, cardID, image, text, type, minmax, false, cost)
    {
        interfaceSignal = null;
    }

    // Run through deckList, if card not active, add it to Interface List
    public void Interfacing(Player user, Player enemy, bool open) {
        // Reset Variables
        cardList = new Card[Constants.maxCardAmount];
        discardedCount = 0;


        // Run through Deck List, check if it's a disabled skill or null card - if yes, then add to discard card list
        for (int i = 0; i < Constants.maxCardAmount; i++) {
            if (user.GetCard(i) != null && user.GetCard(i) != this && !(user.GetDeckCard(i).isActiveAndEnabled || user.GetDeckCard(i).GetIsReaction()) &&
                (user.GetCard(i).GetCardType() == CardTypes.Nullification || user.GetCard(i).GetCardType() == CardTypes.Skill)) { 
                cardList[discardedCount] = user.GetCard(i);
                discardedCardList[discardedCount] = i;
                discardedCount++;
            }
        }

        // Interface script setup
        if (discardedCount > 1) {
            if (open)
                user.Interfacing(cardList, this, discardedCount);
        } else
        {
            interfaceSignal = 0;
        }
    }

    public override void Effect(Player user, Player enemy, int priority)
    {
        switch (priority)
        {
            case 18:
                if (discardedCount > 0) {
                    user.RestoreCard(discardedCardList[(int)interfaceSignal]);
                }
                break;
        }
    }
}