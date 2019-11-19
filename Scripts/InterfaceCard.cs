using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InterfaceCard : UICard, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
{
    private bool isClickable;
    private Interface interfface;
    private int option;

    // Update is called once per frame
    void Update()
    {
        if (GetIsMobile() && Input.GetMouseButtonDown(0) && GetPointerOver())
            OutHover(1f, Constants.cardRiseHeight(GetIsMobile())/4);

        // Control Position
        MoveCard();
    }

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
        if (!GetIsMobile())
        {
            OnHover(7 * Constants.cardBigSize(false)/8, Constants.cardRiseHeight(false)/4);
            ChangeColor(1f);
        }
    }

    public new void OnPointerExit(PointerEventData eventData)
    {
        if (!GetIsMobile())
        {
            OutHover(1f, Constants.cardRiseHeight(false)/4);
            ChangeColor(1f);
        }
    }

    public new void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Clicked");
        if (isClickable)
        {
            Debug.Log("Clickable");
            interfface.Close(option);
        }
    }

    public new void OnPointerUp(PointerEventData eventData)
    {
        if (GetIsMobile() && !GetPointerOver())
            OnHover(Constants.cardBigSize(true), Constants.cardRiseHeight(true)/4);
    }
}
