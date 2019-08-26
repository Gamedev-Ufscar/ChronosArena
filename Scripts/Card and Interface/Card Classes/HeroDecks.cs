using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroDecks : MonoBehaviour
{
    public static HeroDecks HD;
    public static float cardZoomSize = 2.8f;
    public static float cardMoveUp = 11f;

    public PlayerManager myManager;
    public PlayerManager enemyManager;
    public InterfaceScript interfaceScript;
    public AudioManager audioManager;
    public VoicelineManager voicelineManager;


    private void Awake()
    {
        HeroDecks.HD = this;
    }

    private void OnEnable()
    {
        if (HeroDecks.HD == null)
        {
            HeroDecks.HD = this;
        }
        else
        {
            if (HeroDecks.HD != this)
            {
                Destroy(HeroDecks.HD);
                HeroDecks.HD = this;
            }
        }

        HeroDecks.HD.myManager = GameObject.Find("Player Manager").GetComponent<PlayerManager>();
        for (int i = 0; i < myManager.sideList.Length; i++) { myManager.sideList[i] = 0; }

        HeroDecks.HD.enemyManager = GameObject.Find("Enemy Manager").GetComponent<PlayerManager>();
        for (int i = 0; i < enemyManager.sideList.Length; i++) { enemyManager.sideList[i] = 0; }

        HeroDecks.HD.audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
        HeroDecks.HD.voicelineManager = GameObject.Find("Audio Manager").GetComponent<VoicelineManager>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public string value (Card cardd, int value)
    {
        switch (cardd.type)
        {
            case CardTypes.Attack:
                if (value == 1) {
                    Damage damageCardd = (Damage)cardd;
                    return "" + damageCardd.damage;
                } else {
                    Limit limitCardd = (Limit)cardd;
                    return "" + limitCardd.limitMax;
                }

            case CardTypes.Defense:
                if (value == 1)
                {
                    Defense defenseCardd = (Defense)cardd;
                    return "" + defenseCardd.protection;
                } else
                {
                    return "";
                }

            case CardTypes.Charge:
                if (value == 1) {
                    ChargeInterface chargeCardd = (ChargeInterface)cardd;
                    return "" + chargeCardd.charge;
                } else {
                    Limit limitCardd = (Limit)cardd;
                    return "" + limitCardd.limitMax;
                }

            case CardTypes.Ultimate:
                if (value == 2)
                {
                    return "" + cardd.cost;
                } else
                {
                    return "";
                }

            default:
                return "";
        }
    }

    public Card heroCard(HeroEnum hero, int id)
    {
        switch (hero)
        {
            case HeroEnum.Uga:
                return UgaDeck(id);

            case HeroEnum.Timothy:
                return TimothyDeck(id);

            case HeroEnum.Harold:
                return HaroldDeck(id);

            case HeroEnum.Yuri:
                return YuriDeck(id);

            default:
                return RobotoDeck(id);
        }
    }

    public Card RobotoDeck(int id)
    {
        switch (id)
        {
            case 0:
                Attack robotoAttack = new Attack();
                robotoAttack.name = "Ataque Robotico";
                robotoAttack.type = CardTypes.Attack;
                robotoAttack.id = 0;
                robotoAttack.minmax = 1616;
                robotoAttack.damage = 2;
                robotoAttack.isUnblockable = false;
                robotoAttack.limit = 0;
                robotoAttack.limitMax = 2;
                return robotoAttack;

            case 1:
                Defense robotoDefense = new Defense();
                robotoDefense.name = "Defesa Robotica";
                robotoDefense.type = CardTypes.Defense;
                robotoDefense.id = 1;
                robotoDefense.minmax = 0808;
                robotoDefense.protection = 2;
                return robotoDefense;

            case 2:
                Charge robotoCharge = new Charge();
                robotoCharge.name = "Carga Robotica";
                robotoCharge.type = CardTypes.Charge;
                robotoCharge.id = 2;
                robotoCharge.minmax = 1414;
                robotoCharge.charge = 1;
                robotoCharge.limit = 0;
                robotoCharge.limitMax = 3;
                return robotoCharge;

            case 3:
                Nullification robotoNull = new Nullification();
                robotoNull.name = "Anulacao Robotica";
                robotoNull.type = CardTypes.Nullification;
                robotoNull.id = 3;
                robotoNull.minmax = 0404;
                robotoNull.nullificationList = new CardTypes[4];
                robotoNull.nullificationList[0] = CardTypes.Charge; robotoNull.nullificationList[1] = CardTypes.Skill;
                robotoNull.nullificationList[2] = CardTypes.Ultimate; robotoNull.nullificationList[3] = CardTypes.Item;
                return robotoNull;

            case 4:
                return RobotoDeck(3);


            case 5:
                BasicSkill robotoBoom = new BasicSkill();
                robotoBoom.name = "Boom Robotico";
                robotoBoom.type = CardTypes.Skill;
                robotoBoom.id = 5;
                robotoBoom.minmax = 1616;
                robotoBoom.damage = 4;
                robotoBoom.isUnblockable = false;
                robotoBoom.protection = 0;
                robotoBoom.charge = 0;
                return robotoBoom;

            case 6:
                BasicSkill robotoUlti = new BasicSkill();
                robotoUlti.name = "Mecha Robotico";
                robotoUlti.type = CardTypes.Ultimate;
                robotoUlti.id = 6;
                robotoUlti.minmax = 0816;
                robotoUlti.cost = 2;
                robotoUlti.damage = 2;
                robotoUlti.isUnblockable = false;
                robotoUlti.protection = 2;
                robotoUlti.charge = 0;
                return robotoUlti;

            default:
                return null;
        }
    }

    public Card UgaDeck(int id)
    {
        switch (id)
        {
            case 0:
                Attack clubSmack = new Attack();
                clubSmack.name = "PANCADA COM TRONCO";
                clubSmack.type = CardTypes.Attack;
                clubSmack.image = ImageStash.IS.UgaList[0];
                clubSmack.id = 0;
                clubSmack.minmax = 1616;
                clubSmack.damage = 2;
                clubSmack.isUnblockable = false;
                clubSmack.limit = 0;
                clubSmack.limitMax = 2;
                return clubSmack;

            case 1:
                Defense clubBlock = new Defense();
                clubBlock.name = "DEFESA COM TRONCO";
                clubBlock.type = CardTypes.Defense;
                clubBlock.image = ImageStash.IS.UgaList[1];
                clubBlock.id = 2;
                clubBlock.minmax = 0808;
                clubBlock.protection = 2;
                return clubBlock;

            case 2:
                Charge headScratch = new Charge();
                headScratch.name = "COÇAR A CABEÇA";
                headScratch.type = CardTypes.Charge;
                headScratch.image = ImageStash.IS.UgaList[2];
                headScratch.id = 2;
                headScratch.minmax = 1414;
                headScratch.charge = 1;
                headScratch.limit = 0;
                headScratch.limitMax = 3;
                return headScratch;

            case 3:
                Nullification ugaScream = new Nullification();
                ugaScream.name = "GRITO UGA";
                ugaScream.type = CardTypes.Nullification;
                ugaScream.image = ImageStash.IS.UgaList[3];
                ugaScream.text = "";
                ugaScream.id = 3;
                ugaScream.minmax = 0404;
                ugaScream.nullificationList = new CardTypes[4];
                ugaScream.nullificationList[0] = CardTypes.Charge; ugaScream.nullificationList[1] = CardTypes.Skill;
                ugaScream.nullificationList[2] = CardTypes.Ultimate; ugaScream.nullificationList[3] = CardTypes.Item;
                return ugaScream;

            case 4:
                return UgaDeck(3);


            case 5:
                BasicSkill boneSpear = new BasicSkill();
                boneSpear.name = "LANÇA DE OSSOS";
                boneSpear.type = CardTypes.Skill;
                boneSpear.image = ImageStash.IS.UgaList[4];
                boneSpear.text = "CAUSA 3a      .";
                boneSpear.id = 5;
                boneSpear.minmax = 1616;
                boneSpear.damage = 4;
                boneSpear.isUnblockable = false;
                boneSpear.protection = 0;
                boneSpear.charge = 0;
                return boneSpear;

            case 6:
                AutoHealSkill rawMeat = new AutoHealSkill();
                rawMeat.name = "CARNE CRUA";
                rawMeat.type = CardTypes.Skill;
                rawMeat.image = ImageStash.IS.UgaList[5];
                rawMeat.text = "CURA 2g   .";
                rawMeat.id = 6;
                rawMeat.minmax = 1414;
                rawMeat.damage = -2;
                rawMeat.isUnblockable = false;
                return rawMeat;

            case 7:
                BugaScream bugaScream = new BugaScream();
                bugaScream.name = "GRITO BUGA";
                bugaScream.type = CardTypes.Ultimate;
                bugaScream.image = ImageStash.IS.UgaList[6];
                bugaScream.text = "CAUSA 1a     . A CARTA DE ATAQUE DO \nUSUÁRIO AUMENTA DE DANO EM 1 \\nPERMANENTEMENTE.";
                bugaScream.id = 7;
                bugaScream.minmax = 1617;
                bugaScream.cost = 2;
                return bugaScream;

            default:
                return null;
        }
    }

    public Card TimothyDeck(int id)
    {
        switch (id)
        {
            case 0:
                Attack sonicGun = new Attack();
                sonicGun.name = "PISTOLA SÔNICA"; 
                sonicGun.type = CardTypes.Attack;
                sonicGun.image = ImageStash.IS.TimothyList[0];
                sonicGun.id = 0;
                sonicGun.minmax = 1616;
                sonicGun.damage = 2;
                sonicGun.isUnblockable = false;
                sonicGun.limit = 0;
                sonicGun.limitMax = 2;
                return sonicGun;

            case 1:
                Defense temporalShield = new Defense();
                temporalShield.name = "ESCUDO TEMPORAL";
                temporalShield.type = CardTypes.Defense;
                temporalShield.image = ImageStash.IS.TimothyList[1];
                temporalShield.id = 1;
                temporalShield.minmax = 0808;
                temporalShield.protection = 2;
                return temporalShield;

            case 2:
                WatchAdjustments watchAdjustments = new WatchAdjustments();
                watchAdjustments.name = "AJUSTES NO RELÓGIO";
                watchAdjustments.type = CardTypes.Charge;
                watchAdjustments.image = ImageStash.IS.TimothyList[2];
                watchAdjustments.text = "+1c   OU +2c , -2g   .";
                watchAdjustments.id = 2;
                watchAdjustments.minmax = 1414;
                return watchAdjustments;

            case 3:
                Nullification fourSecondsBack = new Nullification();
                fourSecondsBack.name = "QUATRO SEGUNDOS ATRÁS";
                fourSecondsBack.type = CardTypes.Nullification;
                fourSecondsBack.image = ImageStash.IS.TimothyList[3];
                fourSecondsBack.text = "";
                fourSecondsBack.id = 3;
                fourSecondsBack.minmax = 0404;
                fourSecondsBack.nullificationList = new CardTypes[4];
                fourSecondsBack.nullificationList[0] = CardTypes.Charge; fourSecondsBack.nullificationList[1] = CardTypes.Skill;
                fourSecondsBack.nullificationList[2] = CardTypes.Ultimate; fourSecondsBack.nullificationList[3] = CardTypes.Item;
                return fourSecondsBack;

            case 4:
                return TimothyDeck(3);

            case 5:
                TimeLock timeLock = new TimeLock();
                timeLock.name = "MARCA DO TEMPO";
                timeLock.type = CardTypes.Skill;
                timeLock.image = ImageStash.IS.TimothyList[4];
                timeLock.text = "MARCA O TEMPO PARA A ULTIMATE. \\n+1c .";
                timeLock.id = 5;
                timeLock.minmax = 0014;
                return timeLock;

            case 6:
                return TimothyDeck(5);

            case 7:
                DejaVu dejaVu = new DejaVu();
                dejaVu.name = "DÉJÀ VU";
                dejaVu.type = CardTypes.Skill;
                dejaVu.image = ImageStash.IS.TimothyList[5];
                dejaVu.text = "2b   . \\nPREVÊ A CARTA DO OPONENTE, \\nNO PRÓXIMO TURNO.";
                dejaVu.id = 7;
                dejaVu.minmax = 0817;
                return dejaVu;

            case 8:
                ChronosMachine chronosMachine = new ChronosMachine();
                chronosMachine.name = "MÁQUINA DE CRONOS";
                chronosMachine.type = CardTypes.Ultimate;
                chronosMachine.image = ImageStash.IS.TimothyList[6];
                chronosMachine.text = "O g      DE AMBOS JOGADORES \\nVOLTA PARA QUANDO A ÚLTIMA \\nMARCA DO TEMPO FOI JOGADA.";
                chronosMachine.id = 8;
                chronosMachine.minmax = 1212;
                chronosMachine.cost = 1;
                chronosMachine.isChronos = true;
                return chronosMachine;

            case 9:
                ChronosMachine backToFuture = new ChronosMachine();
                backToFuture.name = "DE VOLTA AO FUTURO";
                backToFuture.type = CardTypes.Ultimate;
                backToFuture.image = ImageStash.IS.TimothyList[7];
                backToFuture.text = "O g      DO JOGADOR DE SUA \\nESCOLHA VOLTA PARA QUANDO A \\nMÁQUINA DE CRONOS MAIS \\nRECENTE FOI USADA.";
                backToFuture.id = 9;
                backToFuture.minmax = 1212;
                backToFuture.cost = 2;
                backToFuture.isChronos = false;
                return backToFuture;

            default:
                return null;
        }
    }

    public Card HaroldDeck(int id)
    {
        switch (id)
        {
            case 0:
                Attack miniLaser = new Attack();
                miniLaser.name = "MINI-LASER";
                miniLaser.type = CardTypes.Attack;
                miniLaser.image = ImageStash.IS.UgaList[0];
                miniLaser.text = "PODE JOGAR d   , MESMO QUANDO \\nESTIVER NO LIMITE.";
                miniLaser.id = 0;
                miniLaser.minmax = 1616;
                miniLaser.damage = 2;
                miniLaser.isUnblockable = false;
                miniLaser.limit = 0;
                miniLaser.limitMax = 1;
                return miniLaser;

            case 1:
                Defense hideout = new Defense();
                hideout.name = "ESCONDERIJO";
                hideout.type = CardTypes.Defense;
                hideout.image = ImageStash.IS.UgaList[0];
                hideout.id = 1;
                hideout.minmax = 0808;
                hideout.protection = 1;
                return hideout;

            case 2:
                Charge machinations = new Charge();
                machinations.name = "PLANOS MALÉFICOS";
                machinations.type = CardTypes.Charge;
                machinations.image = ImageStash.IS.UgaList[0];
                machinations.id = 2;
                machinations.minmax = 1414;
                machinations.charge = 1;
                machinations.limit = 0;
                machinations.limitMax = 2;
                return machinations;

            case 3:
                Sabotage sabotageDmg = new Sabotage();
                sabotageDmg.name = "SABOTAGEM (DANO)";
                sabotageDmg.type = CardTypes.Nullification;
                sabotageDmg.image = ImageStash.IS.UgaList[0];
                sabotageDmg.text = "CAUSA 2a       SE ACERTAR.";
                sabotageDmg.id = 3;
                sabotageDmg.minmax = 0416;
                sabotageDmg.nullType = 0;
                sabotageDmg.damage = 2;
                sabotageDmg.nullificationList = new CardTypes[4];
                sabotageDmg.nullificationList[0] = CardTypes.Charge; sabotageDmg.nullificationList[1] = CardTypes.Skill;
                sabotageDmg.nullificationList[2] = CardTypes.Ultimate; sabotageDmg.nullificationList[3] = CardTypes.Item;
                return sabotageDmg;

            case 4:
                Sabotage sabotageDef = new Sabotage();
                sabotageDef.name = "SABOTAGEM (DEFESA)";
                sabotageDef.type = CardTypes.Nullification;
                sabotageDef.image = ImageStash.IS.UgaList[0];
                sabotageDef.text = "CONCEDE 2 b   , MESMO SE ERRAR.";
                sabotageDef.id = 4;
                sabotageDef.minmax = 0408;
                sabotageDef.nullType = 1;
                sabotageDef.protection = 2;
                sabotageDef.nullificationList = new CardTypes[4];
                sabotageDef.nullificationList[0] = CardTypes.Charge; sabotageDef.nullificationList[1] = CardTypes.Skill;
                sabotageDef.nullificationList[2] = CardTypes.Ultimate; sabotageDef.nullificationList[3] = CardTypes.Item;
                return sabotageDef;

            case 5:
                Sabotage sabotageNull = new Sabotage();
                sabotageNull.name = "SABOTAGEM (NULL)";
                sabotageNull.type = CardTypes.Nullification;
                sabotageNull.image = ImageStash.IS.UgaList[0];
                sabotageNull.text = "RECUPERA 1d     SE ACERTAR.";
                sabotageNull.id = 5;
                sabotageNull.minmax = 0419;
                sabotageNull.nullType = 2;
                sabotageNull.nullificationList = new CardTypes[4];
                sabotageNull.nullificationList[0] = CardTypes.Charge; sabotageNull.nullificationList[1] = CardTypes.Skill;
                sabotageNull.nullificationList[2] = CardTypes.Ultimate; sabotageNull.nullificationList[3] = CardTypes.Item;
                return sabotageNull;

            case 6:
                CloningMachine cloningMachine = new CloningMachine();
                cloningMachine.name = "MÁQUINA DE CLONAGEM";
                cloningMachine.type = CardTypes.Skill;
                cloningMachine.image = ImageStash.IS.UgaList[0];
                cloningMachine.text = "COPIA OS EFEITOS DA CARTA QUE O \\nINIMIGO JOGOU.";
                cloningMachine.id = 6;
                cloningMachine.minmax = 0019;
                return cloningMachine;

            case 7:
                return HaroldDeck(6);

            case 8:
                PerverseEngineering perverseEngineering = new PerverseEngineering();
                perverseEngineering.name = "ENGENHARIA MALIGNA";
                perverseEngineering.type = CardTypes.Skill;
                perverseEngineering.image = ImageStash.IS.UgaList[0];
                perverseEngineering.text = "RETIRA 1d     OU 1e     DO INIMIGO.";
                perverseEngineering.id = 8;
                perverseEngineering.minmax = 1818;
                return perverseEngineering;

            case 9:
                Catastrophe catastrophe = new Catastrophe();
                catastrophe.name = "MESTRE DO DESASTRE";
                catastrophe.type = CardTypes.Ultimate;
                catastrophe.image = ImageStash.IS.UgaList[0];
                catastrophe.text = "RETIRA 2c   E  1f      DO INIMIGO.";
                catastrophe.id = 9;
                catastrophe.minmax = 1018;
                catastrophe.cost = 1;
                return catastrophe;

            case 10:
                TemporalShieldTwo temporalShieldTwo = new TemporalShieldTwo();
                temporalShieldTwo.name = "ESCUDO TEMPORAL 2.0";
                temporalShieldTwo.type = CardTypes.Ultimate;
                temporalShieldTwo.image = ImageStash.IS.UgaList[0];
                temporalShieldTwo.text = "CONCEDE 1 b   . \\nPODE SER SACRIFICADA DURANTE A \\nREVELAÇÃO.";
                temporalShieldTwo.id = 10;
                temporalShieldTwo.minmax = 1212;
                temporalShieldTwo.cost = 1;
                temporalShieldTwo.isReaction = true;
                return temporalShieldTwo;

            default:
                return null;
        }
    }

    public Card YuriDeck(int id)
    {
        switch (id)
        {
            case 0:
                Attack sniperShot = new Attack();
                sniperShot.name = "TIRO DE SNIPER";
                sniperShot.type = CardTypes.Attack;
                sniperShot.image = ImageStash.IS.YuriList[0];
                sniperShot.id = 0;
                sniperShot.minmax = 1616;
                sniperShot.damage = 3;
                sniperShot.isUnblockable = false;
                sniperShot.limit = 0;
                sniperShot.limitMax = 1;
                return sniperShot;

            case 1:
                Defense cover = new Defense();
                cover.name = "COBERTURA";
                cover.type = CardTypes.Defense;
                cover.image = ImageStash.IS.YuriList[1];
                cover.id = 1;
                cover.minmax = 0808;
                cover.protection = 1;
                return cover;

            case 2:
                Charge reloadSniper = new Charge();
                reloadSniper.name = "RECARREGAR";
                reloadSniper.type = CardTypes.Charge;
                reloadSniper.image = ImageStash.IS.YuriList[2];
                reloadSniper.id = 2;
                reloadSniper.minmax = 1414;
                reloadSniper.charge = 1;
                reloadSniper.limit = 0;
                reloadSniper.limitMax = 3;
                return reloadSniper;

            case 3:
                Nullification supressionShot = new Nullification();
                supressionShot.name = "TIRO DE SUPRESSÃO";
                supressionShot.type = CardTypes.Nullification;
                supressionShot.image = ImageStash.IS.YuriList[3];
                supressionShot.text = "";
                supressionShot.id = 3;
                supressionShot.minmax = 0404;
                supressionShot.nullificationList = new CardTypes[4];
                supressionShot.nullificationList[0] = CardTypes.Charge; supressionShot.nullificationList[1] = CardTypes.Skill;
                supressionShot.nullificationList[2] = CardTypes.Ultimate; supressionShot.nullificationList[3] = CardTypes.Item;
                return supressionShot;

            case 4:
                Attack bulletBarrage = new Attack();
                bulletBarrage.name = "CHUVA DE BALAS";
                bulletBarrage.type = CardTypes.Skill;
                bulletBarrage.image = ImageStash.IS.YuriList[4];
                bulletBarrage.text = "CAUSA 4a     .\\nATIVA LIMITE DE ATAQUE.";
                bulletBarrage.id = 4;
                bulletBarrage.minmax = 1616;
                bulletBarrage.damage = 4;
                bulletBarrage.isUnblockable = false;
                bulletBarrage.limit = 0;
                bulletBarrage.limitMax = 1;
                return bulletBarrage;


            case 5:
                BasicSkill calculatedShot = new BasicSkill();
                calculatedShot.name = "TIRO CALCULADO";
                calculatedShot.type = CardTypes.Skill;
                calculatedShot.image = ImageStash.IS.YuriList[5];
                calculatedShot.text = "CAUSA 3a     .";
                calculatedShot.id = 5;
                calculatedShot.minmax = 1616;
                calculatedShot.damage = 3;
                calculatedShot.isUnblockable = false;
                calculatedShot.protection = 0;
                calculatedShot.charge = 0;
                return calculatedShot;

            case 6:
                SideEffectSkill weakSpot = new SideEffectSkill();
                weakSpot.name = "PONTO FRACO";
                weakSpot.type = CardTypes.Skill;
                weakSpot.image = ImageStash.IS.YuriList[6];
                weakSpot.text = "DANO CAUSADO PRÓXIMO TURNO É \\nIMBLOQUEÁVEL.";
                weakSpot.id = 6;
                weakSpot.minmax = 1717;
                weakSpot.sideEffect = 0;
                weakSpot.duration = 2;
                return weakSpot;

            case 7:
                Dexterity dexterity = new Dexterity();
                dexterity.name = "DESTREZA";
                dexterity.type = CardTypes.Ultimate;
                dexterity.image = ImageStash.IS.YuriList[7];
                dexterity.text = "RECUPERA 1 e     OU 1 d   .";
                dexterity.id = 7;
                dexterity.minmax = 1818;
                dexterity.cost = 1;
                return dexterity;

            case 8:
                BasicSkill vodka = new BasicSkill();
                vodka.name = "VODKA";
                vodka.type = CardTypes.Passive;
                vodka.image = ImageStash.IS.YuriList[8];
                vodka.text = "-1g      QUANDO ANULADO.";
                vodka.id = 8;
                vodka.minmax = 1818;
                return vodka;

            default:
                return null;
        }
    }
}
