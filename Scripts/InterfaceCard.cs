using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InterfaceCard : UICard
{
    private bool option;
    private Interface interfface;

    // Constructor
    public InterfaceCard(Card card, Interface interfface) : base(card)
    {
        this.interfface = interfface;
    }

    public void SetOption(bool option)
    {
        this.option = option;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (option)
        {
            interfface.Close(true);
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
