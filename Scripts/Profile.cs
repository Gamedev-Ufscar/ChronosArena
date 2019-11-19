using UnityEngine;
using UnityEngine.UI;

public class Profile : Button
{
    static float maxTime = 0.2f;

    [SerializeField]
    private Player player;

    private float? timeSinceLastTap = null;

    // Update is called once per frame
    void Update()
    {
        if (GetIsMobile() && timeSinceLastTap != null)
        {
            if (timeSinceLastTap < maxTime)
                timeSinceLastTap += Time.deltaTime;
            else
            {
                player.OnShufflePress();
                timeSinceLastTap = null;
            }

        }
    }

    public void SetImage(Sprite image)
    {
        GetComponent<Image>().sprite = image;
    }

    public void ChangeScale(float scale)
    {
        transform.localScale = new Vector3(1.210431f*scale, 1.272552f*scale);
    }

    public override void PointerDown()
    {
        if (GetIsMobile())
        {
            if (timeSinceLastTap != null && timeSinceLastTap < maxTime)
            {
                RightPointerDown();
                timeSinceLastTap = null;
            } else
            {
                timeSinceLastTap = 0f;
            }
        }
        else
        {
            player.OnShufflePress();
        }
        ChangeScale(0.9f);
    }

    public override void RightPointerDown() {
        player.InvokeSummary();
        ChangeScale(0.9f);
    }

    public override void PointerUp()
    {
        ChangeScale(1f);
    }

    public override void RightPointerUp()
    {
    }

    public override void PointerEnter()
    {

    }

    public override void PointerExit()
    {
        ChangeScale(1f);
    }
}
