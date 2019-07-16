using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChooseCharacter : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Image>().color = new Color(0.8f, 0.8f, 0.8f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponent<Image>().color = new Color(1f, 1f, 1f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GetComponent<Image>().color = new Color(0.8f, 0.8f, 0.8f);
    }
}
