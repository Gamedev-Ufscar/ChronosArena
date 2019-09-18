/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChooseCharacter : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    public bool mouseOver = false;
    public float red = 0f;
    public int selectionMode = 0; // 0 = Not selected; 1 = My Hero; 2 = Enemy Hero
    public string heroName;
    public HeroEnum hero = HeroEnum.None;
    public int sideListSize = 0;
    public int handSize;
    public int ultiCount;
    public int passiveCount = 0;
    public List<CardTypes> attackDisableList = new List<CardTypes>();
    public Sprite profile;
    public HeroSelection heroSelection;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Image>().sprite = profile;
        GetComponent<Image>().color = new Color(0.55f, 0.55f, 0.55f);
    }

    // Update is called once per frame
    void Update()
    {
        // On Player Hover (Ignore if char already chosen)
        if (mouseOver && (GameOverseer.GO.myConfirm == false || heroSelection.hermesScript.hero == hero)) {
            if (GameOverseer.GO.myHero == HeroEnum.None || GameOverseer.GO.myHero == hero) {
                GameOverseer.GO.myHero = hero;
                GetComponentInParent<HeroSelection>().myTitle.text = heroName;
                GetComponentInParent<HeroSelection>().myPortrait.sprite = profile;
            }

            // Highlight button
            if (selectionMode != 1 && GameOverseer.GO.enemyHero != hero) {
                GetComponent<Image>().color = new Color(0.8f, 0.8f - red, 0.8f - red);
                transform.localScale = new Vector3(1.1f, 1.1f);
            }

            // Juicy feeling of pressing a button
            if (Input.GetMouseButton(0) && selectionMode != 2) {
                transform.localScale = new Vector3(1f, 1f);
            } //else { transform.localScale = new Vector3(1.1f, 1.1f); }


            if (Input.GetMouseButtonUp(0) && selectionMode != 2) {
                // Selecting hero
                if (heroSelection.hermesScript.hero != hero) {
                    heroSelection.hermesScript.hero = hero;
                    heroSelection.hermesScript.sideListSize = sideListSize;
                    heroSelection.hermesScript.handSize = handSize;
                    heroSelection.hermesScript.profile = profile;
                    heroSelection.hermesScript.ultiCount = ultiCount;
                    heroSelection.hermesScript.passiveCount = passiveCount;
                    heroSelection.hermesScript.attackDisableList = attackDisableList;
                    GameOverseer.GO.myHero = hero;
                    selectionMode = 1;
                    GetComponent<Image>().color = new Color(1f, 1f, 1f);
                    Debug.Log("Selected hero!");

                // Explicitly unselecting hero
                } else if (heroSelection.hermesScript.hero == hero) {
                    heroSelection.hermesScript.hero = HeroEnum.None;
                    GameOverseer.GO.myConfirm = false;
                    GameOverseer.GO.myHero = HeroEnum.None;
                    selectionMode = 0;
                    GetComponent<Image>().color = new Color(0.8f, 0.8f, 0.8f);
                    Debug.Log("Unselected hero!");
                }
            }

        // Out Player Hover
        } else if (GameOverseer.GO.myheroHover == hero) {
            GameOverseer.GO.myHero = HeroEnum.None;

            // Change portrait
            if (GameOverseer.GO.myHero == HeroEnum.None) {
                GetComponentInParent<HeroSelection>().myTitle.text = "???";
                GetComponentInParent<HeroSelection>().myPortrait.sprite = GetComponentInParent<HeroSelection>().noHero;
            }

            // Remove highlight
            if (selectionMode != 1) {
                GetComponent<Image>().color = new Color(0.55f, 0.55f - red, 0.55f - red);
                transform.localScale = new Vector3(1f, 1f);
            }
        } else {
            // Automatically unselecting hero
            if (heroSelection.hermesScript.hero != hero) {
                selectionMode = 0;
                GetComponent<Image>().color = new Color(0.8f, 0.8f, 0.8f);
            }
        }
        

        GetComponent<Image>().color = new Color(GetComponent<Image>().color.r, GetComponent<Image>().color.r - red, GetComponent<Image>().color.r - red);
    }

    // On Hover
    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOver = true;
        
    }

    // Out Hover
    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOver = false;
        
    }

    // Events
    public void eventChangeColor()
    {

    }

    public void eventEnemyChosen()
    {
        heroSelection.hermesScript.enemyHero = hero;
        heroSelection.hermesScript.enemySideListSize = sideListSize;
        heroSelection.hermesScript.enemyHandSize = handSize;
        heroSelection.hermesScript.enemyProfile = profile;
        heroSelection.hermesScript.enemyUltiCount = ultiCount;
        heroSelection.hermesScript.enemyPassiveCount = passiveCount;
        heroSelection.hermesScript.enemyAttackDisableList = attackDisableList;
        selectionMode = 2;
        red = 0.3f;
    }

    public void eventEnemyDeselect()
    {
        heroSelection.hermesScript.enemyHero = HeroEnum.None;
        red = 0f;
        selectionMode = 0;
    }

}*/
