using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUIManager : MonoBehaviour
{
    public Slider myHealthbar;
    public Slider myChargebar;
    public Slider enemyHealthbar;
    public Slider enemyChargebar;
    public PlayerManager myManager;
    public PlayerManager enemyManager;

    public Text stateText;
    public Text cardText;
    public Text enemyCardText;
    public Text MHP;
    public Text MCHP;
    public Text EHP;
    public Text ECHP;
    public string hoveredCard = "";
    public string enemyRevealedCard = "";

    // Start is called before the first frame update
    void Start()
    {
        myManager = HeroDecks.HD.myManager;
        enemyManager = HeroDecks.HD.enemyManager;
    }

    // Update is called once per frame
    void Update()
    {
        stateText.text = " State: " + (int)GameOverseer.GO.state;
        cardText.text = " Current Card: " + hoveredCard;
        enemyCardText.text = "Enemy Card: " + enemyRevealedCard;
        MHP.text = "" + myManager.HP;
        MCHP.text = "" + myManager.Charge;
        EHP.text = "" + enemyManager.HP;
        ECHP.text = "" + enemyManager.Charge;

        myHealthbar.maxValue = 10;
        myHealthbar.minValue = 0;
        myHealthbar.value = myManager.HP;

        myChargebar.maxValue = 10;
        myChargebar.minValue = 0;
        myChargebar.value = myManager.Charge;

        enemyHealthbar.maxValue = 10;
        enemyHealthbar.minValue = 0;
        enemyHealthbar.value = enemyManager.HP;

        enemyChargebar.maxValue = 10;
        enemyChargebar.minValue = 0;
        enemyChargebar.value = enemyManager.Charge;
    }
}
