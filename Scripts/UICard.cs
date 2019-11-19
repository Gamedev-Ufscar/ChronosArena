using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UICard : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    private bool pointerOver = false;
    private bool interfaceActive = false;
    private Vector3 targetPosition;
    private float scale;
    private float color;
    private bool darkened = false;
    private bool isMobile = false;
    private int category;
    private Card card;

    private int cardIndex;

    // Start is called before the first frame update
    public void Start()
    {
        pointerOver = false;
        if (Application.platform == RuntimePlatform.Android)
        {
            isMobile = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isMobile && Input.GetMouseButtonDown(0) && pointerOver)
            OutHover(1f, Constants.cardRiseHeight(isMobile));

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
        }
        transform.GetChild(0).GetComponent<Text>().text = card.GetName();
        transform.GetChild(1).GetComponent<Text>().text = card.typeString(card.GetCardType());
        transform.GetChild(2).GetComponent<Text>().text = card.GetText().Replace("\\n", "\n");
        transform.GetChild(3).GetComponent<Text>().text = card.Value(1);
        transform.GetChild(4).GetComponent<Text>().text = card.Value(2);
        transform.GetChild(5).GetComponent<Text>().text = card.heroString(card.GetHero());

        ChangeScale(1f);
    }

    // Position Manipulators
    public void MoveCard()
    {
        if (!(Mathf.Approximately(transform.localPosition.x, targetPosition.x) && Mathf.Approximately(transform.localPosition.y, targetPosition.y)))
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
        if (transform.childCount >= 6)
            TransformColor((byte)((int)color*53.4f));
    }

    void TransformColor(byte color)
    {
        for (int i = 0; i <= 5; i++) {
            transform.GetChild(i).GetComponent<Text>().color = new Color32(color, color, color, 255);
        }
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

    public void SetDarkened(bool darkened)
    {
        SetDarkened(darkened, 0.6f);
    }

    public void SetDarkened(bool darkened, float color)
    {
        this.darkened = darkened;
        if (darkened)
            ChangeColor(0.3f);
        else
            ChangeColor(color);
    }

    public void SetInterfaceActive(bool interfaceActive)
    {
        this.interfaceActive = interfaceActive;
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

    public bool GetDarkened()
    {
        return darkened;
    }

    public bool GetInterfaceActive()
    {
        return interfaceActive;
    }

    public bool GetIsMobile()
    {
        return isMobile;
    }

    // On Pointer
    public void OnHover(float size, float rise)
    {
        pointerOver = true;
        ChangeScale(size);
        SetAsLastSibling();
        if (!GetDarkened())
        {
            ChangeColor(0.8f);
        }
        ChangePosition(targetPosition + new Vector3(0f, rise, 0f));
    }

    public void OutHover(float size, float rise)
    {
        pointerOver = false;
        ChangeScale(size);
        SetAsFirstSibling();
        if (!GetDarkened())
        {
            ChangeColor(0.6f);
        }
        ChangePosition(targetPosition + new Vector3(0f, -rise, 0f));
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isMobile && !(this is HoverCard))
            OnHover(Constants.cardBigSize(isMobile), Constants.cardRiseHeight(isMobile));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isMobile)
            OutHover(1f, Constants.cardRiseHeight(isMobile));
    }

    public void OnPointerClick(PointerEventData eventData)
    {
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isMobile && !pointerOver)
            OnHover(Constants.cardBigSize(isMobile), Constants.cardRiseHeight(isMobile));
    }

}
