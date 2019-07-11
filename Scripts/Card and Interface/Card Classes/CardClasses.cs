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
        foreach(Card c in target.cardList) {
            if (c is Attack) {
                Attack cc = (Attack)c;
                cc.limit += amount;
                Debug.Log(target.gameObject.name + "'s Attack Limit: " + limit);
                if (cc.limit >= cc.limitMax) {
                    disableCards(target.attackDisableList, target.cardList);
                }
            }
        }
    }

    public void disableCards(List<CardTypes> disables, Card[] playerHand) {
        foreach (Card c in playerHand) {
            if (c != null)
            {
                foreach (CardTypes d in disables) {
                    if (c.type == d)
                    {
                        c.turnsTillPlayable = 1;
                        Debug.Log(c.name + " Disabled");
                    }
                }
            }
        }
    }

    public override void effect (PlayerManager user, PlayerManager enemy)
    {
        causeDamage(damage, enemy);
        raiseLimit(1, user);
        Debug.Log(user.gameObject.name + "'s Attack");
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
        Debug.Log(user.gameObject.name + "'s Defense");
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

    public void raiseLimit(int amount, PlayerManager target) {
        foreach (Card c in target.cardList) {
            if (c is Charge) {
                Charge cc = (Charge)c;
                cc.limit += amount;
                if (cc.limit >= cc.limitMax) {
                    disableCards(target.chargeDisableList, target.cardList);
                }
            }
        }
    }

    public void disableCards(List<CardTypes> disables, Card[] playerHand) {
        foreach (Card c in playerHand) {
            if (c != null)
            {
                foreach (CardTypes d in disables)
                {
                    if (c.type == d)
                    {
                        c.turnsTillPlayable = 1;
                    }
                }
            }
        }
    }

    public override void effect(PlayerManager user, PlayerManager enemy)
    {
        raiseCharge(charge, user);
        raiseLimit(1, user);
        Debug.Log(user.gameObject.name + "'s Charge");
    }
}


public class Nullification : Card, NullInterface
{
    public CardTypes[] nullificationList { get; set; }

    public void myNullify()
    {
        for (int i = 0; i < nullificationList.Length; i++) {
            if (HeroDecks.HD.enemyManager.cardList[GameOverseer.GO.enemyCardPlayed].type == nullificationList[i]) {
                HeroDecks.HD.enemyManager.cardList[GameOverseer.GO.enemyCardPlayed].isNullified = true;
            }
        }
    }

    public void enemyNullify()
    {
        for (int i = 0; i < nullificationList.Length; i++) {
            if (HeroDecks.HD.myManager.cardList[GameOverseer.GO.myCardPlayed].type == nullificationList[i]) {
                HeroDecks.HD.myManager.cardList[GameOverseer.GO.myCardPlayed].isNullified = true;
            }
        }
    }

    public override void effect(PlayerManager user, PlayerManager enemy)
    {
        if (user == HeroDecks.HD.myManager) {
            myNullify();
        } else {
            enemyNullify();
        }
        Debug.Log(user.gameObject.name + "'s Nullify");
    }
}


public class DamageSkill : Card, Damage
{
    public int damage { get; set; }

    public void causeDamage(int damage, PlayerManager target)
    {
        if (damage - target.protection >= 0)
        {
            target.HP -= (damage - target.protection);
        }
    }

    public override void effect(PlayerManager user, PlayerManager enemy)
    {
        causeDamage(damage, enemy);
        Debug.Log(user.gameObject.name + "'s DamageSkill");
    }
}
