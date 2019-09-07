using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ConfirmButton : Button
{
    GameOverseer gameOverseer;
    SelectionOverseer selectionOverseer;

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
    }

    public override void PointerEnter()
    {
        throw new System.NotImplementedException();
    }

    public override void PointerExit()
    {
        throw new System.NotImplementedException();
    }

    public override void PointerDown()
    {
        // Click
        if (pointerOver == true && Input.GetMouseButtonUp(0))
        {
            if (SceneManager.GetActiveScene().buildIndex == 2) {
                selectionOverseer.InvertMyConfirm();

                // Colors
                if (selectionOverseer.GetMyConfirm()) {
                    image.sprite = buttonColors[1];
                } else {
                    image.sprite = buttonColors[0];
                }

            } else if (SceneManager.GetActiveScene().buildIndex == 3) {
                //GameOverseer.GO.myConfirm = !GameOverseer.GO.myConfirm;
            }

        }

        // Juicy feeling
        if (pointerOver == true && Input.GetMouseButton(0))
        {
            transform.localScale = new Vector2(1.1f, 1.1f);
        } else
        {
            transform.localScale = new Vector2(1.2f, 1.2f);
        }
    }
}
