using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardMaker : MonoBehaviour
{
    public static CardMaker CM;
    Hashtable heroHashtable = new Hashtable();
    static CardTypes[] defaultAttackDisableList = new CardTypes[4] { CardTypes.Charge, CardTypes.Skill, CardTypes.Ultimate, CardTypes.Item };
    HeroEnum hero;

    void Awake () {
        DontDestroyOnLoad(this);
        if (CM == null) {
            CM = this;
        } else {
            if (CM != this) {
                Destroy(this);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Declaration
        Hashtable heroHT = new Hashtable();
        Hashtable timothyHT = new Hashtable();
        Hashtable haroldHT = new Hashtable();
        Hashtable ugaHT = new Hashtable();
        Hashtable yuriHT = new Hashtable();

        // Hero HT
        heroHashtable.Add(HeroEnum.Timothy, timothyHT);
        heroHashtable.Add(HeroEnum.Harold, haroldHT);
        heroHashtable.Add(HeroEnum.Uga, ugaHT);
        heroHashtable.Add(HeroEnum.Yuri, yuriHT);

        // Timothy
        hero = HeroEnum.Timothy;
        timothyHT.Add(0, new Attack(hero, "PISTOLA SÔNICA", 0, ImageStash.IS.GetImage(hero, 0), "", 
            CardTypes.Attack, 1616, 2, false, 2));
        timothyHT.Add(1, new Defense(hero, name, 1, ImageStash.IS.GetImage(hero, 0), "", 
            CardTypes.Defense, 0808, 2));
        timothyHT.Add(2, new WatchAdjustments(hero, name, 2, ImageStash.IS.GetImage(hero, 0), "", 
            CardTypes.Charge, 1414));
        timothyHT.Add(3, new Nullification(hero, name, 3, ImageStash.IS.GetImage(hero, 0), "", 
            CardTypes.Nullification, 0404, defaultAttackDisableList));
        timothyHT.Add(4, new Nullification(hero, name, 4, ImageStash.IS.GetImage(hero, 0), "", 
            CardTypes.Nullification, 0404, defaultAttackDisableList));
        timothyHT.Add(5, new TimeLock(hero, name, 5, ImageStash.IS.GetImage(hero, 0), "", 
            CardTypes.Skill, 0014));
        timothyHT.Add(6, new TimeLock(hero, name, 6, ImageStash.IS.GetImage(hero, 0), "", 
            CardTypes.Skill, 0014));
        timothyHT.Add(7, new DejaVu(hero, name, 7, ImageStash.IS.GetImage(hero, 0), "", 
            CardTypes.Skill, 0817));
        timothyHT.Add(8, new ChronosMachine(hero, name, 8, ImageStash.IS.GetImage(hero, 0), "", 
            CardTypes.Ultimate, 1212, true));
        timothyHT.Add(9, new ChronosMachine(hero, name, 9, ImageStash.IS.GetImage(hero, 0), "", 
            CardTypes.Ultimate, 1212, false));

        // Uga
        hero = HeroEnum.Uga;
        ugaHT.Add(0, new Attack(hero, name, 0, ImageStash.IS.GetImage(hero, 0), "", 
            CardTypes.Attack, 1616, 2, false, 2));
        ugaHT.Add(1, new Defense(hero, name, 1, ImageStash.IS.GetImage(hero, 0), "", 
            CardTypes.Defense, 0808, 2));
        ugaHT.Add(2, new Charge(hero, name, 2, ImageStash.IS.GetImage(hero, 0), "", 
            CardTypes.Charge, 1414, 1, 3));
        ugaHT.Add(3, new Nullification(hero, name, 3, ImageStash.IS.GetImage(hero, 0), "", 
            CardTypes.Nullification, 0404, defaultAttackDisableList));
        ugaHT.Add(4, new Nullification(hero, name, 4, ImageStash.IS.GetImage(hero, 0), "", 
            CardTypes.Nullification, 0404, defaultAttackDisableList));
        ugaHT.Add(5, new BasicSkill(hero, name, 5, ImageStash.IS.GetImage(hero, 0), "", 
            CardTypes.Skill, 1616, 3, false, 0, 0));
        ugaHT.Add(6, new AutoHealSkill(hero, name, 6, ImageStash.IS.GetImage(hero, 0), "", 
            CardTypes.Skill, 1212, 2, false));
        ugaHT.Add(7, new BugaScream(hero, name, 7, ImageStash.IS.GetImage(hero, 0), "", 
            CardTypes.Ultimate, 1617));

        // Yuri
        hero = HeroEnum.Yuri;
        yuriHT.Add(0, new Attack(hero, name, 0, ImageStash.IS.GetImage(hero, 0), "", 
            CardTypes.Attack, 1616, 3, false, 1));
        yuriHT.Add(1, new Defense(hero, name, 1, ImageStash.IS.GetImage(hero, 0), "", 
            CardTypes.Defense, 0808, 1));
        yuriHT.Add(2, new Charge(hero, name, 2, ImageStash.IS.GetImage(hero, 0), "", 
            CardTypes.Charge, 1414, 1, 3));
        yuriHT.Add(3, new Nullification(hero, name, 3, ImageStash.IS.GetImage(hero, 0), "", 
            CardTypes.Nullification, 0404, defaultAttackDisableList));
        yuriHT.Add(4, new Attack(hero, name, 4, ImageStash.IS.GetImage(hero, 0), "", 
            CardTypes.Skill, 1616, 4, false, 1));
        yuriHT.Add(5, new BasicSkill(hero, name, 5, ImageStash.IS.GetImage(hero, 0), "", 
            CardTypes.Skill, 1616, 3, false, 0, 0));
        yuriHT.Add(6, new SideEffectSkill(hero, name, 6, ImageStash.IS.GetImage(hero, 0), "", 
            CardTypes.Skill, 1717, 0, 2));
        yuriHT.Add(7, new Dexterity(hero, name, 7, ImageStash.IS.GetImage(hero, 0), "", 
            CardTypes.Ultimate, 1818));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public Card MakeCard(HeroEnum hero, int id)
    {
        Hashtable hashToAccess = (Hashtable)heroHashtable[hero];
        Card card = (Card)hashToAccess[id];
        return card;
    }
}
