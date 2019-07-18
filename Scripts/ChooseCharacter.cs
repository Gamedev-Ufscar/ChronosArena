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
    public int hero = -1;
    public int sideListSize = 0;
    public int handSize;
    public Sprite profile;
    public HermesScript hermesScript;

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
                if (hermesScript.hero == -1) {
                    hermesScript.hero = hero;
                    hermesScript.sideListSize = sideListSize;
                    hermesScript.handSize = handSize;
                    hermesScript.profile = profile;
                    GameOverseer.GO.myHero = hero;
                    selectionMode = 1;
                    GetComponent<Image>().color = new Color(1f, 1f, 1f);
                    Debug.Log("Selected hero!");
                } else if (hermesScript.hero == hero) {
                    hermesScript.hero = -1;
                    GameOverseer.GO.myHero = -1;
                    selectionMode = 0;
                    GetComponent<Image>().color = new Color(0.8f, 0.8f, 0.8f);
                    Debug.Log("Unselected hero!");
                }
            }
        }

        if (GameOverseer.GO.enemyHero == hero) {
            hermesScript.enemyHero = hero;
            hermesScript.enemySideListSize = sideListSize;
            hermesScript.enemyHandSize = handSize;
            hermesScript.enemyProfile = profile;
            selectionMode = 2;
            red = 0.2f;
            GetComponent<Image>().color = new Color(GetComponent<Image>().color.r, GetComponent<Image>().color.r - red, GetComponent<Image>().color.r - red);
        } else if (selectionMode == 2) {
            hermesScript.enemyHero = -1;
            red = 0f;
            selectionMode = 0;
            GetComponent<Image>().color = new Color(GetComponent<Image>().color.r, GetComponent<Image>().color.r, GetComponent<Image>().color.r);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOver = true;
        if (selectionMode != 1) {
            GetComponent<Image>().color = new Color(0.8f, 0.8f - red, 0.8f - red);
            transform.localScale = new Vector3(1.1f, 1.1f);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOver = false;
        if (selectionMode != 1) {
            GetComponent<Image>().color = new Color(0.55f, 0.55f - red, 0.55f - red);
            transform.localScale = new Vector3(1f, 1f);
        }
    }
}
