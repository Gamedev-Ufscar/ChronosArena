using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Card
{
    public string name;
    public int id;
    public Sprite image;
    public int turnsTillPlayable = 0;
    public bool isNullified = false;
    public CardTypes type;
    public int minmax = 0000;

    // ULTIMATE AND NEUTRAL SKILL ONLY
    public int cost = -1;

    public abstract void effect(PlayerManager user, PlayerManager enemy, int priority);

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