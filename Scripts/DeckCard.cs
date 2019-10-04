using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class DeckCard : UICard, IPointerExitHandler
{
    private Deck deck;
    private bool beingHeld = false;
    private bool isReaction = false;
    private bool outOfHand = false;

    private int cardID;

    private UltimateCard ultiCard;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    // Setter/Changer
    public new void ChangePosition(Vector2 newPosition)
    {
        if (!isReaction)
        {
            base.ChangePosition(newPosition);
        }
    }

    public void UpdateCardPosition()
    {
        deck.UpdateCardPosition(this);
    }

    public void SetBeingHeld(bool beingHeld)
    {
        this.beingHeld = beingHeld;
    }

    public void SetOutOfHand(bool outOfHand)
    {
        this.outOfHand = outOfHand;
    }

    public void SetIsReaction(bool isReaction)
    {
        this.isReaction = isReaction;
    }

    public void SetDeck(Deck deck)
    {
        this.deck = deck;
    }

    public void SetID(int cardID)
    {
        this.cardID = cardID;
    }

    public void SetUltiCard(UltimateCard ultiCard)
    {
        this.ultiCard = ultiCard;
    }

    // Getter
    public int GetID()
    {
        return cardID;
    }

    public bool GetIsReaction()
    {
        return isReaction;
    }

    public UltimateCard GetUltiCard()
    {
        return ultiCard;
    }

    public UltimateCard GetUltiCardScript()
    {
        return ultiCard.GetComponent<UltimateCard>();
    }

    public Deck GetDeck()
    {
        return deck;
    }

    public bool GetBeingHeld()
    {
        return beingHeld;
    }

    public bool GetOutOfHand()
    {
        return outOfHand;
    }

    public new void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);

        UpdateCardPosition();
    }
}
