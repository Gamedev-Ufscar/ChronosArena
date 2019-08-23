using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Card
{
    public HeroEnum hero = HeroEnum.Timothy;
    public string name = "";
    public int id;
    public Sprite image;
    public string text = "";
    public int turnsTillPlayable = 0;
    public bool isNullified = false;
    public CardTypes type;
    public int minmax = 0000;

    // ULTIMATE AND NEUTRAL SKILL ONLY
    public bool isReaction = false;
    public int cost = 200;

    public abstract void effect(PlayerManager user, PlayerManager enemy, int priority);

    // This setup is for choices - use the same card, only change text
    public void interfacingSetup(int cardAmount, Sprite[] interfaceList, Card baseCard, string[] textList)
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
    }

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

}

public interface Damage
{
    int damage { get; set; }
    bool isUnblockable { get; set; }

    void causeDamage(int damage, PlayerManager target);
    
}

public interface Limit
{
    int limit { get; set; }
    int limitMax { get; set; }

    void raiseLimit(int amount, PlayerManager target);
    void disableCards(List<CardTypes> disables, Card[] playerHand);

}

public interface ChargeInterface
{
    int charge { get; set; }
    void raiseCharge(int charge, PlayerManager target);
}

public interface Protection
{
    int protection { get; set; }

    void protect(int protection, PlayerManager target);

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