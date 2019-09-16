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
    private Player myPlayer;
    [SerializeField]
    private Player enemyPlayer;

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

    float time = 0f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // Text and Debug
        stateText.text = " State: " + (int)gameOverseer.GetState();
        cardText.text = " Current Card: " + hoveredCard;
        enemyCardText.text = "Enemy Card: " + enemyRevealedCard;
        MHP.text = "" + myPlayer.GetHP();
        MCHP.text = "" + myPlayer.GetCharge();
        EHP.text = "" + enemyPlayer.GetHP();
        ECHP.text = "" + enemyPlayer.GetCharge();

        // Confirmed Hue
        if (GameOverseer.GO.myConfirm) { playerConfirmedHue.SetActive(true); }
        else { playerConfirmedHue.SetActive(false); }

        if (GameOverseer.GO.enemyConfirm) { enemyConfirmedHue.SetActive(true); }
        else { enemyConfirmedHue.SetActive(false); }

        // Healthbar animation
        barAnimation();
    }

    void barAnimation()
    {
        time += Time.deltaTime;
        if (time > 0.05f) {
            time = 0f;

            myHealthbarAnimation();
            enemyHealthbarAnimation();
            myChargebarAnimation();
            enemyChargebarAnimation();
        }
    }

    void myHealthbarAnimation()
    {
        if (myManager.HP < myHealthbarValue) { // If health decreases
            myHealthbarSprite--;
            if (myHealthbarSprite < 0)
            {
                myHealthbarSprite = 6;
                myHealthbarValue--;
            }

            if (myHealthbarValue > 0 && myHealthbarValue <= 10)
                myHealthbar[myHealthbarValue - 1].sprite = healthSprites[myHealthbarSprite];
        }
        else if (myManager.HP >= myHealthbarValue) { // If health increases
            myHealthbarSprite++;
            if (myHealthbarSprite > 6)
            {
                myHealthbarSprite = 0;
                myHealthbarValue++;
            }

            if (myHealthbarValue > 0 && myHealthbarValue <= 10)
                myHealthbar[myHealthbarValue - 1].sprite = healthSprites[myHealthbarSprite];
        }
    }

    void enemyHealthbarAnimation()
    {
        if (enemyManager.HP < enemyHealthbarValue)
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
        else if (enemyManager.HP >= enemyHealthbarValue)
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

    void myChargebarAnimation()
    {
        if (myManager.Charge < myChargebarValue)
        { // If charge decreases
            myChargebarSprite--;
            if (myChargebarSprite < 0)
            {
                myChargebarSprite = 6;
                myChargebarValue--;
            }

            if (myChargebarValue > 0 && myChargebarValue <= 10)
                myChargebar[myChargebarValue - 1].sprite = chargeSprites[myChargebarSprite];
        }
        else if (myManager.Charge >= myChargebarValue)
        { // If charge increases
            myChargebarSprite++;
            if (myChargebarSprite > 6)
            {
                myChargebarSprite = 0;
                myChargebarValue++;
            }
            if (myChargebarValue > 0 && myChargebarValue <= 10)
                myChargebar[myChargebarValue - 1].sprite = chargeSprites[myChargebarSprite];
        }
    }

    void enemyChargebarAnimation()
    {
        if (enemyManager.Charge < enemyChargebarValue)
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
        else if (enemyManager.Charge >= enemyChargebarValue)
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
