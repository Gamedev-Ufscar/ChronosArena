using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChooseCharacter : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    bool mouseOver = false;
    float red = 0f;
    int selectionMode = 0; // 0 = Not selected; 1 = My Hero; 2 = Enemy Hero
    public string heroName;
    public int hero = 200;
    public int sideListSize = 0;
    public int handSize;
    public int ultiCount;
    public Sprite profile;
    public HermesScript hermesScript;
    public Text heroTitle;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Image>().sprite = profile;
        GetComponent<Image>().color = new Color(0.55f, 0.55f, 0.55f);
    }

    // Update is called once per frame
    void Update()
    {
        if (mouseOver) {
            if (Input.GetMouseButtonDown(0) && selectionMode != 2) {
                // Selecting hero
                if (hermesScript.hero == 200) {
                    hermesScript.hero = hero;
                    hermesScript.sideListSize = sideListSize;
                    hermesScript.handSize = handSize;
                    hermesScript.profile = profile;
                    hermesScript.ultiCount = ultiCount;
                    GameOverseer.GO.myHero = hero;
                    selectionMode = 1;
                    GetComponent<Image>().color = new Color(1f, 1f, 1f);
                    Debug.Log("Selected hero!");

                // Unselecting hero
                } else if (hermesScript.hero == hero) {
                    hermesScript.hero = 200;
                    GameOverseer.GO.myConfirm = false;
                    GameOverseer.GO.myHero = 200;
                    selectionMode = 0;
                    GetComponent<Image>().color = new Color(0.8f, 0.8f, 0.8f);
                    Debug.Log("Unselected hero!");
                }
            }
        }

        if (GameOverseer.GO.enemyheroHover == hero) {
            red = 0.1f;
            transform.localScale = new Vector3(1.1f, 1.1f);
        } else {
            red = 0f;
            if (!mouseOver) transform.localScale = new Vector3(1f, 1f);
        }

        if (GameOverseer.GO.enemyHero == hero) {
            hermesScript.enemyHero = hero;
            hermesScript.enemySideListSize = sideListSize;
            hermesScript.enemyHandSize = handSize;
            hermesScript.enemyProfile = profile;
            hermesScript.enemyUltiCount = ultiCount;
            selectionMode = 2;
            red = 0.3f; 
        } else if (selectionMode == 2) {
            hermesScript.enemyHero = -1;
            red = 0f;
            selectionMode = 0;
        }

        GetComponent<Image>().color = new Color(GetComponent<Image>().color.r, GetComponent<Image>().color.r - red, GetComponent<Image>().color.r - red);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOver = true;
        GameOverseer.GO.myheroHover = hero;
        heroTitle.text = heroName;
        if (selectionMode != 1) {
            GetComponent<Image>().color = new Color(0.8f, 0.8f - red, 0.8f - red);
            transform.localScale = new Vector3(1.1f, 1.1f);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOver = false;
        GameOverseer.GO.myheroHover = 200;
        heroTitle.text = "???";
        if (selectionMode != 1) {
            GetComponent<Image>().color = new Color(0.55f, 0.55f - red, 0.55f - red);
            transform.localScale = new Vector3(1f, 1f);
        }
    }
}
