using UnityEngine;
using UnityEngine.EventSystems;

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
        if (!GetIsReaction())
        {
            SetBeingHeld(true);
            OnHover(Constants.cardBigSize(GetIsMobile()), Constants.cardRiseHeight(GetIsMobile()));
            ChangePosition(newPosition);
        }
    }

    public void EnemyRelease()
    {
        if (!GetIsReaction())
        {
            SetBeingHeld(false);
            OutHover(1f, Constants.cardRiseHeight(GetIsMobile()));

            UpdateCardPosition();
        }
    }

    // Hover card
    public new void OnPointerEnter(PointerEventData eventData)
    {
        ChangeColor(1f);
        if (GetIsReaction())
        {
            GetDeck().CreateCardReader(this);
        }
    }

    // Stop Hover Card
    public new void OnPointerExit(PointerEventData eventData)
    {
        ChangeColor(0.6f);
        if (GetIsReaction())
        {
            GetDeck().DestroyReader();
        }
    }
}
