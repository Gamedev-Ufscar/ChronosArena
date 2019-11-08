using UnityEngine;

public class BugaScream : Card
{
    public BugaScream(HeroEnum hero, string name, int cardID, Sprite image, string text, CardTypes type, int minmax, int cost) :
        base(hero, name, cardID, image, text, type, minmax, false, cost)
    {
        
    }

    public override void Effect(Player user, Player enemy, int priority)
    {
        switch (priority) {
            case 16:
                enemy.DealDamage(1, false);
                Debug.Log(user.gameObject.name + "'s Buga Scream");
                break;

            case 17:
                if (user.GetDeckCard(0) != null) {
                    Attack cc = user.GetDeckCard(0).GetCard() as Attack;
                    cc.damage++;
                    user.SetCard(cc as Card, 0);
                    user.GetDeckCard(0).SetupCard(cc);
                }
                RaiseCost(1);

                break;
        }
    }
}

public class WatchAdjustments : Card, ChargeInterface, Limit, Interfacer
{
    public int charge { get; set; }
    public int limit { get; set; }
    public int limitMax { get; set; }
    public Sprite[] interfaceList { get; set; }
    public string[] textList { get; set; }
    public int? interfaceSignal { get; set; }

    public WatchAdjustments(HeroEnum hero, string name, int cardID, Sprite image, string text, CardTypes type, int minmax) :
        base(hero, name, cardID, image, text, type, minmax)
    {
    }

    public void SetSignal(int interfaceSignal)
    {
        this.interfaceSignal = interfaceSignal;
    }

    // Two choices
    public void RaiseCharge(int charge, Player target)
    {
        target.RaiseCharge(charge);
    }

    public void RaiseLimit(int amount, Player target) {
        for (int i = 0; i < 14; i++) { 
            if (target.GetCard(i) as Charge != null) {
                Charge cc = target.GetCard(i) as Charge;
                cc.limit += amount;
                target.SetCard(cc as Card, i);
                if (cc.limit >= cc.limitMax) {
                    DisableCards(target);
                }
            }
        }
    }

    public void DisableCards(Player target) {
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

    public void Interfacing(Player user, Player enemy, bool open)
    {
        textList = new string[2];
        textList[0] = "+1c .";
        textList[1] = "+2c , -2g   .";

        if (open)
            user.Interfacing(this, textList, this, 2);
    }

    public override void Effect(Player user, Player enemy, int priority)
    {
        switch (priority)
        {
            case 14:
                if (interfaceSignal == 0) {
                    RaiseCharge(1, user);
                } else if (interfaceSignal == 1) {
                    RaiseCharge(2, user);
                    user.DealDamage(2, true);
                }
                RaiseLimit(1, user);
                break;
        }
    }
}

public class TimeLock : Card
{
    public TimeLock(HeroEnum hero, string name, int cardID, Sprite image, string text, CardTypes type, int minmax) :
        base(hero, name, cardID, image, text, type, minmax)
    { }

    public override void Effect(Player user, Player enemy, int priority)
    {
        switch (priority)
        {

            case 00:
                user.SetSideEffect(0, (user.GetHP() * 100) + enemy.GetHP());
                Debug.Log(user.gameObject.name + "'s Time Lock");
                break;

            case 14:
                user.RaiseCharge(1);
                break;

        }
    }
}

public class DejaVu : Card
{
    public DejaVu(HeroEnum hero, string name, int cardID, Sprite image, string text, CardTypes type, int minmax) :
        base(hero, name, cardID, image, text, type, minmax)
    { }

    public override void Effect(Player user, Player enemy, int priority)
    {
        switch (priority)
        {
            case 8:
                user.Protect(2);
                break;
            case 17:
                user.SetSideEffect(2, 2);
                break;
        }
    }
}

public class ChronosMachine : Card, Interfacer
{
    public Sprite[] interfaceList { get; set; }
    public string[] textList { get; set; }
    public int? interfaceSignal { get; set; }
    public bool isChronos { get; set; }

    public ChronosMachine(HeroEnum hero, string name, int cardID, Sprite image, string text, CardTypes type, int minmax, bool isChronos, int cost) :
        base(hero, name, cardID, image, text, type, minmax, false, cost)
    {
        this.isChronos = isChronos;
    }

    public void SetSignal(int interfaceSignal)
    {
        this.interfaceSignal = interfaceSignal;
    }

    // Two choices
    public void Interfacing(Player user, Player enemy, bool open)
    {
        if (!isChronos)
        {
            textList = new string[2];
            textList[0] = "VOLTA O g      DO USUÁRIO.";
            textList[1] = "VOLTA O g      DO INIMIGO.";

            if (open)
                user.Interfacing(this, textList, this, 2);
        } else
        {
            interfaceSignal = 0;
        }
    }


    public override void Effect(Player user, Player enemy, int priority)
    {
        switch (priority)
        {
            case 12:
                if (isChronos) {
                    if (user.GetSideEffectValue(0) != 0) {
                        user.SetSideEffect(1, (user.GetHP() * 100) + enemy.GetHP());

                        user.SetHP(user.GetSideEffectValue(0) / 100);
                        enemy.SetHP(user.GetSideEffectValue(0) % 100);
                        RaiseCost(1);
                        Debug.Log("Chronos Machine!");
                    }
                } else {
                    if (user.GetSideEffectValue(1) != 0) {
                        if (interfaceSignal == 0) {
                            user.SetHP(user.GetSideEffectValue(1) / 100);
                        }
                        if (interfaceSignal == 1) {
                            enemy.SetHP(user.GetSideEffectValue(1) % 100);
                        }
                    }
                }
                
                break;
        }
    }
}

public class Sabotage : Card, Damage, Protection, NullInterface
{
    public CardTypes[] nullificationList { get; set; }
    public bool wronged { get; set; }
    public int damage { get; set; }
    public bool isUnblockable { get; set; }
    public int protection { get; set; }

    public int nullType = 0;

    public Sabotage(HeroEnum hero, string name, int cardID, Sprite image, string text, CardTypes type, int minmax, int nullType) :
        base(hero, name, cardID, image, text, type, minmax)
    {
        nullificationList = new CardTypes[4] { CardTypes.Charge, CardTypes.Skill, CardTypes.Ultimate, CardTypes.Item };
        this.nullType = nullType;
    }

    public void CauseDamage(int damage, Player target)
    {
        target.DealDamage(2, false);
    }

    public void SetIsUnblockable(bool isUnblockable)
    {
        this.isUnblockable = isUnblockable;
    }

    public void Protect(int protection, Player target)
    {
        target.Protect(2);
    }

    public void Nullify(Player target)
    {
        wronged = true;
        for (int i = 0; i < nullificationList.Length; i++)
        {
            if (target.GetCardPlayed().GetCardType() == nullificationList[i])
            {
                target.GetCardPlayed().SetIsNullified(true);
                wronged = false;
            }
        }
    }
   

    public override void Effect(Player user, Player enemy, int priority)
    {
        switch (priority) {
            case 4:
                Nullify(enemy);
                break;

            case 8:
                if (nullType == 1) { Protect(protection, user); }
                break;

            case 16:
                if (nullType == 0 && !wronged) { CauseDamage(damage, enemy); }
                break;
        }
    }
}

public class SabotageR : Card, NullInterface, Interfacer
{
    public CardTypes[] nullificationList { get; set; }
    public bool wronged { get; set; }
    public Sprite[] interfaceList { get; set; }
    public Card[] cardList { get; set; }
    public int? interfaceSignal { get; set; }

    int[] discardedCardList = new int[10]; // Lists the deckList indexes of discarded cards
    int discardedCount = 0;

    public int nullType = 0;

    public SabotageR(HeroEnum hero, string name, int cardID, Sprite image, string text, CardTypes type, int minmax) :
        base(hero, name, cardID, image, text, type, minmax)
    {
        nullificationList = new CardTypes[4] { CardTypes.Charge, CardTypes.Skill, CardTypes.Ultimate, CardTypes.Item };
        interfaceSignal = null;
    }

    public void SetSignal(int interfaceSignal)
    {
        this.interfaceSignal = interfaceSignal;
    }

    public void Nullify(Player target)
    {
        wronged = true;
        for (int i = 0; i < nullificationList.Length; i++)
        {
            if (target.GetCardPlayed().GetCardType() == nullificationList[i])
            {
                target.GetCardPlayed().SetIsNullified(true);
                wronged = false;
            }
        }
    }

    public void Interfacing(Player user, Player enemy, bool open)
    {
        // Reset Variables
        cardList = new Card[Constants.maxCardAmount];
        discardedCount = 0;
        interfaceSignal = null;

        if (nullType == 2)
        {
            // First check if the Nullification is valid - if yes, proceed
            wronged = true;
            for (int i = 0; i < nullificationList.Length; i++)
                if (enemy.GetCardPlayed().GetCardType() == nullificationList[i])
                    wronged = false;

            if (!wronged)
            {

                // Run through Deck List, check if there's a disabled nullification card
                for (int i = 0; i < Constants.maxCardAmount; i++)
                {
                    if (user.GetCard(i) != null && user.GetCard(i) != this && !user.GetDeckCard(i).isActiveAndEnabled &&
                        user.GetCard(i).GetCardType() == CardTypes.Nullification)
                    {
                        cardList[discardedCount] = user.GetCard(i);
                        discardedCardList[discardedCount] = i;
                        discardedCount++;
                    }
                }

                // Interface script setup
                if (discardedCount > 1)
                {
                    if (open)
                        user.Interfacing(cardList, this, discardedCount);
                }
                else
                    interfaceSignal = 0;
            }
        }
    }

    public override void Effect(Player user, Player enemy, int priority)
    {
        switch (priority)
        {
            case 4:
                Nullify(enemy);
                break;

            case 19:
                // Setup discarded list for enemy
                if (nullType == 2 && !wronged)
                {
                    if (discardedCount > 0)
                    {
                        user.RestoreCard(discardedCardList[(int)interfaceSignal]);
                    }
                }
                break;
        }
    }
}

public class Catastrophe : Card, Interfacer
{
    public Sprite[] interfaceList { get; set; }
    public Card[] cardList { get; set; }
    public int? interfaceSignal { get; set; }

    int[] ultimateList = new int[3]; // Lists the deckList indexes of ultimate cards
    int ultimateCount = 0;

    public Catastrophe(HeroEnum hero, string name, int cardID, Sprite image, string text, CardTypes type, int minmax, bool isReaction, int cost) :
        base(hero, name, cardID, image, text, type, minmax, isReaction, cost)
    { }

    public void SetSignal(int interfaceSignal)
    {
        this.interfaceSignal = interfaceSignal;
    }

    public void Interfacing(Player user, Player enemy, bool open)
    {
        // Reset Variables
        cardList = new Card[Constants.maxCardAmount];
        ultimateCount = 0;
        interfaceSignal = null;

        // Run through ENEMY Deck List, check if there's an ultimate card
        for (int i = 0; i < Constants.maxHandSize; i++) {
            if (enemy.GetCard(i) != null && enemy.GetCard(i) != this && enemy.GetDeckCard(i).isActiveAndEnabled && !enemy.GetDeckCard(i).GetIsReaction() && 
                enemy.GetCard(i).GetCardType() == CardTypes.Ultimate) {
                cardList[ultimateCount] = enemy.GetCard(i);
                ultimateList[ultimateCount] = i;
                ultimateCount++;
            }
        }

        // Interface script setup
        if (ultimateCount > 1)
        {
            if (open)
                user.Interfacing(cardList, this, ultimateCount);
        }
        else
            interfaceSignal = 0;

    }

    public override void Effect(Player user, Player enemy, int priority)
    {
        switch (priority)
        {
            case 10:
                enemy.RaiseCharge(-2);
                break;
            case 18:
                if (ultimateCount > 0)
                {
                    enemy.DiscardCard(ultimateList[(int)interfaceSignal]);
                }
                this.RaiseCost(1);
                break;
        }
    }
}

public class PerverseEngineering : Card, Interfacer
{
    public Sprite[] interfaceList { get; set; }
    public Card[] cardList { get; set; }
    public int? interfaceSignal { get; set; }

    int[] ultimateList = new int[7]; // Lists the deckList indexes of relevant cards

    int specialCount = 0;

    public PerverseEngineering(HeroEnum hero, string name, int cardID, Sprite image, string text, CardTypes type, int minmax) :
        base(hero, name, cardID, image, text, type, minmax)
    {
        interfaceSignal = null;
    }

    public void SetSignal(int interfaceSignal)
    {
        this.interfaceSignal = interfaceSignal;
    }

    public void Interfacing(Player user, Player enemy, bool open)
    {
        // Reset Variables
        cardList = new Card[Constants.maxCardAmount];
        specialCount = 0;
        interfaceSignal = null;

        // Run through ENEMY Deck List, check if there's a skill or nullification card
        for (int i = 0; i < Constants.maxCardAmount; i++)
        {
            if (enemy.GetCard(i) != null && enemy.GetCard(i) != this && enemy.GetDeckCard(i).isActiveAndEnabled && !enemy.GetDeckCard(i).GetIsReaction() &&
               (enemy.GetCard(i).GetCardType() == CardTypes.Nullification || enemy.GetCard(i).GetCardType() == CardTypes.Skill)) {
                    cardList[specialCount] = enemy.GetCard(i);
                    ultimateList[specialCount] = i;
                    specialCount++;
            }
        }

        // Interface script setup
        if (specialCount > 1)
        {
            if (open)
                user.Interfacing(cardList, this, specialCount);
        }
        else
            interfaceSignal = 0;
    }

    public override void Effect(Player user, Player enemy, int priority)
    {
        switch (priority)
        {
            case 18:
                if (specialCount > 0)
                    enemy.DiscardCard(ultimateList[(int)interfaceSignal]);
                break;
        }
    }
}

public class TemporalShieldTwo : Card
{
    public TemporalShieldTwo(HeroEnum hero, string name, int cardID, Sprite image, string text, CardTypes type, int minmax, bool isReaction, int cost) :
        base(hero, name, cardID, image, text, type, minmax, isReaction, cost)
    { }

    public override void Effect(Player user, Player enemy, int priority)
    {
        user.Protect(2);
    }
}

public class CloningMachine : Card
{
    public CloningMachine(HeroEnum hero, string name, int cardID, Sprite image, string text, CardTypes type, int minmax) :
        base(hero, name, cardID, image, text, type, minmax)
    { }

    public override void Effect(Player user, Player enemy, int priority)
    {
        enemy.GetCardPlayed().Effect(user, enemy, priority);
    }
}