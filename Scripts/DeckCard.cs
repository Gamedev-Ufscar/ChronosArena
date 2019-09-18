using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class DeckCard : UICard
{
    private Card card;
    private Deck deck;
    private bool beingHeld = false;
    private bool isReaction = false;
    private bool outOfHand = false;

    private int cardID;

    private GameObject ultiCard;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    // Constructor
    public DeckCard(Card card, Deck deck) : base(card)
    {
        this.deck = deck;
        this.card = card;
    }

    // Setter/Changer
    public new void ChangePosition(Vector2 newPosition)
    {
        if (!isReaction)
        {
            base.ChangePosition(newPosition);
        }
    }

    public void SetIsReaction(bool isReaction)
    {
        this.isReaction = isReaction;
    }

    public void SetID(int cardID)
    {
        this.cardID = cardID;
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

    public GameObject GetUltiCard()
    {
        return ultiCard;
    }

    public UltimateCard GetUltiCardScript()
    {
        return ultiCard.GetComponent<UltimateCard>();
    }
}
