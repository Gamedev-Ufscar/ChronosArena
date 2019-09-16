using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UICard : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    private bool pointerOver = false;
    private Vector3 targetPosition;
    private float scale;
    private float color;
    private int category;
    private Card card;

    private int cardIndex;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Control Position
        ReturnCard();
    }

    // Constructor
    public UICard(Card card)
    {
        this.card = card;
        transform.GetChild(0).GetComponent<Text>().text = card.GetName();
        transform.GetChild(1).GetComponent<Text>().text = card.typeString(card.GetCardType());
        transform.GetChild(2).GetComponent<Text>().text = card.GetText().Replace("\\n", "\n");
        transform.GetChild(3).GetComponent<Text>().text = card.Value(1);
        transform.GetChild(4).GetComponent<Text>().text = card.Value(2);
        transform.GetChild(5).GetComponent<Text>().text = card.heroString(card.GetHero());
    }

    // Position Manipulators
    public void ReturnCard()
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

    public void SetCard(Card card)
    {
        this.card = card;
    }

    public void SetIndex(int cardIndex)
    {
        this.cardIndex = cardIndex;
    }

    public void RecedeIndex()
    {
        cardIndex--;
    }

    public void PushIndex()
    {
        cardIndex++;
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

    public bool GetIsActive()
    {
        return gameObject.activeInHierarchy;
    }

    public bool GetPointerOver()
    {
        return pointerOver;
    }

    public int GetIndex()
    {
        return cardIndex;
    }

    // On Pointer
    public void OnPointerEnter(PointerEventData eventData)
    {
        pointerOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        pointerOver = false;
        ChangeScale(1);
        SetAsFirstSibling();
        ChangeColor(0.6f);
    }




}
