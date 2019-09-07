using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UICard : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    private Deck deck;
    //private bool pointerOver = false;
    private Vector3 targetPosition;
    private float scale;
    private float color;
    private int category;
    private Card card;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Control Position
        returnCard();
    }

    // Constructor
    public UICard(Card card, Deck deck)
    {
        this.card = card;
        this.deck = deck;
    }

    // Position Manipulators
    public void returnCard()
    {
        transform.localPosition = Vector2.Lerp(transform.localPosition, targetPosition, 0.1f);
    }

    // Setters/Changers
    public void ChangePosition(Vector2 newPosition)
    {
        targetPosition = newPosition;
    }

    public void ChangeScale(float scale)
    {
        gameObject.transform.localScale = new Vector3(scale * 0.8665f, scale * 1.177f);
    }

    public void ChangeColor(float color)
    {
        gameObject.GetComponent<Image>().color = new Color(color, color, color);
    }

    public void SetAsLastSibling()
    {
        transform.SetAsLastSibling();
    }

    public void SetAsFirstSibling()
    {
        transform.SetAsFirstSibling();
    }

    // Getters
    public Card GetCard()
    {
        return card;
    }

    public int GetCategory()
    {
        return category;
    }

    // On Pointer
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!deck.getHoldingCard())
        {
            ChangeScale(2);
            SetAsLastSibling();
            ChangeColor(1f);
            transform.localPosition = transform.localPosition + new Vector3(0f, 5f, 0f);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ChangeScale(1);
        SetAsFirstSibling();
        ChangeColor(0.6f);
    }




}
