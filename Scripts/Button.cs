using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public abstract class Button : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler
{
    bool mouseOver = false;
    float red = 0f;
    float blue = 0f;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Getter
    public bool GetMouse()
    {
        return mouseOver;
    }

    // Setter/Changer
    public void ChangeTone(float color)
    {
        if (GetComponent<Image>() != null)
            GetComponent<Image>().color = new Color(color - blue, color - red - blue, color - red);
        if (GetComponent<Text>() != null)
            GetComponent<Text>().color = new Color(color - blue, color - red - blue, color - red);
    }

    public void ChangeTone(float color, float red)
    {
        this.red = red;
        ChangeTone(color);
    }

    public void ChangeTone(float color, float blue, bool isBlue)
    {
        this.blue = blue;
        ChangeTone(color);
    }

    public void ChangeScale(float scale)
    {
            transform.localScale = new Vector3((transform.localScale.x/Mathf.Abs(transform.localScale.x)) * scale, scale, scale);
    }

    public void ChangeChildTone(int indexx, float color)
    {
        for (int i = 0; i < indexx; i++)
        {
            if (gameObject.transform.GetChild(i) != null)
            {
                if (gameObject.transform.GetChild(i).GetComponent<Text>() != null)
                {
                    gameObject.transform.GetChild(i).GetComponent<Text>().color = new Color(color, color - red, color - red);
                }
                else if (gameObject.transform.GetChild(i).GetComponent<Image>() != null)
                {
                    gameObject.transform.GetChild(i).GetComponent<Image>().color = new Color(color, color - red, color - red);
                }
            }
        }
    }

    public void ChangeChildTone(int indexx, float color, float red)
    {
        this.red = red;
        ChangeChildTone(indexx, color);
    }

    public abstract void PointerDown();

    public abstract void RightPointerDown();

    public abstract void PointerUp();

    public abstract void RightPointerUp();

    public abstract void PointerEnter();

    public abstract void PointerExit();

    // Pointer stuff
    public void OnPointerDown(PointerEventData eventData)
    {
        if (mouseOver)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
                PointerDown();
            else if (eventData.button == PointerEventData.InputButton.Right)
                RightPointerDown();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (mouseOver)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
                PointerUp();
            else if (eventData.button == PointerEventData.InputButton.Right)
                RightPointerUp();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOver = true;
        PointerEnter();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOver = false;
        PointerExit();
    }

}