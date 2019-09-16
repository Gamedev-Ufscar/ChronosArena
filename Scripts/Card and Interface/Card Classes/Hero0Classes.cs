using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugaScream : Card
{
    public BugaScream(HeroEnum hero, string name, int cardID, Sprite image, string text, CardTypes type, int minmax) :
        base(hero, name, cardID, image, text, type, minmax)
    { }

    public override void Effect(Player user, Player enemy, int priority)
    {
        switch (priority) {
            case 16:
                enemy.DealDamage(1, false);
                Debug.Log(user.gameObject.name + "'s Buga Scream");
                break;

            case 17:
                if (user.GetCard(0) != null) {
                    Attack cc = (Attack)user.GetCard(0);
                    cc.damage++;
                    user.SetCard((Card)cc, 0);
                }
                RaiseCost(1);

                break;
        }
    }
}

public class WatchAdjustments : Card, ChargeInterface, Limit, Interfacer
{
    public int charge { get; set; }
    public int limit { get; set; }
    public int limitMax { get; set; }
    public Sprite[] interfaceList { get; set; }
    public string[] textList { get; set; }
    public int interfaceSignal { get; set; }

    public WatchAdjustments(HeroEnum hero, string name, int cardID, Sprite image, string text, CardTypes type, int minmax) :
        base(hero, name, cardID, image, text, type, minmax)
    { }

    public void SetSignal(int interfaceSignal)
    {
        this.interfaceSignal = interfaceSignal;
    }

    // Two choices
    public void RaiseCharge(int charge, Player target)
    {
        target.RaiseCharge(charge);
    }

    public void RaiseLimit(int amount, Player target) {
        for (int i = 0; i < 14; i++) { 
            if (target.GetCard(i) as Charge != null) {
                Charge cc = target.GetCard(i) as Charge;
                cc.limit += amount;
                target.SetCard(cc as Card, i);
                if (cc.limit >= cc.limitMax) {
                    DisableCards(target);
                }
            }
        }
    }

    public void DisableCards(Player target) {
        for (int i = 0; i < 14; i++) {
            if (target.GetCard(i) != null) {
                foreach (CardTypes d in target.GetChargeDisable()) {
                    if (target.GetCard(i).GetCardType() == d) {
                        target.GetCard(i).SetTurnsTill(1);
                        Debug.Log(target.GetCard(i).GetName() + " Disabled");
                    }
                }
            }
        }
    }

    public void Interfacing(Player user, Player enemy)
    {
        textList = new string[2];
        textList[0] = "+1c .";
        textList[1] = "+2c , -2g   .";

        user.Interfacing(this, textList, this);
    }

    public override void Effect(Player user, Player enemy, int priority)
    {
        switch (priority)
        {
            case 14:
                if (interfaceSignal == 0) {
                    RaiseCharge(1, user);
                } else if (interfaceSignal == 1) {
                    RaiseCharge(2, user);
                    user.DealDamage(2, false);
                }
                RaiseLimit(1, user);
                break;
        }
    }
}

public class TimeLock : Card
{
    public TimeLock(HeroEnum hero, string name, int cardID, Sprite image, string text, CardTypes type, int minmax) :
        base(hero, name, cardID, image, text, type, minmax)
    { }

    public override void Effect(Player user, Player enemy, int priority)
    {
        switch (priority)
        {

            case 00:
                user.SetSideEffect(0, user.GetHP() * 100 + enemy.GetHP());
                Debug.Log(user.gameObject.name + "'s Time Lock");
                break;

            case 14:
                user.RaiseCharge(1);
                break;

        }
    }
}

public class DejaVu : Card
{
    public DejaVu(HeroEnum hero, string name, int cardID, Sprite image, string text, CardTypes type, int minmax) :
        base(hero, name, cardID, image, text, type, minmax)
    { }

    public override void Effect(Player user, Player enemy, int priority)
    {
        switch (priority)
        {
            case 8:
                user.Protect(2);
                break;
            case 17:
                user.SetSideEffect(2, 2);
                break;
        }
    }
}

public class ChronosMachine : Card, Interfacer
{
    public Sprite[] interfaceList { get; set; }
    public string[] textList { get; set; }
    public int interfaceSignal { get; set; }
    public bool isChronos { get; set; }

    public ChronosMachine(HeroEnum hero, string name, int cardID, Sprite image, string text, CardTypes type, int minmax, bool isChronos) :
        base(hero, name, cardID, image, text, type, minmax)
    {
        this.isChronos = isChronos;
    }

    public void SetSignal(int interfaceSignal)
    {
        this.interfaceSignal = interfaceSignal;
    }

    // Two choices
    public void Interfacing(Player user, Player enemy)
    {
        if (!isChronos)
        {
            textList = new string[2];
            textList[0] = "VOLTA O g      DO USUÁRIO.";
            textList[1] = "VOLTA O g      DO INIMIGO.";

            user.Interfacing(this, textList, this);
        } else
        {
            interfaceSignal = 0;
        }
    }


    public override void Effect(Player user, Player enemy, int priority)
    {
        switch (priority)
        {
            case 12:
                if (isChronos) {
                    if (user.GetSideEffectValue(0) != 0) {
                        user.SetSideEffect(1, user.GetHP() * 100 + enemy.GetHP());

                        user.SetHP(user.GetSideEffectValue(0) / 100);
                        enemy.SetHP(user.GetSideEffectValue(0) % 100);
                        RaiseCost(1);
                        Debug.Log("Chronos Machine!");
                    }
                } else {
                    if (user.GetSideEffectValue(1) != 0) {
                        if (interfaceSignal == 0) {
                            user.SetHP(user.GetSideEffectValue(1) / 100);
                        }
                        if (interfaceSignal == 1) {
                            enemy.SetHP(user.GetSideEffectValue(1) % 100);
                        }
                    }
                }
                
                break;
        }
    }
}

public class Sabotage : Card, Damage, Protection, NullInterface, Interfacer
{
    public CardTypes[] nullificationList { get; set; }
    public bool wronged { get; set; }
    public int damage { get; set; }
    public bool isUnblockable { get; set; }
    public int protection { get; set; }
    public Sprite[] interfaceList { get; set; }
    public Card[] cardList { get; set; }
    public int interfaceSignal { get; set; }

    int[] discardedCardList = new int[10]; // Lists the deckList indexes of discarded cards
    bool bugCatcher = true;

    public int nullType = 0;

    public Sabotage(HeroEnum hero, string name, int cardID, Sprite image, string text, CardTypes type, int minmax) :
        base(hero, name, cardID, image, text, type, minmax)
    {
    }

    public void SetSignal(int interfaceSignal)
    {
        this.interfaceSignal = interfaceSignal;
    }

    public void CauseDamage(int damage, Player target)
    {
        target.DealDamage(2, false);
    }

    public void SetIsUnblockable(bool isUnblockable)
    {
        this.isUnblockable = isUnblockable;
    }

    public void Protect(int protection, Player target)
    {
        target.Protect(2);
    }

    public void Nullify(Player target)
    {
        wronged = true;
        for (int i = 0; i < nullificationList.Length; i++)
        {
            if (target.GetCard(i).GetCardType() == nullificationList[i])
            {
                target.GetCard(i).SetIsNullified(true);
                wronged = false;
            }
        }
    }

    public void Interfacing(Player user, Player enemy)
    {
        cardList = new Card[Constants.maxCardAmount];
        int discardedCount = 0;


        if (nullType == 2)
        {
            // First check if the Nullification is valid - if yes, proceed
            wronged = true;
            for (int i = 0; i < nullificationList.Length; i++) {
                if (enemy.GetCardPlayed().GetCardType() == nullificationList[i]) {
                    wronged = false;
                }
            }

            if (!wronged)
            {

                // Run through Deck List, check if there's a disabled nullification card
                for (int i = 0; i < Constants.maxCardAmount; i++)
                {
                    if (user.GetCard(i) != null)
                    {
                        if (user.GetCard(i) != this && user.GetHandCard(i).gameObject.activeInHierarchy == false &&
                            user.GetCard(i).GetCardType() == CardTypes.Nullification)
                        {
                            cardList[discardedCount] = user.GetCard(i);
                            discardedCardList[discardedCount] = i;
                            discardedCount++;
                            bugCatcher = false;
                        }
                    }
                }

                // Interface script setup
                if (!bugCatcher) {
                    user.Interfacing(cardList, this);
                }
            }
        }
    }

    public override void Effect(Player user, Player enemy, int priority)
    {
        switch (priority) {
            case 4:
                Nullify(enemy);
                break;

            case 8:
                if (nullType == 1) { Protect(protection, user); }
                break;

            case 16:
                if (nullType == 0 && wronged == false) { CauseDamage(damage, enemy); }
                break;

            case 19:
                // Setup discarded list for enemy
                if (nullType == 2) { 
                    if (!bugCatcher) {
                        user.RestoreCard(discardedCardList[interfaceSignal]);
                        Debug.Log("Discarded: " + discardedCardList[interfaceSignal]);
                        bugCatcher = true;
                    }
                }
                break;
        }
    }
}

public class Catastrophe : Card, Interfacer
{
    public Sprite[] interfaceList { get; set; }
    public Card[] cardList { get; set; }
    public int interfaceSignal { get; set; }

    int[] ultimateList = new int[3]; // Lists the deckList indexes of ultimate cards
    bool bugCatcher = true;

    public Catastrophe(HeroEnum hero, string name, int cardID, Sprite image, string text, CardTypes type, int minmax) :
        base(hero, name, cardID, image, text, type, minmax)
    { }

    public void SetSignal(int interfaceSignal)
    {
        this.interfaceSignal = interfaceSignal;
    }

    public void Interfacing(Player user, Player enemy)
    {
        cardList = new Card[Constants.maxCardAmount];
        int ultimateCount = 0;

        // Run through ENEMY Deck List, check if there's an ultimate card
        for (int i = 0; i < Constants.maxHandSize; i++)
        {
            if (enemy.GetCard(i) != null)
            {
                if (enemy.GetCard(i) != this && enemy.GetHandCard(i).gameObject.activeInHierarchy && enemy.GetCard(i).GetCardType() == CardTypes.Ultimate)
                {
                    cardList[ultimateCount] = enemy.GetCard(i);
                    ultimateList[ultimateCount] = i;
                    ultimateCount++;
                    bugCatcher = false;
                }
            }
        }

        // Interface script setup
        if (bugCatcher == false && ultimateCount > 1)
        {
            user.Interfacing(cardList, this);
        } else {
            interfaceSignal = 0;
            // CHECK LATER
            //GameOverseer.GO.interfaceSignalSent = 0;
        }
    }

    public override void Effect(Player user, Player enemy, int priority)
    {
        switch (priority)
        {
            case 10:
                enemy.RaiseCharge(-2);
                break;
            case 18:
                if (!bugCatcher)
                {
                    enemy.DiscardCard(ultimateList[interfaceSignal]);
                    bugCatcher = true;
                }
                this.RaiseCost(1);
                break;
        }
    }
}

public class PerverseEngineering : Card, Interfacer
{
    public Sprite[] interfaceList { get; set; }
    public Card[] cardList { get; set; }
    public int interfaceSignal { get; set; }

    int[] ultimateList = new int[7]; // Lists the deckList indexes of relevant cards
    bool bugCatcher = true;

    public PerverseEngineering(HeroEnum hero, string name, int cardID, Sprite image, string text, CardTypes type, int minmax) :
        base(hero, name, cardID, image, text, type, minmax)
    { }

    public void SetSignal(int interfaceSignal)
    {
        this.interfaceSignal = interfaceSignal;
    }

    public void Interfacing(Player user, Player enemy)
    {
        cardList = new Card[Constants.maxCardAmount];
        int ultimateCount = 0;

        // Run through ENEMY Deck List, check if there's a skill or nullification card
        for (int i = 0; i < Constants.maxCardAmount; i++)
        {
            if (enemy.GetCard(i) != null)
            {
                if (enemy.GetCard(i) != this && enemy.GetHandCard(i).gameObject.activeInHierarchy == true &&
                    (enemy.GetCard(i).GetCardType() == CardTypes.Nullification || enemy.GetCard(i).GetCardType() == CardTypes.Skill))
                {
                    cardList[ultimateCount] = enemy.GetCard(i);
                    ultimateList[ultimateCount] = i;
                    ultimateCount++;
                    bugCatcher = false;
                }
            }
        }

        // Interface script setup
        if (bugCatcher == false && ultimateCount > 1)
        {
            user.Interfacing(cardList, this);
        } else {
            interfaceSignal = 0;
            // CHECK LATER
            //GameOverseer.GO.interfaceSignalSent = 0;
        }
    }

    public override void Effect(Player user, Player enemy, int priority)
    {
        switch (priority)
        {
            case 18:
                if (!bugCatcher)
                {
                    enemy.DiscardCard(ultimateList[interfaceSignal]);
                    bugCatcher = true;
                }
                break;
        }
    }
}

public class TemporalShieldTwo : Card
{
    public TemporalShieldTwo(HeroEnum hero, string name, int cardID, Sprite image, string text, CardTypes type, int minmax) :
        base(hero, name, cardID, image, text, type, minmax)
    { }

    public override void Effect(Player user, Player enemy, int priority)
    {
        user.Protect(1);
    }
}

public class CloningMachine : Card
{
    public CloningMachine(HeroEnum hero, string name, int cardID, Sprite image, string text, CardTypes type, int minmax) :
        base(hero, name, cardID, image, text, type, minmax)
    { }

    public override void Effect(Player user, Player enemy, int priority)
    {
        enemy.GetCardPlayed().Effect(user, enemy, priority);
    }
}