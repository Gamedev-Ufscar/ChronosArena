using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugaScream : Card
{
    public override void effect(PlayerManager user, PlayerManager enemy, int priority)
    {
        switch (priority) {
            case 16:
                if (1 - enemy.protection >= 0) {
                    enemy.HP -= (1 - enemy.protection);
                }
                Debug.Log(user.gameObject.name + "'s Buga Scream");
                break;

            case 17:
                if (user.cardList[0] != null) {
                    Attack cc = (Attack)user.cardList[0];
                    cc.damage++;
                    user.cardList[0] = (Card)cc;
                }
                cost++;

                break;
        }
    }
}

public class WatchAdjustments : Card, Interfacer
{
    public Sprite[] interfaceList { get; set; }
    public int interfaceSignal { get; set; }


    // Two choices
    public void interfacing()
    {
        interfaceList = new Sprite[2];
        interfaceList[0] = HeroDecks.HD.imageList[0];
        interfaceList[1] = HeroDecks.HD.imageList[0];

        HeroDecks.HD.interfaceScript.cardAmount = 2;
        HeroDecks.HD.interfaceScript.interfaceList = interfaceList;
        HeroDecks.HD.interfaceScript.invoker = this;
        Debug.Log("Interface script setup");
        HeroDecks.HD.interfaceScript.gameObject.SetActive(true);
    }

    public override void effect(PlayerManager user, PlayerManager enemy, int priority)
    {
        switch (priority)
        {
            case 14:
                if (interfaceSignal == 0) {
                    user.Charge++;
                } else if (interfaceSignal == 1) {
                    user.Charge += 2;
                    user.HP -= 2;
                }
                break;
        }
    }
}

public class TimeLock : Card
{

    public override void effect(PlayerManager user, PlayerManager enemy, int priority)
    {
        switch (priority)
        {

            case 00:
                user.sideList[0] = user.HP * 100 + enemy.HP;
                Debug.Log(user.gameObject.name + "'s Time Lock");
                break;

            case 14:
                user.Charge++;
                break;

        }
    }
}

public class DejaVu : Card
{ 
    public override void effect(PlayerManager user, PlayerManager enemy, int priority)
    {
        switch (priority)
        {
            case 8:
                user.protection = 2;
                break;
            case 17:
                user.sideList[1] = 2;
                break;
        }
    }
}

public class ChronosMachine : Card, Interfacer
{
    public Sprite[] interfaceList { get; set; }
    public int interfaceSignal { get; set; }
    public bool isChronos { get; set; }


    // Two choices
    public void interfacing()
    {
        if (!isChronos)
        {
            interfaceList = new Sprite[2];
            interfaceList[0] = HeroDecks.HD.imageList[0];
            interfaceList[1] = HeroDecks.HD.imageList[0];

            HeroDecks.HD.interfaceScript.cardAmount = 2;
            HeroDecks.HD.interfaceScript.interfaceList = interfaceList;
            HeroDecks.HD.interfaceScript.invoker = this;
            Debug.Log("Interface script setup");
            HeroDecks.HD.interfaceScript.gameObject.SetActive(true);
        } else
        {
            interfaceSignal = 0;
        }
    }


    public override void effect(PlayerManager user, PlayerManager enemy, int priority)
    {
        switch (priority)
        {
            case 12:
                if (user.sideList[0] != 0) {
                    if (interfaceSignal == 0 || isChronos) {
                        user.HP = user.sideList[0] / 100;
                    }
                    if (interfaceSignal == 1 || isChronos) {
                        enemy.HP = user.sideList[0] % 100;
                    }
                    Debug.Log("Chronos Machine!");
                }
                if (isChronos)
                    this.cost++;
                
                break;
        }
    }
}