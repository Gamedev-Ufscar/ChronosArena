using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HermesScript : MonoBehaviour
{
    public int hero = -1;
    public int sideListSize = 0;
    public int handSize;
    public int enemyHero = -1;
    public int enemySideListSize = 0;
    public int enemyHandSize;

    private bool delivered = false;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 3 && delivered == false)
        {
            delivered = true;

            // My Manager
            HeroDecks.HD.myManager.hero = hero;
            HeroDecks.HD.myManager.sideList = new int[sideListSize];
            HeroDecks.HD.myManager.initialCardCount = handSize;

            // Enemyy Manager
            HeroDecks.HD.enemyManager.hero = enemyHero;
            HeroDecks.HD.enemyManager.sideList = new int[enemySideListSize];
            HeroDecks.HD.enemyManager.initialCardCount = enemyHandSize;
        }
    }
}
