using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EnemyCard : DeckCard
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
        if (!beingHeld || isReaction) {
            MoveCard();
        }
    }

    public void EnemyChangePosition(Vector2 newPosition)
    {
        SetBeingHeld(true);
        ChangePosition(newPosition);
    }

    public void EnemyStopMovement()
    {
        SetBeingHeld(false);
    }

    // Hover card
    public new void OnPointerEnter(PointerEventData eventData)
    {
        ChangeColor(1f);
    }

    // Stop Hover Card
    public new void OnPointerExit(PointerEventData eventData)
    {
        ChangeColor(0.6f);
    }
}
