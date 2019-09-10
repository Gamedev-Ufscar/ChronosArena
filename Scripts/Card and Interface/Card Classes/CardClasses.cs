using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : Card, Damage, Limit
{
    public int damage { get; set; }
    public bool isUnblockable { get; set; }

    public Attack(HeroEnum hero, string name, int cardID, Sprite image, string text, CardTypes type, int minmax, int damage, bool isUnblockable, int limitMax) : 
        base(hero, name, cardID, image, text, type, minmax)
    {
        this.damage = damage;
        this.isUnblockable = isUnblockable;
        this.limit = 0;
        this.limitMax = limitMax;
    }

    public void causeDamage(int damage, Player target) {
        target.DealDamage(damage, isUnblockable);
    }

    public int limit { get; set; }
    public int limitMax { get; set; }

    public void raiseLimit(int amount, Player target) {
        for (int i = 0; i < 14; i++) { 
            if (target.GetCard(i) as Attack != null) {
                Attack cc = target.GetCard(i) as Attack;
                cc.limit += amount;
                target.SetCard(cc as Card, i);
                Debug.Log(target.gameObject.name + "'s Attack Limit: " + limit);
                if (cc.limit >= cc.limitMax) {
                    disableCards(target);
                }
            }
        }
    }

    public void disableCards(Player target) {
        for (int i = 0; i < 14; i++) {
            if (target.GetCard(i) != null) {
                foreach (CardTypes d in target.GetAttackDisable()) {
                    if (target.GetCard(i).GetCardType() == d) {
                        target.GetCard(i).SetTurnsTill(1);
                        Debug.Log(target.GetCard(i).GetName() + " Disabled");
                    }
                }
            }
        }
    }

    public override void effect(Player user, Player enemy, int priority)
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

    public Defense(HeroEnum hero, string name, int cardID, Sprite image, string text, CardTypes type, int minmax, int protection) :
        base(hero, name, cardID, image, text, type, minmax)
    {
        this.protection = protection;
    }

    public void protect(int protection, Player target)
    {
        target.Protect(this.protection);
    }

    public override void effect(Player user, Player enemy, int priority)
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
    public void raiseCharge(int charge, Player target) {
        target.RaiseCharge(charge);
    }

    public int limit { get; set; }
    public int limitMax { get; set; }

    public Charge(HeroEnum hero, string name, int cardID, Sprite image, string text, CardTypes type, int minmax, int charge, int limitMax) :
        base(hero, name, cardID, image, text, type, minmax)
    {
        this.charge = charge;
        this.limit = 0;
        this.limitMax = limitMax;
    }

    public void raiseLimit(int amount, Player target) {
        for (int i = 0; i < 14; i++) { 
            if (target.GetCard(i) as Charge != null) {
                Charge cc = target.GetCard(i) as Charge;
                cc.limit += amount;
                target.SetCard(cc as Card, i);
                if (cc.limit >= cc.limitMax) {
                    disableCards(target);
                }
            }
        }
    }

    public void disableCards(Player target) {
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

    public override void effect(Player user, Player enemy, int priority)
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

    public Nullification(HeroEnum hero, string name, int cardID, Sprite image, string text, CardTypes type, int minmax, CardTypes[] nullificationList) :
    base(hero, name, cardID, image, text, type, minmax)
    {
        this.nullificationList = nullificationList;
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

    public override void effect(Player user, Player enemy, int priority)
    {
        switch (priority) {
            case 4:
                Nullify(enemy);
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

    public BasicSkill(HeroEnum hero, string name, int cardID, Sprite image, string text, CardTypes type, int minmax, int damage, bool isUnblockable, int protection, int charge) :
    base(hero, name, cardID, image, text, type, minmax)
    {
        this.damage = damage;
        this.isUnblockable = isUnblockable;
        this.protection = protection;
        this.charge = charge;
    }

    public void causeDamage(int damage, Player target)
    {
        target.DealDamage(damage, isUnblockable);
    }

    public void protect(int protection, Player target)
    {
        target.Protect(this.protection);
    }

    public void raiseCharge(int charge, Player target)
    {
        target.RaiseCharge(charge);
    }

    public override void effect(Player user, Player enemy, int priority)
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

    public AutoHealSkill(HeroEnum hero, string name, int cardID, Sprite image, string text, CardTypes type, int minmax, int damage, bool isUnblockable) :
    base(hero, name, cardID, image, text, type, minmax)
    {
        this.damage = damage;
        this.isUnblockable = isUnblockable;
    }

    public void causeDamage(int heal, Player target)
    {
        target.Heal(heal);
    }

    public override void effect(Player user, Player enemy, int priority)
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

    public SideEffectSkill(HeroEnum hero, string name, int cardID, Sprite image, string text, CardTypes type, int minmax, int sideEffect, int duration) :
    base(hero, name, cardID, image, text, type, minmax)
    {
        this.sideEffect = sideEffect;
        this.duration = duration;
    }

    public override void effect(Player user, Player enemy, int priority)
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