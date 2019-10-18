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

    private int cardID;
    private int tempIndex;

    // Start is called before the first frame update
    void Start()
    {
        bought = false;
        SetDarkened(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Control position
        MoveCard();
    }
    

    // Setter
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

    public void SetTempIndex(int tempIndex)
    {
        this.tempIndex = tempIndex;
    }

    public void RecedeTempIndex()
    {
        tempIndex--;
    }

    public void PushTempIndex()
    {
        tempIndex++;
    }

    public new void SetDarkened(bool darkened)
    {
        base.SetDarkened(darkened);
        UpdateColor();
    }

    // Color
    public void UpdateColor()
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
    

    // Hovering
    public void OnHover()
    {
        ultiArea.RevealArea();
        base.OnHover(Constants.cardBigSize, Constants.cardRiseHeight);
        ultiArea.SetSibling(true);
        UpdateColor();

    }

    public void OutHover()
    {
        base.OutHover(1f, Constants.cardRiseHeight);
        ultiArea.SetSibling(false);
        ultiArea.HideArea();
        UpdateColor();

    }

    public new void OnPointerEnter(PointerEventData eventData)
    {
        if (!GetInterfaceActive())
            OnHover();
    }

    public new void OnPointerExit(PointerEventData eventData)
    {
        OutHover();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ultiArea.UltiBuy(this);
        Debug.Log(ultiArea.gameObject.name);
    }
}
