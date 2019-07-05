using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Card
{
    public string name;
    public int id;
    public Sprite image;
    public bool isPlayable = true;
    public CardTypes type;

    public abstract void effect();

    [HideInInspector]
    public PlayerManager hero;

}

public interface Damage
{
    int damage { get; set; }
    PlayerManager target { get; set; }

    void causeDamage(int damage, PlayerManager target);
    
}

public interface Limit
{
    int limit { get; set; }
    int limitMax { get; set; }

    void raiseLimit(int amount, PlayerManager target);
    void disableCards(List<CardTypes> disables, List<Card> playerHand);

}

public interface ChargeInterface
{
    int charge { get; set; }
    void raiseCharge(int charge, PlayerManager target);
}

public interface Protection
{
    int protection { get; set; }

    void protect(int protection);

}

public interface Skill
{
    void discard();

}

public interface NullInterface
{
    int[] nullificationList { get; set; }
}