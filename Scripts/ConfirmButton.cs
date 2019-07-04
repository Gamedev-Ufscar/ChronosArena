using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ConfirmButton : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    public PhotonView PV;
    public Sprite[] buttonColors = new Sprite[3];
    private Image image;

    [HideInInspector]
    public bool pointerOver = false;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
       
    }

    // Update is called once per frame
    void Update()
    {
        MouseClick();
    }

    public void MouseClick()
    {

        // Click
        if (pointerOver == true && Input.GetMouseButtonDown(0))
        {
            GameOverseer.GO.myConfirm = !GameOverseer.GO.myConfirm;
            image.sprite = buttonColors[1];


        // Button Colors
        } else if (!Input.GetMouseButton(0)) {
            if (GameOverseer.GO.myConfirm == true)  {
                image.sprite = buttonColors[2];
            } else {
                image.sprite = buttonColors[0];
            }
        }

    }

    // Pointer Stuff

    public void OnPointerEnter(PointerEventData eventData)
    {
        pointerOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {

        pointerOver = false;
    }
}
