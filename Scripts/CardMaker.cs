using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardMaker : MonoBehaviour
{
    public static CardMaker CM;
    Hashtable heroHashtable;
    static CardTypes[] defaultAttackDisableList = new CardTypes[4] { CardTypes.Charge, CardTypes.Skill, CardTypes.Ultimate, CardTypes.Item };
    HeroEnum hero;

    void Awake () {
        DontDestroyOnLoad(this);
        if (CM == null) {
            CM = this;
            Setup();
        } else {
            if (CM != this) {
                Destroy(this);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Setup();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Setup()
    {
        // Declaration
        heroHashtable = new Hashtable();
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
        timothyHT.Add(1, new Defense(hero, "ESCUDO TEMPORAL", 1, ImageStash.IS.GetImage(hero, 1), "",
            CardTypes.Defense, 0808, 2));
        timothyHT.Add(2, new WatchAdjustments(hero, "AJUSTES NO RELÓGIO", 2, ImageStash.IS.GetImage(hero, 2), "+1c   OU +2c , -2g   .",
            CardTypes.Charge, 1414));
        timothyHT.Add(3, new Nullification(hero, "QUATRO SEGUNDOS ATRÁS", 3, ImageStash.IS.GetImage(hero, 3), "",
            CardTypes.Nullification, 0404, defaultAttackDisableList));
        timothyHT.Add(4, new Nullification(hero, "QUATRO SEGUNDOS ATRÁS", 4, ImageStash.IS.GetImage(hero, 3), "",
            CardTypes.Nullification, 0404, defaultAttackDisableList));
        timothyHT.Add(5, new TimeLock(hero, "MARCA DO TEMPO", 5, ImageStash.IS.GetImage(hero, 4), "MARCA O TEMPO PARA A ULTIMATE. \\n+1c .",
            CardTypes.Skill, 0014));
        timothyHT.Add(6, new TimeLock(hero, "MARCA DO TEMPO", 6, ImageStash.IS.GetImage(hero, 4), "MARCA O TEMPO PARA A ULTIMATE. \\n+1c .",
            CardTypes.Skill, 0014));
        timothyHT.Add(7, new DejaVu(hero, "DÉJÀ VU", 7, ImageStash.IS.GetImage(hero, 5), "2b   . \\nPREVÊ A CARTA DO OPONENTE, \\nNO PRÓXIMO TURNO.",
            CardTypes.Skill, 0817));
        timothyHT.Add(8, new ChronosMachine(hero, "MÁQUINA DE CRONOS", 8, ImageStash.IS.GetImage(hero, 6), "O g      DE AMBOS JOGADORES \\nVOLTA PARA QUANDO A ÚLTIMA \\nMARCA DO TEMPO FOI JOGADA.",
            CardTypes.Ultimate, 1212, true, 1));
        timothyHT.Add(9, new ChronosMachine(hero, "DE VOLTA AO FUTURO", 9, ImageStash.IS.GetImage(hero, 7), "O g      DO JOGADOR DE SUA \\nESCOLHA VOLTA PARA QUANDO A \\nMÁQUINA DE CRONOS MAIS \\nRECENTE FOI USADA.",
            CardTypes.Ultimate, 1212, false, 2));

        // Uga
        hero = HeroEnum.Uga;
        ugaHT.Add(0, new Attack(hero, "PANCADA COM TRONCO", 0, ImageStash.IS.GetImage(hero, 0), "",
            CardTypes.Attack, 1616, 2, false, 2));
        ugaHT.Add(1, new Defense(hero, "DEFESA COM TRONCO", 1, ImageStash.IS.GetImage(hero, 1), "",
            CardTypes.Defense, 0808, 2));
        ugaHT.Add(2, new Charge(hero, "COÇAR A CABEÇA", 2, ImageStash.IS.GetImage(hero, 2), "",
            CardTypes.Charge, 1414, 1, 3));
        ugaHT.Add(3, new Nullification(hero, "GRITO UGA", 3, ImageStash.IS.GetImage(hero, 3), "",
            CardTypes.Nullification, 0404, defaultAttackDisableList));
        ugaHT.Add(4, new Nullification(hero, "GRITO UGA", 4, ImageStash.IS.GetImage(hero, 3), "",
            CardTypes.Nullification, 0404, defaultAttackDisableList));
        ugaHT.Add(5, new BasicSkill(hero, "LANÇA DE OSSOS", 5, ImageStash.IS.GetImage(hero, 4), "CAUSA 3a      .",
            CardTypes.Skill, 1616, 3, false, 0, 0));
        ugaHT.Add(6, new AutoHealSkill(hero, "CARNE CRUA", 6, ImageStash.IS.GetImage(hero, 5), "CURA 2g   .",
            CardTypes.Skill, 1212, 2, false));
        ugaHT.Add(7, new BugaScream(hero, "FÚRIA BUGA", 7, ImageStash.IS.GetImage(hero, 6), "CAUSA 1a     . A CARTA DE ATAQUE DO \nUSUÁRIO AUMENTA DE DANO EM 1 \\nPERMANENTEMENTE.",
            CardTypes.Ultimate, 1617, 2));

        // Yuri
        hero = HeroEnum.Yuri;
        yuriHT.Add(0, new Attack(hero, "TIRO DE SNIPER", 0, ImageStash.IS.GetImage(hero, 0), "",
            CardTypes.Attack, 1616, 3, false, 1));
        yuriHT.Add(1, new Defense(hero, "COBERTURA", 1, ImageStash.IS.GetImage(hero, 1), "",
            CardTypes.Defense, 0808, 1));
        yuriHT.Add(2, new Charge(hero, "RECARREGAR", 2, ImageStash.IS.GetImage(hero, 2), "",
            CardTypes.Charge, 1414, 1, 3));
        yuriHT.Add(3, new Nullification(hero, "TIRO DE SUPRESSÃO", 3, ImageStash.IS.GetImage(hero, 3), "",
            CardTypes.Nullification, 0404, defaultAttackDisableList));
        yuriHT.Add(4, new Attack(hero, "CHUVA DE BALAS", 4, ImageStash.IS.GetImage(hero, 4), "CAUSA 4a     .\\nATIVA LIMITE DE ATAQUE.",
            CardTypes.Skill, 1616, 4, false, 1));
        yuriHT.Add(5, new BasicSkill(hero, "TIRO CALCULADO", 5, ImageStash.IS.GetImage(hero, 5), "CAUSA 3a     .",
            CardTypes.Skill, 1616, 3, false, 0, 0));
        yuriHT.Add(6, new SideEffectSkill(hero, "PONTO FRACO", 6, ImageStash.IS.GetImage(hero, 6), "DANO CAUSADO PRÓXIMO TURNO É \\nIMBLOQUEÁVEL.",
            CardTypes.Skill, 1717, 0, 2));
        yuriHT.Add(7, new Dexterity(hero, "DESTREZA", 7, ImageStash.IS.GetImage(hero, 7), "RECUPERA 1 e     OU 1 d   .",
            CardTypes.Ultimate, 1818, 1));
        yuriHT.Add(8, new Dexterity(hero, "VODKA", 8, ImageStash.IS.GetImage(hero, 8), "-1g      QUANDO ANULADO.",
            CardTypes.Passive, 0000, 1));

    }
    
    public Card MakeCard(HeroEnum hero, int id)
    {
        Hashtable hashToAccess = (Hashtable)heroHashtable[hero];
        Card card = (Card)((Hashtable)heroHashtable[hero])[id];
        //Debug.Log(card.GetName());
        return card;
    }

    public SideEffect MakeSideEffect(HeroEnum hero, int id)
    {
        Hashtable hashToAccess = (Hashtable)heroHashtable[hero];
        SideEffect sideEffect = (SideEffect)((Hashtable)heroHashtable[hero])[id];
        //Debug.Log(card.GetName());
        return sideEffect;
    }
}