using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HermesScript : MonoBehaviour
{
    public int hero = -1;
    public int sideListSize = 0;
    public int handSize;
    public int ultiCount;
    public Sprite profile;

    public int enemyHero = -1;
    public int enemySideListSize = 0;
    public int enemyHandSize;
    public int enemyUltiCount;
    public Sprite enemyProfile;

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
            HeroDecks.HD.myManager.ultiCount = ultiCount;
            GameObject.Find("Player Profile").GetComponent<ProfileScript>().profile = profile;

            // Enemyy Manager
            HeroDecks.HD.enemyManager.hero = enemyHero;
            HeroDecks.HD.enemyManager.sideList = new int[enemySideListSize];
            HeroDecks.HD.enemyManager.initialCardCount = enemyHandSize;
            HeroDecks.HD.enemyManager.ultiCount = enemyUltiCount;
            GameObject.Find("Enemy Profile").GetComponent<ProfileScript>().profile = enemyProfile;
        }
    }
}
