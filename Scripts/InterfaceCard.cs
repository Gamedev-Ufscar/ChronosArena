using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InterfaceCard : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    public bool option = false;
    bool zoomCard = false;
    InterfaceScript interfaceScript;
    [HideInInspector]
    public int index = 0;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Image>().color = new Color(0.6f, 0.6f, 0.6f);
        zoomCard = false;
    }

    // Update is called once per frame
    void Update()
    {
        interfaceScript = GetComponentInParent<InterfaceScript>();
        if (zoomCard) { 
            ZoomCard();
        } else { 
            ReturnCard();
        }
    }

    public void ZoomCard()
    {
        //0.8665, 1.177
        transform.localPosition = Vector2.Lerp(transform.localPosition + new Vector3(0f, HeroDecks.cardMoveUp, 0f),
                                                    interfaceScript.cardLocations[index], Time.deltaTime * 5f);
        transform.localScale = new Vector3(HeroDecks.cardZoomSize * 0.8665f, HeroDecks.cardZoomSize * 1.177f, 1f);
        gameObject.GetComponent<Canvas>().overrideSorting = true;

        // Choose Option
        if (Input.GetMouseButtonDown(0) && option) {
            interfaceScript.interfaceSignal = index;
            GameOverseer.GO.interfaceSignalSent = index;
        }

    }

    public void ReturnCard()
    {
        // Get back into position
        transform.localScale = new Vector3(0.8665f, 1.177f, 1f);
        transform.localPosition = Vector2.Lerp(transform.localPosition, interfaceScript.cardLocations[index], Time.deltaTime * 5f);

        // When card gets back into position, remove override
        if (transform.localPosition.x >= interfaceScript.cardLocations[index].x - 1f &&
            transform.localPosition.x <= interfaceScript.cardLocations[index].x + 1f &&
            transform.localPosition.y >= interfaceScript.cardLocations[index].y - 1f &&
            transform.localPosition.y <= interfaceScript.cardLocations[index].y + 1f) {
            gameObject.GetComponent<Canvas>().overrideSorting = false;
            gameObject.GetComponent<Canvas>().sortingOrder = 1;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponent<Image>().color = new Color(0.8f, 0.8f, 0.8f);
        zoomCard = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GetComponent<Image>().color = new Color(0.6f, 0.6f, 0.6f);
        zoomCard = false;
    }
}
