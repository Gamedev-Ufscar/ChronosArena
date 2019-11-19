using UnityEngine;
using UnityEngine.EventSystems;

public class UltimateCard : UICard, IPointerDownHandler, IPointerExitHandler, IPointerEnterHandler, IPointerUpHandler
{
    private Card card;
    private UltiArea ultiArea;

    private bool bought = false;

    private int cardID;
    private int tempIndex;

    // Start is called before the first frame update
    new void Start()
    {
        bought = false;
        SetDarkened(false);

        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        // Control position
        MoveCard();

        if (GetIsMobile() && Input.GetMouseButtonDown(0) && GetPointerOver())
            if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                OutHover();
    }
    

    // Setter
    public void SetBought(bool bought)
    {
        this.bought = bought;

        UpdateColor();
    }

    public void SetID(int cardID)
    {
        this.cardID = cardID;
    }

    public void SetUltiArea(UltiArea ultiArea)
    {
        this.ultiArea = ultiArea;
    }

    public void SetTempIndex(int tempIndex)
    {
        this.tempIndex = tempIndex;
    }

    public void RecedeTempIndex()
    {
        tempIndex--;
    }

    public void PushTempIndex()
    {
        tempIndex++;
    }

    public new void SetDarkened(bool darkened)
    {
        base.SetDarkened(darkened);
        UpdateColor();
    }

    // Color
    public void UpdateColor()
    {
        if (GetDarkened())
        {
            ChangeColor(0.3f);
        } else
        {
            if (GetBought())
            {
                ChangeColor(1f);
            } else
            {
                if (GetPointerOver())
                {
                    ChangeColor(0.8f);
                } else
                {
                    ChangeColor(0.6f);
                }
            }
        }
    }

    // Getter 

    public int GetID()
    {
        return cardID;
    }

    public int GetTempIndex()
    {
        return tempIndex;
    }

    public bool GetBought()
    {
        return bought;
    }
    

    // Hovering
    public void OnHover()
    {
        ultiArea.RevealArea();
        base.OnHover(Constants.cardBigSize(GetIsMobile()), Constants.cardRiseHeight(GetIsMobile()));
        ultiArea.SetSibling(true);
        UpdateColor();
    }

    public void OutHover()
    {
        base.OutHover(1f, Constants.cardRiseHeight(GetIsMobile()));
        ultiArea.SetSibling(false);
        ultiArea.HideArea();
        UpdateColor();
    }

    public new void OnPointerEnter(PointerEventData eventData)
    {
        if (!GetIsMobile())
            OnHover();
    }

    public new void OnPointerExit(PointerEventData eventData)
    {
        if (!GetIsMobile())
            OutHover();
    }

    public new void OnPointerDown(PointerEventData eventData)
    {
        if (GetPointerOver())
            ultiArea.UltiBuy(this);
    }

    public new void OnPointerUp(PointerEventData eventData)
    {
        if (GetIsMobile() && !GetPointerOver())
            OnHover();
    }
}
