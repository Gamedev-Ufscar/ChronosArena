using UnityEngine;
using UnityEngine.UI;

public class ConfirmButton : Button
{
    [SerializeField]
    SelectionOverseer selectionOverseer;

    //public PhotonView PV;
    [SerializeField]
    private Sprite[] buttonColors = new Sprite[2];
    private Image image;

    float time = 0f;

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
