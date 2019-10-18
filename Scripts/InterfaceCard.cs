using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InterfaceCard : UICard, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    private bool isClickable;
    private Interface interfface;
    private int option;

    public void SetInterface(Interface interfface)
    {
        this.interfface = interfface;
    }

    public void SetIsClickable(bool isClickable)
    {
        this.isClickable = isClickable;
    }

    public void SetOption(int option)
    {
        this.option = option;
    }

    public new void OnPointerEnter(PointerEventData eventData)
    {
        OnHover(7*Constants.cardBigSize/8, 1*Constants.cardRiseHeight/4);
        ChangeColor(1f);
    }

    public new void OnPointerExit(PointerEventData eventData)
    {
        OutHover(1f, Constants.cardRiseHeight / 4);
        ChangeColor(1f);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Clicked");
        if (isClickable)
        {
            Debug.Log("Clickable");
            interfface.Close(option);
        }
    }
}
