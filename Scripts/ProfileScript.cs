using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ProfileScript : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    public Sprite profile;
    public GameObject blackInterface;
    bool mouseOver = false;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Image>().color = new Color(0.8f, 0.8f, 0.8f);
        transform.localScale = new Vector3(1.210431f, 1.272552f);
    }

    // Update is called once per frame
    void Update()
    {
        // Set Image
        GetComponent<Image>().sprite = profile;

        if (mouseOver && HeroDecks.HD.interfaceScript.gameObject.activeInHierarchy == false) {
            // Shuffle
            if (Input.GetMouseButtonDown(0) && gameObject.name == "Player Profile") {
                HeroDecks.HD.myManager.myHand.GetComponent<DeckManager>().Shuffle();

            // Summary
            } else if (Input.GetMouseButtonDown(1)) {
                Summary();
            }
        }

        // Juicy button
        if (mouseOver && (Input.GetMouseButton(0) || Input.GetMouseButton(1)) && gameObject.name == "Player Profile"
             && HeroDecks.HD.interfaceScript.gameObject.activeInHierarchy == false) {
            transform.localScale = new Vector3(1.1f, 1.1f);
        } else {
            transform.localScale = new Vector3(1.210431f, 1.272552f);
        }

    }

    // Summary
    void Summary ()
    {
        // Establish manager to be examined
        PlayerManager thisManager;
        if (name == "Player Profile") { thisManager = HeroDecks.HD.myManager; }
        else { thisManager = HeroDecks.HD.enemyManager; }

        // Create both lists
        Sprite[] imageList = new Sprite[thisManager.initialCardCount + thisManager.ultiCount];
        string[] textList = new string[thisManager.initialCardCount + thisManager.ultiCount];

        // Go through Manager's card list and add to interface script
        HeroDecks.HD.interfaceScript.cardAmount = thisManager.cardList.Length;
        for (int i = 0; i < thisManager.initialCardCount + thisManager.ultiCount; i++)
        {
            if (thisManager.cardList[i] != null)
            {
                imageList[i] = thisManager.cardList[i].image;
                textList[i] = thisManager.cardList[i].text;
            }
        }

        // Final setup
        HeroDecks.HD.interfaceScript.optionMenu = false;
        HeroDecks.HD.interfaceScript.cardAmount = thisManager.initialCardCount + thisManager.ultiCount;
        HeroDecks.HD.interfaceScript.interfaceList = imageList;
        HeroDecks.HD.interfaceScript.textList = textList;
        HeroDecks.HD.interfaceScript.gameObject.SetActive(true);
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOver = true;
        if (gameObject.name == "Player Profile")
            GetComponent<Image>().color = new Color(1f, 1f, 1f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOver = false;
        GetComponent<Image>().color = new Color(0.8f, 0.8f, 0.8f);
    }
}
