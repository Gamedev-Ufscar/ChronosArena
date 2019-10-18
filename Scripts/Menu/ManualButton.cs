using UnityEngine;
using UnityEngine.UI;

public class ManualButton : Button
{
    [SerializeField]
    private ManualOverseer manualOverseer;
    [SerializeField]
    private int pageLink = 0;
    [SerializeField]
    private int ownPage = 0;
    [SerializeField]
    private ManualButtonType type;

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
        ChangeScale(0.9f);
    }

    // Getter
    float GetColor()
    {
        if (GetComponent<Image>() != null)
            return GetComponent<Image>().color.a;
        if (GetComponent<Text>() != null)
            return GetComponent<Text>().color.a;

        return 0f;
    }

    public int GetOwnPage()
    {
        return ownPage;
    }

    public override void RightPointerDown() { }

    public override void PointerUp() {
        ChangeScale(1f);

        if (type == ManualButtonType.Link)
            manualOverseer.OpenManualPage(pageLink);
        else if (type == ManualButtonType.Left)
            manualOverseer.TurnPage(true);
        else if (type == ManualButtonType.Right)
            manualOverseer.TurnPage(false);
        else
            manualOverseer.CloseManual();
    }

    public override void RightPointerUp() { }

    public override void PointerEnter()
    {
        if (type == ManualButtonType.Link)
        {
            if (GetColor() > 0.5f)
                ChangeTone(1f, 0.3f, true);
        }
        else
            ChangeTone(1f);

    }

    public override void PointerExit()
    {
        ChangeScale(1f);

        if (type == ManualButtonType.Link)
        {
            if (GetColor() > 0.5f)
                ChangeTone(0.57f, 0.3f, true);
        }
        else
            ChangeTone(0.57f);
    }
}
