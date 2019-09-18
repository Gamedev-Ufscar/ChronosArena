using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HandCard : DeckCard, IPointerDownHandler, IPointerUpHandler
{
    private Card card;
    private Deck deck;
    private bool beingHeld = false;
    private Vector2 center = new Vector2(0f, 0f);
    private bool isReaction = false;
    private bool outOfHand = false;

    private GameObject ultiCard;

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
            ReturnCard();
        }
    }

    // Constructor
    public HandCard(Card card, Deck deck) : base(card, deck)
    {
        this.card = card;
        this.deck = deck;
    }

    // Getter
    public bool GetOutOfHand()
    {
        return outOfHand;
    }

    // Position Manipulators
    public void HoldCard()
    {
        // Control offset
        ChangePosition((Vector2)(new Vector3(center.x, center.y) + Input.mousePosition));
        //transform.position = new Vector3(center.x, center.y) + Input.mousePosition;
        center = Vector3.Lerp(center, new Vector3(0f, 0f, 0f), 0.1f);
    }

    public void ChangePosition(int id, Vector2 newPosition, Vector2 localNewPosition)
    {
        base.ChangePosition(newPosition);
        deck.SendCardPosition(id, newPosition, localNewPosition);
    }

    public new void OnPointerEnter(PointerEventData eventData)
    {
        if (!deck.GetHoldingCard())
        {
            ChangeScale(2);
            SetAsLastSibling();
            ChangeColor(1f);
            transform.localPosition = transform.localPosition + new Vector3(0f, 5f, 0f);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Control variables and override
        deck.SetHoldingCard(true);
        beingHeld = true;
        center = transform.position - Input.mousePosition;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Unleashed card
        deck.UnleashedCard(this);

        // Control variables and override
        deck.SetHoldingCard(false);
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
