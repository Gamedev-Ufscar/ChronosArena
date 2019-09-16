/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// CRÉDITOS PARA LEO "RUTHER" DEL LAMA

public class PointerHandler : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler
{
    private bool canvasComp = false;
    private UICard cardH;

    void Start()
    {
        cardH = GetComponent<UICard>();
    }

    void Update()
    {
        if ((cardH != null && cardH.zoomCard))
        {
            //PutCanvasOnParent();
        } else {
            RemoveCanvasFromParent();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (cardH != null)
            GetComponent<Image>().color = new Color(0.6f, 0.6f, 0.6f);
            cardH.zoomCard = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (cardH != null)
            GetComponent<Image>().color = new Color(1f, 1f, 1f);
            cardH.zoomCard = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (cardH != null)
            cardH.moveCard = true;
    }

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        if (cardH != null)
            cardH.moveCard = false;
    }

    public void PutCanvasOnParent()
    {
        if (canvasComp)
            return;

        Canvas c = transform.parent.gameObject.AddComponent<Canvas>();
        c.overrideSorting = true;
        c.sortingOrder = 2;
        transform.parent.gameObject.AddComponent<GraphicRaycaster>();
        canvasComp = true;
    }

    public void RemoveCanvasFromParent()
    {
        if (!canvasComp)
            return;
        
        Destroy(transform.parent.gameObject.GetComponent<GraphicRaycaster>());
        Canvas c = transform.parent.gameObject.GetComponent<Canvas>();
        c.sortingOrder = 1;
        c.overrideSorting = false;
        Destroy(c);
        canvasComp = false;
    }
}*/
