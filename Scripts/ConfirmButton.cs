using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ConfirmButton : Button
{
    [SerializeField]
    GameOverseer gameOverseer;
    [SerializeField]
    SelectionOverseer selectionOverseer;

    //public PhotonView PV;
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

    }

    public override void PointerExit()
    {

    }

    public override void PointerUp()
    {
        transform.localScale = new Vector2(1.2f, 1.2f);
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            selectionOverseer.ConfirmButton();

            // Colors
            if (selectionOverseer.GetMyConfirm())
            {
                image.sprite = buttonColors[1];
            }
            else
            {
                image.sprite = buttonColors[0];
            }

        }
        else if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            //GameOverseer.GO.myConfirm = !GameOverseer.GO.myConfirm;
        }
    }

    public override void RightPointerUp()
    {
    }

    public override void PointerDown()
    {
        // Juicy feeling
        transform.localScale = new Vector2(1.1f, 1.1f);
    }

    public override void RightPointerDown() { }
}
