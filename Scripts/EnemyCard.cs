using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EnemyCard : DeckCard, IPointerExitHandler, IPointerEnterHandler
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
        //if (!GetBeingHeld() || GetIsReaction()) {
            MoveCard();
        //}
    }

    public new void SetupCard(Card card)
    {
        SetCard(card);
    }

    public void EnemyHold(Vector2 newPosition)
    {
        //Debug.Log("being held true");
        SetBeingHeld(true);
        OnHover(Constants.cardBigSize, Constants.cardRiseHeight);
        ChangePosition(newPosition);
    }

    public void EnemyRelease()
    {
        //Debug.Log("being held false");
        SetBeingHeld(false);
        OutHover(1f, Constants.cardRiseHeight);
        UpdateCardPosition();
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
