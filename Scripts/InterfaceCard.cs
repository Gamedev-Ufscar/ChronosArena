using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InterfaceCard : UICard, IPointerDownHandler
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

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Clicked");
        if (isClickable)
        {
            Debug.Log("Clickable");
            interfface.Close(option);
        }
    }

    public new void OnPointerEnter(PointerEventData eventData)
    {
        ChangeScale(2);
        SetAsLastSibling();
        ChangeColor(1f);
        transform.localPosition = transform.localPosition + new Vector3(0f, 5f, 0f);
    }
}
