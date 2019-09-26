using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HermesScript : MonoBehaviour
{
    public HeroEnum hero = HeroEnum.None;
    public int sideListSize = 0;
    public int handSize;
    public int ultiCount;
    public int passiveCount;
    public List<CardTypes> attackDisableList = new List<CardTypes>();
    public Sprite profile;

    public HeroEnum enemyHero = HeroEnum.None;
    public int enemySideListSize = 0;
    public int enemyHandSize;
    public int enemyUltiCount;
    public int enemyPassiveCount;
    public List<CardTypes> enemyAttackDisableList = new List<CardTypes>();
    public Sprite enemyProfile;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            if (hero == HeroEnum.None)
            {
                Destroy(gameObject);
            }
            else
            {
                GameOverseer gameOverseer = GameObject.Find("Game Overseer").GetComponent<GameOverseer>();

                gameOverseer.GetMyPlayer().CreatePlayer(hero, handSize, ultiCount, passiveCount, attackDisableList, profile);
                gameOverseer.GetEnemyPlayer().CreatePlayer(enemyHero, enemyHandSize, enemyUltiCount, enemyPassiveCount, enemyAttackDisableList, enemyProfile);
                gameObject.SetActive(false);


                /*
                HeroDecks.HD.myManager.hero = hero;
                HeroDecks.HD.myManager.sideList = new int[sideListSize];
                HeroDecks.HD.myManager.initialCardCount = handSize;
                HeroDecks.HD.myManager.ultiCount = ultiCount;
                HeroDecks.HD.myManager.passiveCount = passiveCount;
                HeroDecks.HD.myManager.attackDisableList = attackDisableList;
                GameObject.Find("Player Profile").GetComponent<ProfileScript>().profile = profile;

                // Enemyy Manager
                HeroDecks.HD.enemyManager.hero = enemyHero;
                HeroDecks.HD.enemyManager.sideList = new int[enemySideListSize];
                HeroDecks.HD.enemyManager.initialCardCount = enemyHandSize;
                HeroDecks.HD.enemyManager.ultiCount = enemyUltiCount;
                HeroDecks.HD.enemyManager.passiveCount = enemyPassiveCount;
                HeroDecks.HD.enemyManager.attackDisableList = enemyAttackDisableList;
                GameObject.Find("Enemy Profile").GetComponent<ProfileScript>().profile = enemyProfile;
                */
            }
        }
    }

    public void LoadHermes(HeroEnum hero, int sideListSize, int handSize, int ultiCount, int passiveCount, List<CardTypes> attackDisableList, Sprite profile)
    {
        this.hero = hero;
        this.sideListSize = sideListSize;
        this.handSize = handSize;
        this.ultiCount = ultiCount;
        this.passiveCount = passiveCount;
        this.attackDisableList = attackDisableList;
        this.profile = profile;
    }

    public void LoadEnemyHermes(HeroEnum hero, int sideListSize, int handSize, int ultiCount, int passiveCount, List<CardTypes> attackDisableList, Sprite profile)
    {
        this.enemyHero = hero;
        this.enemySideListSize = sideListSize;
        this.enemyHandSize = handSize;
        this.enemyUltiCount = ultiCount;
        this.enemyPassiveCount = passiveCount;
        this.enemyAttackDisableList = attackDisableList;
        this.enemyProfile = profile;
    }
}
