using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UltimateCard : UICard, IPointerClickHandler, IPointerExitHandler, IPointerEnterHandler
{
    private Card card;
    private UltiArea ultiArea;
    private bool playable = false;
    private bool bought = false;

    private int cardID;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    

    // Setter
    public void SetPlayable(bool playable)
    {
        this.playable = playable;
    }

    public void SetBought(bool bought)
    {
        this.bought = bought;
    }

    public void SetID(int cardID)
    {
        this.cardID = cardID;
    }

    public void SetUltiArea(UltiArea ultiArea)
    {
        this.ultiArea = ultiArea;
    }


    // Getter 

    public int GetID()
    {
        return cardID;
    }

    public bool GetBought()
    {
        return bought;
    }

    // On Pointer
    public new void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        if (!bought) ChangeColor(0.8f);

        if (!ultiArea.GetHoldingCard())
        {
            ultiArea.BeingHighlighted(cardID);
        }
    }

    public new void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        if (!bought) ChangeColor(0.6f);

        ultiArea.StopHighlighted(cardID);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (playable && !bought)
        {
            bought = true;
            ChangeColor(1f);

        } else
        {
            bought = false;
            ChangeColor(0.8f);

        }
        ultiArea.SendUltiPurchase(cardID, bought);
    }
}
