using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HandCard : UICard, IPointerDownHandler, IPointerUpHandler
{
    private Card card;
    private Deck deck;
    private bool beingHeld = false;
    private Vector2 center = new Vector2(0f, 0f);
    private bool isReaction = false;
    private bool outOfHand = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Control Position
        if (beingHeld && !isReaction) {
            HoldCard();

        } else {
            returnCard();
        }
    }

    // Constructor
    public HandCard(Card card, Deck deck) : base(card, deck)
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

    // Getter
    public bool GetIsReaction()
    {
        return isReaction;
    }


    // Position Manipulators
    public void HoldCard()
    {
        // Control offset
        transform.position = new Vector3(center.x, center.y) + Input.mousePosition;
        center = Vector3.Lerp(center, new Vector3(0f, 0f, 0f), 0.1f);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Control variables and override
        deck.setHoldingCard(true);
        beingHeld = true;
        center = transform.position - Input.mousePosition;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Unleashed card
        if (outOfHand && !isReaction)
        {
            deck.UnleashedCard(this);
        }

        // Control variables and override
        deck.setHoldingCard(false);
        beingHeld = false;
    }

    public void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Trigger Exit");
        if (other.tag == "Hand")
        {
            outOfHand = false;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        //Debug.Log("Trigger Exit");
        if (other.tag == "Hand")
        {
            outOfHand = true;
        }
    }
}
