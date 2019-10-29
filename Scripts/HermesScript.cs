using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HermesScript : MonoBehaviour
{
    private HeroEnum hero = HeroEnum.None;
    private int sideListSize = 0;
    private int handSize;
    private int ultiCount;
    private int passiveCount;
    private int sideCount;
    private List<CardTypes> attackDisableList = new List<CardTypes>();
    private Sprite profile;

    private HeroEnum enemyHero = HeroEnum.None;
    private int enemySideListSize = 0;
    private int enemyHandSize;
    private int enemyUltiCount;
    private int enemyPassiveCount;
    private int enemySideCount;
    private List<CardTypes> enemyAttackDisableList = new List<CardTypes>();
    private Sprite enemyProfile;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == (int)SceneList.Game)
        {
            if (hero == HeroEnum.None)
            {
                Destroy(gameObject);
            }
            else
            {
                GameOverseer gameOverseer = GameObject.Find("Game Overseer").GetComponent<GameOverseer>();

                gameOverseer.GetMyPlayer().CreatePlayer(hero, handSize, ultiCount, passiveCount, sideCount, attackDisableList, profile);
                gameOverseer.GetEnemyPlayer().CreatePlayer(enemyHero, enemyHandSize, enemyUltiCount, enemyPassiveCount, enemySideCount, enemyAttackDisableList, enemyProfile);
                Destroy(gameObject);
            }
        }
    }

    public void LoadHermes(HeroEnum hero, int sideListSize, int handSize, int ultiCount, int passiveCount, int sideCount, List<CardTypes> attackDisableList, Sprite profile)
    {
        this.hero = hero;
        this.sideListSize = sideListSize;
        this.handSize = handSize;
        this.ultiCount = ultiCount;
        this.passiveCount = passiveCount;
        this.sideCount = sideCount;
        this.attackDisableList = attackDisableList;
        this.profile = profile;
    }

    public void LoadEnemyHermes(HeroEnum hero, int sideListSize, int handSize, int ultiCount, int passiveCount, int sideCount, List<CardTypes> attackDisableList, Sprite profile)
    {
        this.enemyHero = hero;
        this.enemySideListSize = sideListSize;
        this.enemyHandSize = handSize;
        this.enemyUltiCount = ultiCount;
        this.enemyPassiveCount = passiveCount;
        this.enemySideCount = sideCount;
        this.enemyAttackDisableList = attackDisableList;
        this.enemyProfile = profile;
    }
}
