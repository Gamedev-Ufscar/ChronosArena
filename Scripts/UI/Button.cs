using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    bool mouseOver = false;
    Image image;
    Text text;
    public Image arrow;

    public int type;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        text = GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        Highlighting();
    }


    void Highlighting()
    {
        if (mouseOver) {
            // Sorting Order
            transform.SetAsLastSibling();

            // Cor
            image.color = new Color(1f, 1f, 1f, image.color.a);
            text.color = new Color(1f, 1f, 1f, image.color.a);
            if (arrow != null) { arrow.color = new Color(1f, 1f, 1f, arrow.color.a); }



        } else {
            // Sorting Order
            transform.SetAsFirstSibling();

            // Cor
            if (SceneManager.GetActiveScene().buildIndex == 3)
            {
                image.color = new Color(0.9f, 0.9f, 0.9f, image.color.a);
                text.color = new Color(0.9f, 0.9f, 0.9f, image.color.a);
                if (arrow != null) { arrow.color = new Color(0.9f, 0.9f, 0.9f, arrow.color.a); }
            } else {
                image.color = new Color(0.57f, 0.57f, 0.57f, image.color.a);
                text.color = new Color(0.57f, 0.57f, 0.57f, image.color.a);
                if (arrow != null) { arrow.color = new Color(0.57f, 0.57f, 0.57f, arrow.color.a); }
            }
        }
    }


    // Pointer stuff
    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOver = true;
        if (GetComponentInParent<LibraryHub>() != null) {
            GetComponentInParent<LibraryHub>().currentSheet = type;
            GetComponentInParent<LibraryHub>().update = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOver = false;
        if (GetComponentInParent<LibraryHub>() != null) {
            GetComponentInParent<LibraryHub>().currentSheet = 200;
            GetComponentInParent<LibraryHub>().update = true;
        }
    }

    public bool GetMouse()
    {
        return mouseOver;
    }

}