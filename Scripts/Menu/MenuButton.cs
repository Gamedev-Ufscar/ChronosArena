using UnityEngine;

public class MenuButton : Button
{
    [SerializeField]
    private MenuOverseer menuOverseer;
    [SerializeField]
    private int type = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    public override void PointerDown()
    {
        menuOverseer.Selected(type);
        Debug.Log("selected");
    }

    public override void RightPointerDown() { }

    public override void PointerUp()
    {
    }

    public override void RightPointerUp()
    {
    }

    public override void PointerEnter()
    {
        menuOverseer.ButtonHover(this);
    }

    public override void PointerExit()
    {
        menuOverseer.ButtonStopHover(this);
    }

}
