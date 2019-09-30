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
        MoveCard();
    }

    // Constructor
    public void SetupCard(Card card)
    {
        SetCard(card);
        if (!(this is EnemyCard))
        {
            gameObject.GetComponent<Image>().sprite = card.GetImage();
            transform.GetChild(0).GetComponent<Text>().text = card.GetName();
            transform.GetChild(1).GetComponent<Text>().text = card.typeString(card.GetCardType());
            transform.GetChild(2).GetComponent<Text>().text = card.GetText().Replace("\\n", "\n");
            transform.GetChild(3).GetComponent<Text>().text = card.Value(1);
            transform.GetChild(4).GetComponent<Text>().text = card.Value(2);
            transform.GetChild(5).GetComponent<Text>().text = card.heroString(card.GetHero());
        }
    }

    // Position Manipulators
    public void MoveCard()
    {
        transform.localPosition = Vector2.Lerp(transform.localPosition, targetPosition, 0.1f);
    }

    // Setters/Changers
    public void ChangePosition(Vector2 newPosition)
    {
        targetPosition = newPosition;
    }

    public void SetPosition(Vector2 newPosition)
    {
        transform.position = newPosition;
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

    public Vector2 GetTargetPosition()
    {
        return targetPosition;
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
    public void OnHover()
    {
        pointerOver = true;
        Debug.Log("Pointer Enter!");
        ChangeScale(Constants.cardBigSize);
        SetAsLastSibling();
        ChangeColor(1f);
        ChangePosition(targetPosition + new Vector3(0f, Constants.cardRiseHeight, 0f));
    }

    public void OutHover()
    {
        pointerOver = false;
        ChangeScale(1);
        SetAsFirstSibling();
        ChangeColor(0.6f);
        ChangePosition(targetPosition + new Vector3(0f, -Constants.cardRiseHeight, 0f));
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnHover();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OutHover();
    }




}
