using UnityEngine;
using UnityEngine.UI;

public class GameDevButton : Button
{

    // Start is called before the first frame update
    void Start()
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


    public override void RightPointerDown() { }

    public override void PointerUp()
    {
        ChangeScale(1f);
        Application.OpenURL("https://www.facebook.com/GameDevUFSCar");
    }

    public override void RightPointerUp() { }

    public override void PointerEnter()
    {
        if (GetColor() > 0.5f)
            ChangeTone(1f, 0.3f, true);

    }

    public override void PointerExit()
    {
        ChangeScale(1f);
        if (GetColor() > 0.5f)
            ChangeTone(0.57f, 0.3f, true);
    }
}
