using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UltimateCard : UICard, IPointerClickHandler, IPointerExitHandler, IPointerEnterHandler
{
    private Card card;
    private Deck deck;
    private bool playable = false;
    private bool bought = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Constructor
    public UltimateCard(Card card, Deck deck) : base(card, deck)
    {
        this.card = card;
        this.deck = deck;
    }

    // Setter
    public void setPlayable(bool playable)
    {
        this.playable = playable;
    }

    // Getter 

    // On Pointer
    public new void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        if (!bought) changeColor(0.8f);
    }

    public new void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        if (!bought) changeColor(0.6f);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (playable && !bought)
        {
            bought = true;
            changeColor(1f);

        } else
        {
            bought = false;
            changeColor(0.8f);

        }
    }

    public void acquireCard(Card card)
    {
        deck.acquireUICard(card);
        bought = false;
        gameObject.SetActive(false);
    }
}
