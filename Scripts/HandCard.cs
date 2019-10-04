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
        SetPosition(new Vector3(center.x, center.y) + Input.mousePosition);
        SetCenter(Vector3.Lerp(center, new Vector3(0f, 0f, 0f), 0.1f));

        GetDeck().SendCardPosition(GetID(), new Vector2(transform.localPosition.x, transform.localPosition.y-2));
    }

    public new void ChangePosition(Vector2 newPosition)
    {
        base.ChangePosition(newPosition);
        //GetDeck().SendCardPosition(GetID(), newPosition);
    }

    public void ChangePosition(int id, Vector2 newPosition)
    {
        base.ChangePosition(newPosition);
        //GetDeck().SendCardPosition(id, newPosition);
    }

    public void SetCenter(Vector2 center)
    {
        this.center = center;
    }

    // Hover card
    public new void OnPointerEnter(PointerEventData eventData)
    {
        if (!GetDeck().GetHoldingCard())
        {
            base.OnPointerEnter(eventData);
            GetDeck().SendCardPosition(this);
        }
    }

    // Stop Hover Card
    public new void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);

        Debug.Log("exit card");
        GetDeck().SendCardPosition(null, new Vector2(0f, 0f));
    }

    // Hold card
    public void OnPointerDown(PointerEventData eventData)
    {
        GetDeck().SetHoldingCard(true);
        SetBeingHeld(true);
        SetCenter(transform.position - Input.mousePosition);
    }

    // Stop holding card, maybe summon card?
    public void OnPointerUp(PointerEventData eventData)
    {
        // Unleashed card
        GetDeck().UnleashedCard(this);

        // Stop with centering
        UpdateCardPosition();
        ChangePosition((Vector3)GetTargetPosition() + new Vector3(0f, Constants.cardRiseHeight));

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
