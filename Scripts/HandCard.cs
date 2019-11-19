using UnityEngine;
using UnityEngine.EventSystems;

public class HandCard : DeckCard, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IPointerEnterHandler
{
    private Vector2 center = new Vector2(0f, 0f);

    // Update is called once per frame
    void Update()
    {
        // Control Position
        if (GetBeingHeld() && !GetIsReaction()) {
            HoldCard();
        } else {
            MoveCard();

            if (GetIsMobile() && Input.GetMouseButtonDown(0) && GetPointerOver())
            {
                OutHover(1f, Constants.cardRiseHeight(GetIsMobile()));
                OutHover();
            }
        }
    }

    // Position Manipulators
    public void HoldCard()
    {
        // Control offset
        Vector2 inputPosition;
        if (GetIsMobile() && Input.touchCount > 0)
            inputPosition = Input.GetTouch(0).position;
        else
            inputPosition = Input.mousePosition;

        SetPosition(new Vector2(center.x, center.y) + inputPosition);
        SetCenter(Vector2.Lerp(center, new Vector2(0f, 0f), 0.1f));

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

    public void OutHover()
    {
        GetDeck().SendCardPosition(null, new Vector2(0f, 0f));

        if (GetCard().GetTurnsTill() > 0 || GetDarkened())
        {
            ChangeColor(0.3f);
        }
    }

    // Hover card
    public new void OnPointerEnter(PointerEventData eventData)
    {
        if (!GetDeck().GetHoldingCard() && !GetIsMobile())
        {
            base.OnPointerEnter(eventData);
            GetDeck().SendCardPosition(this);

            if (GetCard().GetTurnsTill() > 0 || GetDarkened())
            {
                ChangeColor(0.3f);
            }
        }
    }

    // Stop Hover Card
    public new void OnPointerExit(PointerEventData eventData)
    {
        if (!GetIsMobile())
        {
            base.OnPointerExit(eventData);
            OutHover();
        }
    }

    // Hold card
    public new void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        GetDeck().SetHoldingCard(true);
        SetBeingHeld(true);
        SetCenter(transform.position - Input.mousePosition);
    }

    // Stop holding card, maybe summon card?
    public new void OnPointerUp(PointerEventData eventData)
    {
        if (GetBeingHeld())
        {
            base.OnPointerUp(eventData);

            // Unleashed card
            GetDeck().UnleashedCard(this);

            // Stop with centering
            UpdateCardPosition();
            ChangePosition((Vector3)GetTargetPosition() + new Vector3(0f, Constants.cardRiseHeight(GetIsMobile())));

            // Control variables and override
            GetDeck().SetHoldingCard(false);
            SetBeingHeld(false);
        }

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
