using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : Card, Damage, Limit
{

    public int damage { get; set; }
    public PlayerManager target { get; set; }

    public void causeDamage(int damage, PlayerManager target) {
        if (damage - target.protection >= 0)
        {
            target.HP -= (damage - target.protection);
        }
    }

    public int limit { get; set; }
    public int limitMax { get; set; }

    public void raiseLimit(int amount, PlayerManager target) {
        foreach(Attack c in target.cardList)
        {
            c.limit++;
            if (c.limit >= c.limitMax)
            {
                disableCards(target.attackDisableList, target.cardList);
            }
        }
    }

    public void disableCards(List<int> disables, List<Card> playerHand) {
        foreach (Card c in playerHand) {
            foreach (int d in disables)
            {
                if (c.type == d)
                {
                    c.isPlayable = false;
                }
            }
        }
    }

    public override void effect ()
    {
        causeDamage(damage, target);
        raiseLimit(1, target);
    }
}

public class Defense : Card, Protection
{
    public int protection { get; set; }

    public void protect(int protection) {
        hero.protection = this.protection;
    }

    public override void effect()
    {
        protect(protection);
    }
}

public class Charge : Card, ChargeInterface, Limit
{

    public int charge { get; set; }
    public PlayerManager target { get; set; }
    public void raiseCharge(int charge, PlayerManager target) {
        target.Charge += charge;
    }

    public int limit { get; set; }
    public int limitMax { get; set; }

    public void raiseLimit(int amount, PlayerManager target)
    {
        foreach (Charge c in target.cardList)
        {
            c.limit++;
            if (c.limit >= c.limitMax)
            {
                disableCards(target.chargeDisableList, target.cardList);
            }
        }
    }

    public void disableCards(List<int> disables, List<Card> playerHand)
    {
        foreach (Card c in playerHand)
        {
            foreach (int d in disables)
            {
                if (c.type == d)
                {
                    c.isPlayable = false;
                }
            }
        }
    }

    public override void effect()
    {
        raiseCharge(charge, target);
        raiseLimit(1, target);
    }
}

public class Nullification : Card, NullInterface
{
    public int[] nullificationList { get; set; }

    public override void effect()
    {
        
    }
}
