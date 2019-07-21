using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ProfileScript : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    public Sprite profile;
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
        GetComponent<Image>().sprite = profile;
        if (mouseOver && Input.GetMouseButtonDown(0) && gameObject.name == "Player Profile"
             && HeroDecks.HD.interfaceScript.gameObject.activeInHierarchy == false) {
            HeroDecks.HD.myManager.myHand.GetComponent<DeckManager>().Shuffle();
        }

        if (mouseOver && Input.GetMouseButton(0) && gameObject.name == "Player Profile"
             && HeroDecks.HD.interfaceScript.gameObject.activeInHierarchy == false) {
            transform.localScale = new Vector3(1.1f, 1.1f);
        } else {
            transform.localScale = new Vector3(1.210431f, 1.272552f);
        }

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
