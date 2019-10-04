using UnityEngine;
using UnityEngine.UI;

public class MidButton : Button
{
    [SerializeField]
    GameOverseer gameOverseer;

    //public PhotonView PV;
    [SerializeField]
    private Sprite[] buttonColors = new Sprite[3];
    private Image image;

    float time = 0f;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();

    }

    // Update is called once per frame
    void Update()
    {
        Glow();
    }

    void Glow()
    {
        time -= Time.deltaTime;
        if (time <= 0f)
        {

            if (!gameOverseer.GetMyConfirm() && gameOverseer.GetState() != GameState.Choice)
            {
                if (image.sprite == buttonColors[0] && !base.GetMouse())
                {
                    image.sprite = buttonColors[1];
                    time = 0.35f;
                }
                else
                {
                    image.sprite = buttonColors[0];
                    time = 1f;
                }
            }

        }
    }

    public void SetImageColor(bool confirmed)
    {
        if (confirmed)
            image.sprite = buttonColors[1];
        else
        {
            image.sprite = buttonColors[0];
        }
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

        if (gameOverseer.GetState() != GameState.Choice)
        {
            gameOverseer.InvertMyConfirm();

            if (gameOverseer.GetMyConfirm())
            {
                image.sprite = buttonColors[1];
            }
            else
            {
                image.sprite = buttonColors[0];
            }
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
