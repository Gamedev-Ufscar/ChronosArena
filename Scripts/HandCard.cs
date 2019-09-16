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
    public HandCard(Card card, Deck deck) : base(card)
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

	public void SetCard(Card card) {
		this.card = card;
	}

    // Getter
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

    // Position Manipulators
    public void HoldCard()
    {
        // Control offset
        transform.position = new Vector3(center.x, center.y) + Input.mousePosition;
        center = Vector3.Lerp(center, new Vector3(0f, 0f, 0f), 0.1f);
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
        if (outOfHand && !isReaction)
        {
            deck.UnleashedCard(this);
        }

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
