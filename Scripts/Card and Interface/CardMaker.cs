using System.Collections;
using UnityEngine;

public class CardMaker : MonoBehaviour
{
    public static CardMaker CM;
    Hashtable heroHashtable;
    static CardTypes[] defaultNullDisableList = new CardTypes[4] { CardTypes.Charge, CardTypes.Skill, CardTypes.Ultimate, CardTypes.Item };
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

        ImageStash IS = ImageStash.IS;
        if (IS == null) return;

        // Timothy
        hero = HeroEnum.Timothy;
        timothyHT.Add(0, new Attack(hero, "PISTOLA SÔNICA", 0, IS.GetImage(hero, 0), "",
            CardTypes.Attack, 1616, 2, false, 2));
        timothyHT.Add(1, new Defense(hero, "ESCUDO TEMPORAL", 1, IS.GetImage(hero, 1), "",
            CardTypes.Defense, 0808, 2));
        timothyHT.Add(2, new WatchAdjustments(hero, "AJUSTES NO RELÓGIO", 2, IS.GetImage(hero, 2), "+1c   OU +2c , -2g   .",
            CardTypes.Charge, 1414));
        timothyHT.Add(3, new Nullification(hero, "QUATRO SEGUNDOS ATRÁS", 3, IS.GetImage(hero, 3), "",
            CardTypes.Nullification, 0404, defaultNullDisableList));
        timothyHT.Add(4, new Nullification(hero, "QUATRO SEGUNDOS ATRÁS", 4, IS.GetImage(hero, 3), "",
            CardTypes.Nullification, 0404, defaultNullDisableList));
        timothyHT.Add(5, new TimeLock(hero, "MARCA DO TEMPO", 5, IS.GetImage(hero, 4), "MARCA O TEMPO PARA A ULTIMATE. \\n+1c .",
            CardTypes.Skill, 0014));
        timothyHT.Add(6, new TimeLock(hero, "MARCA DO TEMPO", 6, IS.GetImage(hero, 4), "MARCA O TEMPO PARA A ULTIMATE. \\n+1c .",
            CardTypes.Skill, 0014));
        timothyHT.Add(7, new DejaVu(hero, "DÉJÀ VU", 7, IS.GetImage(hero, 5), "2b   . \\nPREVÊ A CARTA DO OPONENTE, \\nNO PRÓXIMO TURNO.",
            CardTypes.Skill, 0817));
        timothyHT.Add(8, new ChronosMachine(hero, "MÁQUINA DE CRONOS", 8, IS.GetImage(hero, 6), "O g      DE AMBOS JOGADORES \\nVOLTA PARA QUANDO A ÚLTIMA \\nMARCA DO TEMPO FOI JOGADA.",
            CardTypes.Ultimate, 1212, true, 1));
        timothyHT.Add(9, new ChronosMachine(hero, "DE VOLTA AO FUTURO", 9, IS.GetImage(hero, 7), "O g      DO JOGADOR DE SUA \\nESCOLHA VOLTA PARA QUANDO A \\nMÁQUINA DE CRONOS MAIS \\nRECENTE FOI USADA.",
            CardTypes.Ultimate, 1212, false, 2));
        timothyHT.Add(10, new Chronos(0000));
        timothyHT.Add(11, new Chronos(0000));
        timothyHT.Add(12, new DejaVuSE(0, SEPhase.Choice));

        // Harold
        hero = HeroEnum.Harold;
        haroldHT.Add(0, new Attack(hero, "MINI-LASER", 0, IS.GetImage(hero, 0), "AO ATINGIR LIMITE, AINDA PODE JOGAR d   .",
            CardTypes.Attack, 1616, 2, false, 1));
        haroldHT.Add(1, new Defense(hero, "ESCONDERIJO", 1, IS.GetImage(hero, 1), "",
            CardTypes.Defense, 0808, 1));
        haroldHT.Add(2, new Charge(hero, "PLANOS MALÉFICOS", 2, IS.GetImage(hero, 2), "",
            CardTypes.Charge, 1414, 1, 3));
        haroldHT.Add(3, new Sabotage(hero, "SABOTAGEM", 3, IS.GetImage(hero, 3), "CAUSA 2a      , \\nSE ACERTAR ANULAÇÃO.",
            CardTypes.Nullification, 0416, 0));
        haroldHT.Add(4, new Sabotage(hero, "SABOTAGEM", 4, IS.GetImage(hero, 4), "2b   , MESMO SE ERRAR ANULAÇÃO.",
            CardTypes.Nullification, 0408, 1));
        haroldHT.Add(5, new SabotageR(hero, "SABOTAGEM", 5, IS.GetImage(hero, 5), "RECUPERA 1 d   , \\nSE ACERTAR ANULAÇÃO.",
            CardTypes.Nullification, 0419));
        haroldHT.Add(6, new CloningMachine(hero, "MÁQUINA DE CLONAGEM", 6, IS.GetImage(hero, 6), "COPIA OS EFEITOS DA CARTA INIMIGA.",
            CardTypes.Skill, 0018));
        haroldHT.Add(7, new CloningMachine(hero, "MÁQUINA DE CLONAGEM", 7, IS.GetImage(hero, 6), "COPIA OS EFEITOS DA CARTA INIMIGA.",
            CardTypes.Skill, 0018));
        haroldHT.Add(8, new PerverseEngineering(hero, "ENGENHARIA MALIGNA", 8, IS.GetImage(hero, 7), "DESCARTA d     OU e     INIMIGA.",
            CardTypes.Skill, 1818));
        haroldHT.Add(9, new Catastrophe(hero, "MESTRE DO DESASTRE", 9, IS.GetImage(hero, 8), "RETIRA 2c   E DESCARTA f      \\nINIMIGA DE SUA ESCOLHA.",
            CardTypes.Ultimate, 1212, false, 2));
        haroldHT.Add(10, new TemporalShieldTwo(hero, "ESCUDO TEMPORAL 2.0", 10, IS.GetImage(hero, 9), "CONCEDE 2 b   .\\nREAÇÃO.",
            CardTypes.Ultimate, 0808, true, 1));

        // Uga
        hero = HeroEnum.Uga;
        ugaHT.Add(0, new Attack(hero, "PANCADA COM TRONCO", 0, IS.GetImage(hero, 0), "",
            CardTypes.Attack, 1616, 2, false, 2));
        ugaHT.Add(1, new Defense(hero, "DEFESA COM TRONCO", 1, IS.GetImage(hero, 1), "",
            CardTypes.Defense, 0808, 2));
        ugaHT.Add(2, new Charge(hero, "COÇAR A CABEÇA", 2, IS.GetImage(hero, 2), "",
            CardTypes.Charge, 1414, 1, 3));
        ugaHT.Add(3, new Nullification(hero, "GRITO UGA", 3, IS.GetImage(hero, 3), "",
            CardTypes.Nullification, 0404, defaultNullDisableList));
        ugaHT.Add(4, new Nullification(hero, "GRITO UGA", 4, IS.GetImage(hero, 3), "",
            CardTypes.Nullification, 0404, defaultNullDisableList));
        ugaHT.Add(5, new BasicSkill(hero, "LANÇA DE OSSOS", 5, IS.GetImage(hero, 4), "CAUSA 3a      .",
            CardTypes.Skill, 1616, 3, false, 0, 0));
        ugaHT.Add(6, new AutoHealSkill(hero, "CARNE CRUA", 6, IS.GetImage(hero, 5), "CURA 2g   .",
            CardTypes.Skill, 1212, 2, false));
        ugaHT.Add(7, new BugaScream(hero, "FÚRIA BUGA", 7, IS.GetImage(hero, 6), "CAUSA 1a     . A CARTA DE ATAQUE DO \nUSUÁRIO AUMENTA DE DANO EM 1 \\nPERMANENTEMENTE.",
            CardTypes.Ultimate, 1617, 2));

        // Yuri
        hero = HeroEnum.Yuri;
        yuriHT.Add(0, new Attack(hero, "TIRO DE SNIPER", 0, IS.GetImage(hero, 0), "",
            CardTypes.Attack, 1616, 3, false, 1));
        yuriHT.Add(1, new Defense(hero, "COBERTURA", 1, IS.GetImage(hero, 1), "",
            CardTypes.Defense, 0808, 1));
        yuriHT.Add(2, new Charge(hero, "RECARREGAR", 2, IS.GetImage(hero, 2), "",
            CardTypes.Charge, 1414, 1, 3));
        yuriHT.Add(3, new Nullification(hero, "TIRO DE SUPRESSÃO", 3, IS.GetImage(hero, 3), "",
            CardTypes.Nullification, 0404, defaultNullDisableList));
        yuriHT.Add(4, new Attack(hero, "CHUVA DE BALAS", 4, IS.GetImage(hero, 4), "CAUSA 4a     .\\nATIVA LIMITE DE ATAQUE.",
            CardTypes.Skill, 1616, 4, false, 1));
        yuriHT.Add(5, new BasicSkill(hero, "TIRO CALCULADO", 5, IS.GetImage(hero, 5), "CAUSA 3a     .",
            CardTypes.Skill, 1616, 3, false, 0, 0));
        yuriHT.Add(6, new SideEffectSkill(hero, "PONTO FRACO", 6, IS.GetImage(hero, 6), "DANO CAUSADO PRÓXIMO TURNO É \\nIMBLOQUEÁVEL.",
            CardTypes.Skill, 1717, 0, 2));
        yuriHT.Add(7, new Dexterity(hero, "DESTREZA", 7, IS.GetImage(hero, 7), "RECUPERA 1 e     OU 1 d   .",
            CardTypes.Ultimate, 1818, 1));
        yuriHT.Add(8, new Dexterity(hero, "VODKA", 8, IS.GetImage(hero, 8), "-1g      QUANDO ANULADO.",
            CardTypes.Passive, 0000, 1));
        yuriHT.Add(9, new WeakSpot(0, SEPhase.EffectsBefore));
        yuriHT.Add(10, new Vodka(1, SEPhase.EffectsAfter));

    }
    
    public Card MakeCard(HeroEnum hero, int id)
    {
        Hashtable hashToAccess = (Hashtable)heroHashtable[hero];
        if (((Hashtable)heroHashtable[hero])[id] is Card)
        {
            Card card = (Card)((Hashtable)heroHashtable[hero])[id];
            return card;
        }
        return null;
    }

    public SideEffect MakeSideEffect(HeroEnum hero, int id)
    {
        Hashtable hashToAccess = (Hashtable)heroHashtable[hero];
        SideEffect sideEffect = (SideEffect)((Hashtable)heroHashtable[hero])[id];
        return sideEffect;
    }

    public string HeroName(HeroEnum hero)
    {
        switch (hero)
        {
            case HeroEnum.Uga: return "Uga";
            case HeroEnum.Timothy: return "Timothy";
            case HeroEnum.Yuri: return "Yuri";
            case HeroEnum.Harold: return "Dr. Harold";
            default: return "";
        }
    }
}