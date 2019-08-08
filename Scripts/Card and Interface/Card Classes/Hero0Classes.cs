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
                user.protection += 2;
                break;
            case 17:
                user.sideList[2] = 2;
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
                if (isChronos) {
                    if (user.sideList[0] != 0) {
                        user.sideList[1] = user.HP * 100 + enemy.HP;

                        user.HP = user.sideList[0] / 100;
                        enemy.HP = user.sideList[0] % 100;
                        this.cost++;
                        Debug.Log("Chronos Machine!");
                    }
                } else {
                    if (user.sideList[1] != 0) {
                        if (interfaceSignal == 0) {
                            user.HP = user.sideList[1] / 100;
                        }
                        if (interfaceSignal == 1) {
                            enemy.HP = user.sideList[1] % 100;
                        }
                    }
                }
                
                break;
        }
    }
}

public class Sabotage : Card, Damage, Protection, NullInterface, Interfacer
{
    public CardTypes[] nullificationList { get; set; }
    public bool wronged { get; set; }
    public int damage { get; set; }
    public bool isUnblockable { get; set; }
    public int protection { get; set; }
    public Sprite[] interfaceList { get; set; }
    public int interfaceSignal { get; set; }

    int[] discardedCardList = new int[10]; // Lists the deckList indexes of discarded cards
    bool bugCatcher = true;

    public int nullType = 0;

    public void causeDamage(int damage, PlayerManager target)
    {
        if (damage - target.protection >= 0)
        {
            target.HP -= (damage - target.protection);
        }
    }

    public void protect(int protection, PlayerManager target)
    {
        target.protection += this.protection;
    }

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

    public void interfacing()
    {
        interfaceList = new Sprite[HeroDecks.HD.myManager.cardList.Length];
        int discardedCount = 0;


        if (nullType == 2)
        {
            // First check if the Nullification is valid - if yes, proceed
            wronged = true;
            for (int i = 0; i < nullificationList.Length; i++) {
                if (HeroDecks.HD.enemyManager.cardList[GameOverseer.GO.enemyCardPlayed].type == nullificationList[i]) {
                    wronged = false;
                }
            }

            if (!wronged)
            {

                // Run through Deck List, check if there's a disabled nullification card
                for (int i = 0; i < HeroDecks.HD.myManager.myHand.GetComponent<DeckManager>().deckList.Length; i++)
                {
                    if (HeroDecks.HD.myManager.cardList[i] != null && HeroDecks.HD.myManager.myHand.GetComponent<DeckManager>().deckList[i] != null)
                    {
                        if (HeroDecks.HD.myManager.cardList[HeroDecks.HD.myManager.myHand.GetComponent<DeckManager>().deckList[i].GetComponent<CardInHand>().thisCard] != this &&
                            HeroDecks.HD.myManager.myHand.GetComponent<DeckManager>().deckList[i].activeInHierarchy == false &&
                            HeroDecks.HD.myManager.cardList[HeroDecks.HD.myManager.myHand.GetComponent<DeckManager>().deckList[i].GetComponent<CardInHand>().thisCard].type == CardTypes.Nullification)
                        {
                            interfaceList[discardedCount] = HeroDecks.HD.myManager.cardList[HeroDecks.HD.myManager.myHand.GetComponent<DeckManager>().deckList[i].GetComponent<CardInHand>().thisCard].image;
                            discardedCardList[discardedCount] = i;
                            discardedCount++;
                            bugCatcher = false;
                        }
                    }
                }

                // Interface script setup
                if (!bugCatcher) {
                    HeroDecks.HD.interfaceScript.cardAmount = discardedCount;
                    HeroDecks.HD.interfaceScript.interfaceList = interfaceList;
                    HeroDecks.HD.interfaceScript.invoker = this;
                    HeroDecks.HD.interfaceScript.gameObject.SetActive(true);
                }
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

            case 8:
                if (nullType == 1) { protect(protection, user); }
                break;

            case 16:
                if (nullType == 0 && wronged == false) { causeDamage(damage, enemy); }
                break;

            case 19:
                // Setup discarded list for enemy
                if (nullType == 2) { 
                    if (user == HeroDecks.HD.enemyManager) {
                        int discardedCount = 0;
                        for (int i = 0; i < HeroDecks.HD.enemyManager.myHand.GetComponent<DeckManager>().deckList.Length; i++) {
                            if (HeroDecks.HD.enemyManager.cardList[i] != null && HeroDecks.HD.enemyManager.myHand.GetComponent<DeckManager>().deckList[i] != null) {
                                if (HeroDecks.HD.enemyManager.cardList[HeroDecks.HD.enemyManager.myHand.GetComponent<DeckManager>().deckList[i].GetComponent<EnemyCardInHand>().thisCard] != this &&
                                    HeroDecks.HD.enemyManager.myHand.GetComponent<DeckManager>().deckList[i].activeInHierarchy == false &&
                                    HeroDecks.HD.enemyManager.cardList[HeroDecks.HD.enemyManager.myHand.GetComponent<DeckManager>().deckList[i].GetComponent<EnemyCardInHand>().thisCard].type == CardTypes.Nullification) {
                                    discardedCardList[discardedCount] = i;
                                    discardedCount++;
                                    bugCatcher = false;
                                }
                            }
                        }
                    }

                    if (!bugCatcher) {
                        user.RestoreCard(discardedCardList[interfaceSignal]);
                        Debug.Log("Discarded: " + discardedCardList[interfaceSignal]);
                        bugCatcher = true;
                    }
                }
                break;
        }
    }
}

public class Catastrophe : Card, Interfacer
{
    public Sprite[] interfaceList { get; set; }
    public int interfaceSignal { get; set; }

    int[] ultimateList = new int[3]; // Lists the deckList indexes of ultimate cards
    bool bugCatcher = true;

    public void interfacing()
    {
        interfaceList = new Sprite[HeroDecks.HD.enemyManager.cardList.Length];
        int ultimateCount = 0;

        // Run through ENEMY Deck List, check if there's an ultimate card
        for (int i = 0; i < HeroDecks.HD.enemyManager.myHand.GetComponent<DeckManager>().deckList.Length; i++)
        {
            if (HeroDecks.HD.enemyManager.cardList[i] != null && HeroDecks.HD.enemyManager.myHand.GetComponent<DeckManager>().deckList[i] != null)
            {
                if (HeroDecks.HD.enemyManager.cardList[HeroDecks.HD.enemyManager.myHand.GetComponent<DeckManager>().deckList[i].GetComponent<EnemyCardInHand>().thisCard] != this &&
                    HeroDecks.HD.enemyManager.myHand.GetComponent<DeckManager>().deckList[i].activeInHierarchy == true &&
                    HeroDecks.HD.enemyManager.cardList[HeroDecks.HD.enemyManager.myHand.GetComponent<DeckManager>().deckList[i].GetComponent<EnemyCardInHand>().thisCard].type == CardTypes.Ultimate)
                {
                    interfaceList[ultimateCount] = HeroDecks.HD.enemyManager.cardList[HeroDecks.HD.enemyManager.myHand.GetComponent<DeckManager>().deckList[i].GetComponent<EnemyCardInHand>().thisCard].image;
                    ultimateList[ultimateCount] = i;
                    ultimateCount++;
                    bugCatcher = false;
                }
            }
        }

        // Interface script setup
        if (bugCatcher == false && ultimateCount > 1)
        {
            HeroDecks.HD.interfaceScript.cardAmount = ultimateCount;
            HeroDecks.HD.interfaceScript.interfaceList = interfaceList;
            HeroDecks.HD.interfaceScript.invoker = this;
            HeroDecks.HD.interfaceScript.gameObject.SetActive(true);
        } else {
            interfaceSignal = 0;
            GameOverseer.GO.interfaceSignalSent = 0;
        }
    }

    public override void effect(PlayerManager user, PlayerManager enemy, int priority)
    {
        switch (priority)
        {
            case 10:
                enemy.Charge -= 2;
                if (enemy.Charge < 0) { enemy.Charge = 10; }
                break;
            case 18:
                // Setup ultimate list for enemy
                if (user == HeroDecks.HD.enemyManager)
                {
                    int ultimateCount = 0;
                    for (int i = 0; i < HeroDecks.HD.myManager.myHand.GetComponent<DeckManager>().deckList.Length; i++)
                    {
                        if (HeroDecks.HD.myManager.cardList[i] != null && HeroDecks.HD.myManager.myHand.GetComponent<DeckManager>().deckList[i] != null) {
                            if (HeroDecks.HD.myManager.cardList[HeroDecks.HD.myManager.myHand.GetComponent<DeckManager>().deckList[i].GetComponent<CardInHand>().thisCard] != this &&
                                HeroDecks.HD.myManager.myHand.GetComponent<DeckManager>().deckList[i].activeInHierarchy == true &&
                                HeroDecks.HD.myManager.cardList[HeroDecks.HD.myManager.myHand.GetComponent<DeckManager>().deckList[i].GetComponent<CardInHand>().thisCard].type == CardTypes.Ultimate)
                            {
                                ultimateList[ultimateCount] = i;
                                ultimateCount++;
                                bugCatcher = false;
                            }
                        }
                    }
                }

                if (!bugCatcher)
                {
                    enemy.DiscardCard(ultimateList[interfaceSignal]);
                    bugCatcher = true;
                }
                this.cost++;
                break;
        }
    }
}

public class PerverseEngineering : Card, Interfacer
{
    public Sprite[] interfaceList { get; set; }
    public int interfaceSignal { get; set; }

    int[] ultimateList = new int[7]; // Lists the deckList indexes of relevant cards
    bool bugCatcher = true;

    public void interfacing()
    {
        interfaceList = new Sprite[HeroDecks.HD.enemyManager.cardList.Length];
        int ultimateCount = 0;

        // Run through ENEMY Deck List, check if there's a skill or nullification card
        for (int i = 0; i < HeroDecks.HD.enemyManager.myHand.GetComponent<DeckManager>().deckList.Length; i++)
        {
            if (HeroDecks.HD.enemyManager.cardList[i] != null && HeroDecks.HD.enemyManager.myHand.GetComponent<DeckManager>().deckList[i] != null)
            {
                if (HeroDecks.HD.enemyManager.cardList[HeroDecks.HD.enemyManager.myHand.GetComponent<DeckManager>().deckList[i].GetComponent<EnemyCardInHand>().thisCard] != this &&
                    HeroDecks.HD.enemyManager.myHand.GetComponent<DeckManager>().deckList[i].activeInHierarchy == true &&
                    (HeroDecks.HD.enemyManager.cardList[HeroDecks.HD.enemyManager.myHand.GetComponent<DeckManager>().deckList[i].GetComponent<EnemyCardInHand>().thisCard].type == CardTypes.Nullification ||
                    HeroDecks.HD.enemyManager.cardList[HeroDecks.HD.enemyManager.myHand.GetComponent<DeckManager>().deckList[i].GetComponent<EnemyCardInHand>().thisCard].type == CardTypes.Skill))
                {
                    interfaceList[ultimateCount] = HeroDecks.HD.enemyManager.cardList[HeroDecks.HD.enemyManager.myHand.GetComponent<DeckManager>().deckList[i].GetComponent<EnemyCardInHand>().thisCard].image;
                    ultimateList[ultimateCount] = i;
                    ultimateCount++;
                    bugCatcher = false;
                }
            }
        }

        // Interface script setup
        if (bugCatcher == false && ultimateCount > 1)
        {
            HeroDecks.HD.interfaceScript.cardAmount = ultimateCount;
            HeroDecks.HD.interfaceScript.interfaceList = interfaceList;
            HeroDecks.HD.interfaceScript.invoker = this;
            HeroDecks.HD.interfaceScript.gameObject.SetActive(true);
        }
        else
        {
            interfaceSignal = 0;
            GameOverseer.GO.interfaceSignalSent = 0;
        }
    }

    public override void effect(PlayerManager user, PlayerManager enemy, int priority)
    {
        switch (priority)
        {
            case 18:
                // Setup ultimate list for enemy
                if (user == HeroDecks.HD.enemyManager)
                {
                    int ultimateCount = 0;
                    for (int i = 0; i < HeroDecks.HD.myManager.myHand.GetComponent<DeckManager>().deckList.Length; i++)
                    {
                        if (HeroDecks.HD.myManager.cardList[i] != null && HeroDecks.HD.myManager.myHand.GetComponent<DeckManager>().deckList[i] != null)
                        {
                            if (HeroDecks.HD.myManager.cardList[HeroDecks.HD.myManager.myHand.GetComponent<DeckManager>().deckList[i].GetComponent<CardInHand>().thisCard] != this &&
                                HeroDecks.HD.myManager.myHand.GetComponent<DeckManager>().deckList[i].activeInHierarchy == true &&
                                (HeroDecks.HD.myManager.cardList[HeroDecks.HD.myManager.myHand.GetComponent<DeckManager>().deckList[i].GetComponent<CardInHand>().thisCard].type == CardTypes.Nullification ||
                                HeroDecks.HD.myManager.cardList[HeroDecks.HD.myManager.myHand.GetComponent<DeckManager>().deckList[i].GetComponent<CardInHand>().thisCard].type == CardTypes.Skill))
                            {
                                ultimateList[ultimateCount] = i;
                                ultimateCount++;
                                bugCatcher = false;
                            }
                        }
                    }
                }

                if (!bugCatcher)
                {
                    enemy.DiscardCard(ultimateList[interfaceSignal]);
                    bugCatcher = true;
                }
                break;
        }
    }
}

public class TemporalShieldTwo : Card
{
    public override void effect(PlayerManager user, PlayerManager enemy, int priority)
    {
        user.protection += 1;
    }
}

public class CloningMachine : Card
{
    public override void effect(PlayerManager user, PlayerManager enemy, int priority)
    {
        if (enemy == HeroDecks.HD.enemyManager) {
            enemy.cardList[GameOverseer.GO.enemyCardPlayed].effect(user, enemy, priority);
        } else if (enemy == HeroDecks.HD.myManager) {
            enemy.cardList[GameOverseer.GO.myCardPlayed].effect(user, enemy, priority);
        }
    }
}