using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : Card, Damage, Limit
{

    public int damage { get; set; }
    public bool isUnblockable { get; set; }

    public void causeDamage(int damage, PlayerManager target) {
        if (!isUnblockable) {
            if (damage - target.protection >= 0) {
                target.HP -= (damage - target.protection);
            }
        } else {
            target.HP -= damage;
        }
    }

    public int limit { get; set; }
    public int limitMax { get; set; }

    public void raiseLimit(int amount, PlayerManager target) {
        for (int i = 0; i < target.cardList.Length; i++) { 
            if (target.cardList[i] as Attack != null) {
                Attack cc = target.cardList[i] as Attack;
                cc.limit += amount;
                target.cardList[i] = cc as Card;
                Debug.Log(target.gameObject.name + "'s Attack Limit: " + limit);
                if (cc.limit >= cc.limitMax) {
                    disableCards(target.attackDisableList, target.cardList);
                }
            }
        }
    }

    public void disableCards(List<CardTypes> disables, Card[] playerHand) {
        for (int i = 0; i < playerHand.Length; i++) {
            if (playerHand[i] != null) {
                foreach (CardTypes d in disables) {
                    if (playerHand[i].type == d) {
                        playerHand[i].turnsTillPlayable = 1;
                        Debug.Log(playerHand[i].name + " Disabled");
                    }
                }
            }
        }
    }

    public override void effect(PlayerManager user, PlayerManager enemy, int priority)
    {
        switch (priority) {
            case 16:
                causeDamage(damage, enemy);
                raiseLimit(1, user);
                Debug.Log(user.gameObject.name + "'s Attack");
                break;
        }
    }
}


public class Defense : Card, Protection
{
    public int protection { get; set; }

    public void protect(int protection, PlayerManager target)
    {
        target.protection = this.protection;
    }

    public override void effect(PlayerManager user, PlayerManager enemy, int priority)
    {
        switch (priority) {
            case 8:
                protect(protection, user);
                Debug.Log(user.gameObject.name + "'s Defense");
                break;
        }
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
        for (int i = 0; i < target.cardList.Length; i++) { 
            if (target.cardList[i] as Charge != null) {
                Charge cc = target.cardList[i] as Charge;
                cc.limit += amount;
                target.cardList[i] = cc as Card;
                if (cc.limit >= cc.limitMax) {
                    disableCards(target.chargeDisableList, target.cardList);
                }
            }
        }
    }

    public void disableCards(List<CardTypes> disables, Card[] playerHand) {
        for (int i = 0; i < playerHand.Length; i++) {
            if (playerHand[i] != null) {
                foreach (CardTypes d in disables) {
                    if (playerHand[i].type == d) {
                        playerHand[i].turnsTillPlayable = 1;
                        Debug.Log(playerHand[i].name + " Disabled");
                    }
                }
            }
        }
    }

    public override void effect(PlayerManager user, PlayerManager enemy, int priority)
    {
        switch (priority) {
            case 14:
                raiseCharge(charge, user);
                raiseLimit(1, user);
                Debug.Log(user.gameObject.name + "'s Charge");
                break;
        }
    }
}


public class Nullification : Card, NullInterface
{
    public CardTypes[] nullificationList { get; set; }
    public bool wronged { get; set; }

    public void myNullify()
    {
        wronged = true;
        for (int i = 0; i < nullificationList.Length; i++) {
            if (HeroDecks.HD.enemyManager.cardList[GameOverseer.GO.enemyCardPlayed].type == nullificationList[i]) {
                HeroDecks.HD.enemyManager.cardList[GameOverseer.GO.enemyCardPlayed].isNullified = true;
                wronged = false;
            }
        }
    }

    public void enemyNullify()
    {
        wronged = true;
        for (int i = 0; i < nullificationList.Length; i++) {
            if (HeroDecks.HD.myManager.cardList[GameOverseer.GO.myCardPlayed].type == nullificationList[i]) {
                HeroDecks.HD.myManager.cardList[GameOverseer.GO.myCardPlayed].isNullified = true;
                wronged = false;
            }
        }
    }

    public override void effect(PlayerManager user, PlayerManager enemy, int priority)
    {
        switch (priority) {
            case 4:
                if (user == HeroDecks.HD.myManager) {
                    myNullify();
                } else {
                    enemyNullify();
                }
                break;
        }
    }
}


public class BasicSkill : Card, Damage, Protection, ChargeInterface
{
    public int damage { get; set; }
    public bool isUnblockable { get; set; }
    public int protection { get; set; }
    public int charge { get; set; }

    public void causeDamage(int damage, PlayerManager target)
    {
        if (!isUnblockable) {
            if (damage - target.protection >= 0) {
                target.HP -= (damage - target.protection);
            }
        } else {
            target.HP -= damage;
        }
    }

    public void protect(int protection, PlayerManager target)
    {
        target.protection = this.protection;
    }

    public void raiseCharge(int charge, PlayerManager target)
    {
        target.Charge += charge;
    }

    public override void effect(PlayerManager user, PlayerManager enemy, int priority)
    {
        switch (priority)
        {
            case 8:
                protect(protection, user);
                Debug.Log(user.gameObject.name + "'s BasicSkill");
                break;
            case 14:
                raiseCharge(charge, user);
                break;
            case 16:
                causeDamage(damage, enemy);
                break;
        }
    }
}


public class AutoHealSkill : Card, Damage
{
    public int damage { get; set; }
    public bool isUnblockable { get; set; }

     public void causeDamage(int damage, PlayerManager target)
    {
        target.HP -= damage;
    }

    public override void effect(PlayerManager user, PlayerManager enemy, int priority)
    {
        switch (priority)
        {
            case 12:
                causeDamage(damage, user);
                break;
        }
    }
}


public class SideEffectSkill : Card
{
    public int sideEffect { get; set; }
    public int duration { get; set; }

    public override void effect(PlayerManager user, PlayerManager enemy, int priority)
    {
        switch (priority)
        {
            case 17:
                user.sideList[sideEffect] = duration;
                Debug.Log(user.gameObject.name + "'s SideEffectSkill");
                break;
        }
    }
}