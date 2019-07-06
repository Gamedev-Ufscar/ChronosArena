using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : Card, Damage, Limit
{

    public int damage { get; set; }

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

    public void disableCards(List<CardTypes> disables, List<Card> playerHand) {
        foreach (Card c in playerHand) {
            foreach (CardTypes d in disables)
            {
                if (c.type == d)
                {
                    c.isPlayable = false;
                }
            }
        }
    }

    public override void effect (PlayerManager user, PlayerManager enemy)
    {
        causeDamage(damage, enemy);
        raiseLimit(1, user);
    }
}


public class Defense : Card, Protection
{
    public int protection { get; set; }

    public void protect(int protection, PlayerManager target)
    {
        target.protection = this.protection;
    }

    public override void effect(PlayerManager user, PlayerManager enemy)
    {
        protect(protection, user);
    }
}


public class Charge : Card, ChargeInterface, Limit
{

    public int charge { get; set; }
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

    public void disableCards(List<CardTypes> disables, List<Card> playerHand)
    {
        foreach (Card c in playerHand)
        {
            foreach (CardTypes d in disables)
            {
                if (c.type == d)
                {
                    c.isPlayable = false;
                }
            }
        }
    }

    public override void effect(PlayerManager user, PlayerManager enemy)
    {
        raiseCharge(charge, user);
        raiseLimit(1, user);
    }
}


public class Nullification : Card, NullInterface
{
    public CardTypes[] nullificationList { get; set; }

    public void myNullify()
    {
        for (int i = 0; i <= nullificationList.Length; i++)
        {
            if (GameOverseer.GO.enemyCardPlayed.type == nullificationList[i])
            {
                GameOverseer.GO.enemyCardPlayed.isPlayable = false;
            }
        }
    }

    public void enemyNullify()
    {
        for (int i = 0; i <= nullificationList.Length; i++)
        {
            if (GameOverseer.GO.myCardPlayed.type == nullificationList[i])
            {
                GameOverseer.GO.myCardPlayed.isPlayable = false;
            }
        }
    }

    public override void effect(PlayerManager user, PlayerManager enemy)
    {
        if (user == HeroDecks.HD.myManager)
        {
            myNullify();
        } else
        {
            enemyNullify();
        }
    }
}
