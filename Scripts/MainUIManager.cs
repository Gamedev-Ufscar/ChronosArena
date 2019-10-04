using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUIManager : MonoBehaviour
{
    //public Slider myHealthbar;
    [SerializeField]
    private GameOverseer gameOverseer;

    [SerializeField]
    private Sprite[] healthSprites = new Sprite[7];
    [SerializeField]
    private Sprite[] chargeSprites = new Sprite[7];

    [SerializeField]
    private Image[] myHealthbar = new Image[10];
    private int myHealthbarSprite = 6;

    [SerializeField]
    private Image[] enemyHealthbar = new Image[10];
    private int enemyHealthbarSprite = 6;

    [SerializeField]
    private Image[] myChargebar = new Image[10];
    private int myChargebarSprite = 6;

    [SerializeField]
    private Image[] enemyChargebar = new Image[10];
    private int enemyChargebarSprite = 6;

    [SerializeField]
    private Text stateText;
    [SerializeField]
    private Text cardText;
    [SerializeField]
    private Text enemyCardText;
    [SerializeField]
    private Text MHP;
    [SerializeField]
    private Text MCHP;
    [SerializeField]
    private Text EHP;
    [SerializeField]
    private Text ECHP;
    [SerializeField]
    private string hoveredCard = "";
    [SerializeField]
    private string enemyRevealedCard = "";

    [SerializeField]
    private GameObject playerConfirmedHue;
    [SerializeField]
    private GameObject enemyConfirmedHue;

    int myHealthbarValue = 10;
    int myChargebarValue = 0;
    int enemyHealthbarValue = 10;
    int enemyChargebarValue = 0;

    int playerHP;
    int playerCharge;
    int enemyHP;
    int enemyCharge;
    bool update = false;
    float time = 0f;

    // Start is called before the first frame update
    void Start()
    {
        myHealthbarValue = 10;
        myChargebarValue = 0;
        enemyHealthbarValue = 10;
        enemyChargebarValue = 0;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time > 0.05f)
        {
            time = 0f;

            BarAnimation(playerHP, ref myHealthbarValue, ref myHealthbarSprite, ref myHealthbar, healthSprites);
            BarAnimation(enemyHP, ref enemyHealthbarValue, ref enemyHealthbarSprite, ref enemyHealthbar, healthSprites);
            BarAnimation(playerCharge, ref myChargebarValue, ref myChargebarSprite, ref myChargebar, chargeSprites);
            BarAnimation(enemyCharge, ref enemyChargebarValue, ref enemyChargebarSprite, ref enemyChargebar, chargeSprites);
        }
    }

    // Hues
    public void SetPlayerHue(bool active)
    {
        // Confirmed Hue
        playerConfirmedHue.SetActive(active);
    }

    public void SetEnemyHue(bool active)
    {
        // Confirmed Hue
        enemyConfirmedHue.SetActive(active);
    }

    public void DebugText(string state)
    {
        // Text and Debug
        stateText.text = state;
        //cardText.text = " Current Card: " + hoveredCard;
        //enemyCardText.text = "Enemy Card: " + enemyRevealedCard;
    }

    // Bar stuff
    public void UpdateBar(Player player, Player enemy)
    {
        MHP.text = "" + player.GetHP();
        MCHP.text = "" + player.GetCharge();
        EHP.text = "" + enemy.GetHP();
        ECHP.text = "" + enemy.GetCharge();

        // Give player and enemy
        playerHP = player.GetHP();
        playerCharge = player.GetCharge();
        enemyHP = enemy.GetHP();
        enemyCharge = enemy.GetCharge();

        // Update
        update = true;
    }

    void BarAnimation(int playerP, ref int barValue, ref int barSprite, ref Image[] bar, Sprite[] barSprites)
    {
        // If value is balanced, get out
        if (playerP == barValue && barSprite == 6)
        {
            return;
        }


        if (playerP < barValue)
        { // If value decreases

            // Decrease within a sprite
            barSprite--;

            // Move on to the next sprite
            if (barSprite < 0)
            {
                barSprite = 6;
                barValue--;
            }

            // Update Sprite
            if (barValue > 0 && barValue <= 10)
                bar[barValue - 1].sprite = barSprites[barSprite];
        }
        else if (playerP >= barValue)
        { // If value increases

            // Increase within a sprite
            barSprite++;

            // Move on to the next sprite
            if (barSprite > 6)
            {
                barSprite = 0;
                barValue++;
            }

            // Update Sprite
            if (barValue > 0 && barValue <= 10)
                bar[barValue - 1].sprite = barSprites[barSprite];
        }
    }

    void enemyHealthbarAnimation(int enemyHP)
    {
        if (enemyHP < enemyHealthbarValue)
        { // If health decreases
            enemyHealthbarSprite--;
            if (enemyHealthbarSprite < 0)
            {
                enemyHealthbarSprite = 6;
                enemyHealthbarValue--;
            }

            if (enemyHealthbarValue > 0 && enemyHealthbarValue <= 10)
                enemyHealthbar[enemyHealthbarValue - 1].sprite = healthSprites[enemyHealthbarSprite];
        }
        else if (enemyHP >= enemyHealthbarValue)
        { // If health increases
            enemyHealthbarSprite++;
            if (enemyHealthbarSprite > 6)
            {
                enemyHealthbarSprite = 0;
                enemyHealthbarValue++;
            }

            if (enemyHealthbarValue > 0 && enemyHealthbarValue <= 10)
                enemyHealthbar[enemyHealthbarValue - 1].sprite = healthSprites[enemyHealthbarSprite];
        }
    }

    void enemyChargebarAnimation(int enemyCharge)
    {
        if (enemyCharge < enemyChargebarValue)
        { // If charge decreases
            enemyChargebarSprite--;
            if (enemyChargebarSprite < 0)
            {
                enemyChargebarSprite = 6;
                enemyChargebarValue--;
            }

            if (enemyChargebarValue > 0 && myChargebarValue <= 10)
                enemyChargebar[enemyChargebarValue - 1].sprite = chargeSprites[enemyChargebarSprite];
        }
        else if (enemyCharge >= enemyChargebarValue)
        { // If charge increases
            enemyChargebarSprite++;
            if (enemyChargebarSprite > 6)
            {
                enemyChargebarSprite = 0;
                enemyChargebarValue++;
            }

            if (enemyChargebarValue > 0 && myChargebarValue <= 10)
                enemyChargebar[enemyChargebarValue - 1].sprite = chargeSprites[enemyChargebarSprite];
        }
    }
}
