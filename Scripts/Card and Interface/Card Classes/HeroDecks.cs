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

    public Card RobotoDeck(int index)
    {
        switch (index) {
            case 0:
                Attack robotoAttack = new Attack();
                robotoAttack.name = "Ataque Robotico";
                robotoAttack.type = CardTypes.Attack;
                robotoAttack.id = 0;
                robotoAttack.damage = 2;
                robotoAttack.limitMax = 2;
                return robotoAttack;

            case 1:
                Defense robotoDefense = new Defense();
                robotoDefense.name = "Defesa Robotica";
                robotoDefense.type = CardTypes.Defense;
                robotoDefense.id = 1;
                robotoDefense.protection = 2;
                return robotoDefense;

            case 2:
                Charge robotoCharge = new Charge();
                robotoCharge.name = "Carga Robotica";
                robotoCharge.type = CardTypes.Charge;
                robotoCharge.id = 2;
                robotoCharge.charge = 1;
                return robotoCharge;

            case 3:
                Nullification robotoNull = new Nullification();
                robotoNull.name = "Anulacao Robotica";
                robotoNull.type = CardTypes.Nullification;
                robotoNull.id = 3;
                robotoNull.nullificationList = new CardTypes[4];
                robotoNull.nullificationList[0] = CardTypes.Charge; robotoNull.nullificationList[1] = CardTypes.Skill;
                robotoNull.nullificationList[2] = CardTypes.Ultimate; robotoNull.nullificationList[3] = CardTypes.NeutralSkill;
                return robotoNull;

            default:
                return null;
        }
    }


}
