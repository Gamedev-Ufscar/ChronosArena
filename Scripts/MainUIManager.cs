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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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
