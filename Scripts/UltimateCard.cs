using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UltimateCard : UICard, IPointerClickHandler, IPointerExitHandler, IPointerEnterHandler
{
    private Card card;
    private UltiArea ultiArea;

    private bool bought = false;
    private bool darkened = false;

    private int cardID;
    private int tempIndex;

    // Start is called before the first frame update
    void Start()
    {
        bought = false;
        darkened = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Control position
        MoveCard();
    }
    

    // Setter
    public void SetDarkened(bool darkened)
    {
        this.darkened = darkened;
        UpdateColor();
    }

    public void SetBought(bool bought)
    {
        this.bought = bought;

        UpdateColor();
    }

    public void SetID(int cardID)
    {
        this.cardID = cardID;
    }

    public void SetUltiArea(UltiArea ultiArea)
    {
        this.ultiArea = ultiArea;
    }

    // Color
    private void UpdateColor()
    {
        if (GetDarkened())
        {
            ChangeColor(0.3f);
        } else
        {
            if (GetBought())
            {
                ChangeColor(1f);
            } else
            {
                if (GetPointerOver())
                {
                    ChangeColor(0.8f);
                } else
                {
                    ChangeColor(0.6f);
                }
            }
        }
    }

    // Getter 

    public int GetID()
    {
        return cardID;
    }

    public int GetTempIndex()
    {
        return tempIndex;
    }

    public bool GetBought()
    {
        return bought;
    }

    public bool GetDarkened()
    {
        return darkened;
    }

    // Setter

    public void SetTempIndex(int tempIndex)
    {
        this.tempIndex = tempIndex;
    }

    // Hovering
    public new void OnHover()
    {
        ultiArea.RevealArea();
        base.OnHover();
        ultiArea.SetSibling(true);
        Debug.Log("Hover Ulti");
        UpdateColor();

    }

    public new void OutHover()
    {
        base.OutHover();
        ultiArea.SetSibling(false);
        ultiArea.HideArea();
        Debug.Log("Out Hover Ulti");
        UpdateColor();

    }

    public new void OnPointerEnter(PointerEventData eventData)
    {
        OnHover();
    }

    public new void OnPointerExit(PointerEventData eventData)
    {
        OutHover();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ultiArea.UltiBuy(this);

        UpdateColor();
    }
}
