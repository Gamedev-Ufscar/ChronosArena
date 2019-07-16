using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroDecks : MonoBehaviour
{
    public static HeroDecks HD;
    public PlayerManager myManager;
    public PlayerManager enemyManager;


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
        HeroDecks.HD.enemyManager = GameObject.Find("Enemy Manager").GetComponent<PlayerManager>();
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
                supressionShot.name = "Anulacao Robotica";
                supressionShot.type = CardTypes.Nullification;
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
                weakSpot.id = 6;
                weakSpot.minmax = 0816;
                weakSpot.sideEffect = 0;
                weakSpot.duration = 1;
                return weakSpot;

            case 7:
                BasicSkill dexterity = new BasicSkill();
                dexterity.name = "Mecha Robotico";
                dexterity.type = CardTypes.Ultimate;
                dexterity.id = 6;
                dexterity.minmax = 0816;
                dexterity.cost = 2;
                dexterity.damage = 2;
                dexterity.isUnblockable = false;
                dexterity.protection = 2;
                dexterity.charge = 0;
                return dexterity;

            default:
                return null;
        }
    }
}
