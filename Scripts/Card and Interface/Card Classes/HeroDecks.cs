using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroDecks : MonoBehaviour
{
    public static HeroDecks HD;
    public Sprite[] imageList;
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

    public Card heroCard(int hero, int id)
    {
        switch (hero)
        {
            case 0:
                return UgaDeck(id);

            case 1:
                return TimothyDeck(id);

            case 21:
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
                robotoNull.nullificationList[2] = CardTypes.Ultimate; robotoNull.nullificationList[3] = CardTypes.NeutralSkill;
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
                clubSmack.name = "Pancada com Tronco";
                clubSmack.type = CardTypes.Attack;
                clubSmack.id = 0;
                clubSmack.minmax = 1616;
                clubSmack.damage = 2;
                clubSmack.isUnblockable = false;
                clubSmack.limit = 0;
                clubSmack.limitMax = 2;
                return clubSmack;

            case 1:
                Defense clubBlock = new Defense();
                clubBlock.name = "Defesa com Tronco";
                clubBlock.type = CardTypes.Defense;
                clubBlock.id = 2;
                clubBlock.minmax = 0808;
                clubBlock.protection = 2;
                return clubBlock;

            case 2:
                Charge headScratch = new Charge();
                headScratch.name = "Coçar a Cabeça";
                headScratch.type = CardTypes.Charge;
                headScratch.id = 2;
                headScratch.minmax = 1414;
                headScratch.charge = 1;
                headScratch.limit = 0;
                headScratch.limitMax = 3;
                return headScratch;

            case 3:
                Nullification ugaScream = new Nullification();
                ugaScream.name = "Grito Uga";
                ugaScream.type = CardTypes.Nullification;
                ugaScream.id = 3;
                ugaScream.minmax = 0404;
                ugaScream.nullificationList = new CardTypes[4];
                ugaScream.nullificationList[0] = CardTypes.Charge; ugaScream.nullificationList[1] = CardTypes.Skill;
                ugaScream.nullificationList[2] = CardTypes.Ultimate; ugaScream.nullificationList[3] = CardTypes.NeutralSkill;
                return ugaScream;

            case 4:
                return UgaDeck(3);


            case 5:
                BasicSkill boneSpear = new BasicSkill();
                boneSpear.name = "Lança de Ossos";
                boneSpear.type = CardTypes.Skill;
                boneSpear.id = 5;
                boneSpear.minmax = 1616;
                boneSpear.damage = 4;
                boneSpear.isUnblockable = false;
                boneSpear.protection = 0;
                boneSpear.charge = 0;
                return boneSpear;

            case 6:
                AutoHealSkill rawMeat = new AutoHealSkill();
                rawMeat.name = "Carne Crua";
                rawMeat.type = CardTypes.Skill;
                rawMeat.id = 6;
                rawMeat.minmax = 1414;
                rawMeat.damage = -2;
                rawMeat.isUnblockable = false;
                return rawMeat;

            case 7:
                BugaScream bugaScream = new BugaScream();
                bugaScream.name = "GRITO BUGA";
                bugaScream.type = CardTypes.Ultimate;
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
                sonicGun.name = "Pistola Sônica";
                sonicGun.type = CardTypes.Attack;
                sonicGun.id = 0;
                sonicGun.minmax = 1616;
                sonicGun.damage = 2;
                sonicGun.isUnblockable = false;
                sonicGun.limit = 0;
                sonicGun.limitMax = 2;
                return sonicGun;

            case 1:
                Defense temporalShield = new Defense();
                temporalShield.name = "Escudo Temporal";
                temporalShield.type = CardTypes.Defense;
                temporalShield.id = 2;
                temporalShield.minmax = 0808;
                temporalShield.protection = 2;
                return temporalShield;

            case 2:
                WatchAdjustments watchAdjustments = new WatchAdjustments();
                watchAdjustments.name = "Ajustes no Relógio";
                watchAdjustments.type = CardTypes.Charge;
                watchAdjustments.id = 2;
                watchAdjustments.minmax = 1414;
                return watchAdjustments;

            case 3:
                Nullification fourSecondsBack = new Nullification();
                fourSecondsBack.name = "Quatro Segundos Atrás";
                fourSecondsBack.type = CardTypes.Nullification;
                fourSecondsBack.id = 3;
                fourSecondsBack.minmax = 0404;
                fourSecondsBack.nullificationList = new CardTypes[4];
                fourSecondsBack.nullificationList[0] = CardTypes.Charge; fourSecondsBack.nullificationList[1] = CardTypes.Skill;
                fourSecondsBack.nullificationList[2] = CardTypes.Ultimate; fourSecondsBack.nullificationList[3] = CardTypes.NeutralSkill;
                return fourSecondsBack;

            case 4:
                return TimothyDeck(3);

            case 5:
                TimeLock timeLock = new TimeLock();
                timeLock.name = "Marca do Tempo";
                timeLock.type = CardTypes.Skill;
                timeLock.image = imageList[0];
                timeLock.id = 5;
                timeLock.minmax = 0014;
                return timeLock;

            case 6:
                return TimothyDeck(5);

            case 7:
                DejaVu dejaVu = new DejaVu();
                dejaVu.name = "Déjà Vu";
                dejaVu.type = CardTypes.Skill;
                dejaVu.id = 7;
                dejaVu.minmax = 0817;
                return dejaVu;

            case 8:
                ChronosMachine chronosMachine = new ChronosMachine();
                chronosMachine.name = "Máquina de Cronos";
                chronosMachine.type = CardTypes.Ultimate;
                chronosMachine.id = 8;
                chronosMachine.minmax = 1212;
                chronosMachine.cost = 1;
                chronosMachine.isChronos = true;
                return chronosMachine;

            case 9:
                ChronosMachine fragmentedReturn = new ChronosMachine();
                fragmentedReturn.name = "Retorno Fragmentado";
                fragmentedReturn.type = CardTypes.Ultimate;
                fragmentedReturn.id = 9;
                fragmentedReturn.minmax = 1212;
                fragmentedReturn.cost = 2;
                fragmentedReturn.isChronos = false;
                return fragmentedReturn;

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
                sniperShot.name = "Tiro de Sniper";
                sniperShot.type = CardTypes.Attack;
                sniperShot.id = 0;
                sniperShot.minmax = 1616;
                sniperShot.damage = 3;
                sniperShot.isUnblockable = false;
                sniperShot.limit = 0;
                sniperShot.limitMax = 1;
                return sniperShot;

            case 1:
                Defense cover = new Defense();
                cover.name = "Cobertura";
                cover.type = CardTypes.Defense;
                cover.id = 1;
                cover.minmax = 0808;
                cover.protection = 1;
                return cover;

            case 2:
                Charge reloadSniper = new Charge();
                reloadSniper.name = "Recarregar";
                reloadSniper.type = CardTypes.Charge;
                reloadSniper.id = 2;
                reloadSniper.minmax = 1414;
                reloadSniper.charge = 1;
                reloadSniper.limit = 0;
                reloadSniper.limitMax = 3;
                return reloadSniper;

            case 3:
                Nullification supressionShot = new Nullification();
                supressionShot.name = "Tiro de Supressão";
                supressionShot.type = CardTypes.Nullification;
                supressionShot.image = imageList[0];
                supressionShot.id = 3;
                supressionShot.minmax = 0404;
                supressionShot.nullificationList = new CardTypes[4];
                supressionShot.nullificationList[0] = CardTypes.Charge; supressionShot.nullificationList[1] = CardTypes.Skill;
                supressionShot.nullificationList[2] = CardTypes.Ultimate; supressionShot.nullificationList[3] = CardTypes.NeutralSkill;
                return supressionShot;

            case 4:
                Attack bulletBarrage = new Attack();
                bulletBarrage.name = "Chuva de Balas";
                bulletBarrage.type = CardTypes.Skill;
                bulletBarrage.image = imageList[0];
                bulletBarrage.id = 4;
                bulletBarrage.minmax = 1616;
                bulletBarrage.damage = 4;
                bulletBarrage.isUnblockable = false;
                bulletBarrage.limit = 0;
                bulletBarrage.limitMax = 1;
                return bulletBarrage;


            case 5:
                BasicSkill calculatedShot = new BasicSkill();
                calculatedShot.name = "Tiro Calculado";
                calculatedShot.type = CardTypes.Skill;
                calculatedShot.image = imageList[0];
                calculatedShot.id = 5;
                calculatedShot.minmax = 1616;
                calculatedShot.damage = 3;
                calculatedShot.isUnblockable = false;
                calculatedShot.protection = 0;
                calculatedShot.charge = 0;
                return calculatedShot;

            case 6:
                SideEffectSkill weakSpot = new SideEffectSkill();
                weakSpot.name = "Ponto Fraco";
                weakSpot.type = CardTypes.Skill;
                weakSpot.image = imageList[0];
                weakSpot.id = 6;
                weakSpot.minmax = 1717;
                weakSpot.sideEffect = 0;
                weakSpot.duration = 2;
                return weakSpot;

            case 7:
                Dexterity dexterity = new Dexterity();
                dexterity.name = "Destreza";
                dexterity.type = CardTypes.Ultimate;
                dexterity.id = 7;
                dexterity.minmax = 1818;
                dexterity.cost = 1;
                return dexterity;

            default:
                return null;
        }
    }
}
