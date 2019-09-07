using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Card
{
    Hashtable heroHashtable = new Hashtable();
    static CardTypes[] defaultAttackDisableList = new CardTypes[5] { CardTypes.Attack, CardTypes.Nullification, CardTypes.Skill, CardTypes.Ultimate, CardTypes.Item };

    private HeroEnum hero = HeroEnum.Timothy;
    private string name = "";
    private int id;
    private Sprite image;
    private string text = "";
    private int turnsTillPlayable = 0;
    private bool isNullified = false;
    private CardTypes type;
    private int minmax = 0000;

    // ULTIMATE AND NEUTRAL SKILL ONLY
    private bool isReaction = false;
    private int cost = 200;

    public Card(HeroEnum hero, int cardID)
    {
        // Declaration
        Hashtable heroHT = new Hashtable();
        Hashtable timothyHT = new Hashtable();
        Hashtable haroldHT = new Hashtable();
        Hashtable ugaHT = new Hashtable();
        Hashtable yuriHT = new Hashtable();

        // Hero HT
        heroHashtable.Add(HeroEnum.Timothy, timothyHT);
        heroHashtable.Add(HeroEnum.Harold, haroldHT);
        heroHashtable.Add(HeroEnum.Uga, ugaHT);
        heroHashtable.Add(HeroEnum.Yuri, yuriHT);

        // Timothy
        timothyHT.Add(0, new Attack(hero, name, cardID, image, text, type, minmax, 2, false, 2));
        timothyHT.Add(1, new Defense(hero, name, cardID, image, text, type, minmax, 2));
        timothyHT.Add(2, new WatchAdjustments(hero, name, cardID, image, text, type, minmax));
        timothyHT.Add(3, new Nullification(hero, name, cardID, image, text, type, minmax, defaultAttackDisableList));
        timothyHT.Add(4, new Nullification(hero, name, cardID, image, text, type, minmax, defaultAttackDisableList));
        timothyHT.Add(5, new TimeLock(hero, name, cardID, image, text, type, minmax));
        timothyHT.Add(6, new TimeLock(hero, name, cardID, image, text, type, minmax));
        timothyHT.Add(7, new DejaVu(hero, name, cardID, image, text, type, minmax));
        timothyHT.Add(8, new ChronosMachine(hero, name, cardID, image, text, type, minmax, true));
        timothyHT.Add(9, new ChronosMachine(hero, name, cardID, image, text, type, minmax, false));

        // Uga
        ugaHT.Add(0, new Attack(hero, name, cardID, image, text, type, minmax, 2, false, 2));
        ugaHT.Add(1, new Defense(hero, name, cardID, image, text, type, minmax, 2));
        ugaHT.Add(2, new Charge(hero, name, cardID, image, text, type, minmax, 1, 3));
        ugaHT.Add(3, new Nullification(hero, name, cardID, image, text, type, minmax, defaultAttackDisableList));
        ugaHT.Add(4, new Nullification(hero, name, cardID, image, text, type, minmax, defaultAttackDisableList));
        ugaHT.Add(5, new BasicSkill(hero, name, cardID, image, text, type, minmax, 3, false, 0, 0));
        ugaHT.Add(6, new AutoHealSkill(hero, name, cardID, image, text, type, minmax, 2, false));
        ugaHT.Add(7, new BugaScream(hero, name, cardID, image, text, type, minmax));

        // Yuri
        yuriHT.Add(0, new Attack(hero, name, cardID, image, text, type, minmax, 3, false, 1));
        yuriHT.Add(1, new Defense(hero, name, cardID, image, text, type, minmax, 1));
        yuriHT.Add(2, new Charge(hero, name, cardID, image, text, type, minmax, 1, 3));
        yuriHT.Add(3, new Nullification(hero, name, cardID, image, text, type, minmax, defaultAttackDisableList));
        yuriHT.Add(4, new Attack(hero, name, cardID, image, text, type, minmax, 4, false, 1));
        yuriHT.Add(5, new BasicSkill(hero, name, cardID, image, text, type, minmax, 3, false, 0, 0));
        yuriHT.Add(6, new AutoHealSkill(hero, name, cardID, image, text, type, minmax, 2, false));
        yuriHT.Add(7, new Dexterity(hero, name, cardID, image, text, type, minmax));

    }

    public Card (HeroEnum hero, string name, int cardID, Sprite image, string text, CardTypes type, int minmax)
    {
        this.hero = hero;
        this.name = name;
        this.text = text;
        this.type = type;
        this.minmax = minmax;
    }

    public string value (Card cardd, int value)
    {
        switch (cardd.type)
        {
            case CardTypes.Attack:
                if (value == 1) {
                    Damage damageCardd = (Damage)cardd;
                    return "" + damageCardd.damage;
                } else {
                    Limit limitCardd = (Limit)cardd;
                    return "" + limitCardd.limitMax;
                }

            case CardTypes.Defense:
                if (value == 1)
                {
                    Defense defenseCardd = (Defense)cardd;
                    return "" + defenseCardd.protection;
                } else
                {
                    return "";
                }

            case CardTypes.Charge:
                if (value == 1) {
                    ChargeInterface chargeCardd = (ChargeInterface)cardd;
                    return "" + chargeCardd.charge;
                } else {
                    Limit limitCardd = (Limit)cardd;
                    return "" + limitCardd.limitMax;
                }

            case CardTypes.Ultimate:
                if (value == 2)
                {
                    return "" + cardd.cost;
                } else
                {
                    return "";
                }

            default:
                return "";
        }
    }

    // Getters
    public HeroEnum GetHero()
    {
        return hero;
    }

    public string GetName()
    {
        return name;
    }

    public Sprite Image()
    {
        return image;
    }

    public string GetText()
    {
        return text;
    }

    public CardTypes GetCardType()
    {
        return type;
    }

    public int GetTurnsTill()
    {
        return turnsTillPlayable;
    }

    public int GetMinOrMax(bool wantMin)
    {
        if (wantMin)
        {
            return minmax % 100;
        } else
        {
            return minmax / 100;
        }
    }

    public bool GetIsNullified()
    {
        return isNullified;
    }

    public bool GetIsReaction()
    {
        return isReaction;
    }

    public int GetCost()
    {
        return cost;
    }

    // Setters
    public void SetIsNullified(bool isNullified)
    {
        this.isNullified = isNullified;
    }

    public void SetTurnsTill(int turnsTillPlayable)
    {
        this.turnsTillPlayable = turnsTillPlayable;
    }

    public abstract void effect(Player user, Player enemy, int priority);

    // This setup is for choices - use the same card, only change text
    /*public void interfacingSetup(int cardAmount, Sprite[] interfaceList, Card baseCard, string[] textList)
    {
        HeroDecks.HD.interfaceScript.cardAmount = cardAmount;
        HeroDecks.HD.interfaceScript.interfaceList = interfaceList;
        HeroDecks.HD.interfaceScript.textList = textList;
        HeroDecks.HD.interfaceScript.baseCard = baseCard;
        HeroDecks.HD.interfaceScript.invoker = this;
        HeroDecks.HD.interfaceScript.optionMenu = true;
        HeroDecks.HD.interfaceScript.gameObject.SetActive(true);
    }

    // This setup is for cards - choose a card to draw, discard, etc.
    public void interfacingSetup(int cardAmount, Sprite[] interfaceList, Card[] cardList)
    {
        HeroDecks.HD.interfaceScript.cardAmount = cardAmount;
        HeroDecks.HD.interfaceScript.interfaceList = interfaceList;
        HeroDecks.HD.interfaceScript.cardList = cardList;
        HeroDecks.HD.interfaceScript.invoker = this;
        HeroDecks.HD.interfaceScript.optionMenu = true;
        HeroDecks.HD.interfaceScript.gameObject.SetActive(true);
    }*/

    public string typeString (CardTypes typer)
    {
        switch (typer)
        {
            case CardTypes.Attack:
                return "ATTACK";

            case CardTypes.Defense:
                return "DEFENSE";

            case CardTypes.Charge:
                return "CHARGE";

            case CardTypes.Nullification:
                return "NULLIFICATION";

            case CardTypes.Skill:
                return "SKILL";

            case CardTypes.Ultimate:
                return "ULTIMATE";

            case CardTypes.Item:
                return "ITEM";

            case CardTypes.Structure:
                return "STRUCTURE";

            default:
                return "PASSIVE";
        }
    }

    public string heroString(HeroEnum heror)
    {
        switch (heror)
        {
            case HeroEnum.Timothy:
                return "TIMOTHY";

            case HeroEnum.Harold:
                return "DR. HAROLD";

            case HeroEnum.Uga:
                return "UGA";

            case HeroEnum.Yuri:
                return "YURI";

            case HeroEnum.Zarnada:
                return "ZARNADA";

            case HeroEnum.Tupa:
                return "TUPÃ";

            case HeroEnum.Gerador:
                return "MECHA-GERADOR";

            case HeroEnum.Eugene:
                return "EUGENE";

            default:
                return "ROBOTO";
        }
    }

    // Hashtables
    
}

public interface Damage
{
    int damage { get; set; }
    bool isUnblockable { get; set; }

    void causeDamage(int damage, Player target);
}

public interface Limit
{
    int limit { get; set; }
    int limitMax { get; set; }

    void raiseLimit(int amount, Player target);
    void disableCards(List<CardTypes> disables, Card[] playerHand);

}

public interface ChargeInterface
{
    int charge { get; set; }
    void raiseCharge(int charge, Player target);
}

public interface Protection
{
    int protection { get; set; }

    void protect(int protection, Player target);

}

public interface NullInterface
{
    CardTypes[] nullificationList { get; set; }
    bool wronged { get; set; }
}

public interface Interfacer
{
    Sprite[] interfaceList { get; set; }
    int interfaceSignal { get; set; }

    void interfacing();
}