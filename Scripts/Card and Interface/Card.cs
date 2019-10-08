using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Card
{
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

    public Card (HeroEnum hero, string name, int cardID, Sprite image, string text, CardTypes type, int minmax)
    {
        this.hero = hero;
        this.name = name;
        this.id = cardID;
        this.image = image;
        this.text = text;
        this.type = type;
        this.minmax = minmax;
    }

    public Card(HeroEnum hero, string name, int cardID, Sprite image, string text, CardTypes type, int minmax, bool isReaction, int cost)
    {
        this.hero = hero;
        this.name = name;
        this.id = cardID;
        this.image = image;
        this.text = text;
        this.type = type;
        this.minmax = minmax;
        this.isReaction = isReaction;
        this.cost = cost;
    }

    public string Value (int value)
    {
        switch (type)
        {
            case CardTypes.Attack:
                if (value == 1) {
                    Damage damageCardd = (Damage)this;
                    return "" + damageCardd.damage;
                } else {
                    Limit limitCardd = (Limit)this;
                    return "" + limitCardd.limitMax;
                }

            case CardTypes.Defense:
                if (value == 1)
                {
                    Defense defenseCardd = (Defense)this;
                    return "" + defenseCardd.protection;
                } else
                {
                    return "";
                }

            case CardTypes.Charge:
                if (value == 1) {
                    ChargeInterface chargeCardd = (ChargeInterface)this;
                    return "" + chargeCardd.charge;
                } else {
                    Limit limitCardd = (Limit)this;
                    return "" + limitCardd.limitMax;
                }

            case CardTypes.Ultimate:
                if (value == 2)
                {
                    return "" + cost;
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

    public int GetID()
    {
        return id;
    }

    public Sprite GetImage()
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
    public void RaiseCost(int raise)
    {
        cost += raise;
    }

    public void SetIsNullified(bool isNullified)
    {
        this.isNullified = isNullified;
    }

    public void SetTurnsTill(int turnsTillPlayable)
    {
        this.turnsTillPlayable = turnsTillPlayable;
    }

    public void ReduceTurnsTill()
    {
        this.turnsTillPlayable--;
    }

    public abstract void Effect(Player user, Player enemy, int priority);

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

    void SetIsUnblockable(bool isUnblockable);

    void CauseDamage(int damage, Player target);
}

public interface Limit
{
    int limit { get; set; }
    int limitMax { get; set; }

    void RaiseLimit(int amount, Player target);
    void DisableCards(Player target);

}

public interface ChargeInterface
{
    int charge { get; set; }
    void RaiseCharge(int charge, Player target);
}

public interface Protection
{
    int protection { get; set; }

    void Protect(int protection, Player target);

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

    void SetSignal(int interfaceSignal);

    void Interfacing(Player user, Player enemy);
}