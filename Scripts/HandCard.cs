using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HandCard : DeckCard, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IPointerEnterHandler
{
    private Vector2 center = new Vector2(0f, 0f);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Control Position
        if (GetBeingHeld() && !GetIsReaction()) {
            HoldCard();

        } else {
            MoveCard();
        }
    }

    // Position Manipulators
    public void HoldCard()
    {
        // Control offset
        ChangePosition((Vector2)(new Vector3(center.x, center.y) + Input.mousePosition));
        //transform.position = new Vector3(center.x, center.y) + Input.mousePosition;
        center = Vector3.Lerp(center, new Vector3(0f, 0f, 0f), 0.1f);
    }

    public new void ChangePosition(Vector2 newPosition)
    {
        base.ChangePosition(newPosition);
        GetDeck().SetCardPosition(GetID(), newPosition);
    }

    public void ChangePosition(int id, Vector2 newPosition)
    {
        base.ChangePosition(newPosition);
        GetDeck().SetCardPosition(id, newPosition);
    }


    // Hover card
    public new void OnPointerEnter(PointerEventData eventData)
    {
        if (!GetDeck().GetHoldingCard())
        {
            base.OnPointerEnter(eventData);
        }
    }

    // Stop Hover Card
    public new void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
    }

    // Hold card
    public void OnPointerDown(PointerEventData eventData)
    {
        GetDeck().SetHoldingCard(true);
        SetBeingHeld(true);
        center = transform.position - Input.mousePosition;
    }

    // Stop holding card, maybe summon card?
    public void OnPointerUp(PointerEventData eventData)
    {
        // Unleashed card
        GetDeck().UnleashedCard(this);

        // Control variables and override
        GetDeck().SetHoldingCard(false);
        SetBeingHeld(false);
    }

    public void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Trigger Exit");
        if (other.tag == "Hand")
        {
            SetOutOfHand(false);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        //Debug.Log("Trigger Exit");
        if (other.tag == "Hand")
        {
            SetOutOfHand(true);
        }
    }
}
