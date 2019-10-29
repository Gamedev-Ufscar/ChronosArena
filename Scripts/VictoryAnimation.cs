using UnityEngine;
using UnityEngine.UI;

public class VictoryAnimation : MonoBehaviour
{
    [SerializeField]
    private GameOverseer gameOverseer;
    [SerializeField]
    private GameObject parent;
    [SerializeField]
    private Image square;
    [SerializeField]
    private Text text;
    [SerializeField]
    private Image upperLine;
    [SerializeField]
    private Image lowerLine;

    private static float maxTime = 5f;
    private float rateOfColor = 0.75f/maxTime;
    private float rateOfWidth = 2f / maxTime;
    private float rateOfSize = 0.5f / maxTime;
    private float? time = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (time != null)
        {
            if (time < maxTime)
            {
                ChangeColor(square);
                ChangeColor(text);
                ChangeColor(upperLine);
                ChangeColor(lowerLine);
                ChangeWidth(upperLine);
                ChangeWidth(lowerLine);
                ChangeSize(parent);
                time += Time.deltaTime;
            }

            if (Input.anyKey || Input.GetMouseButtonDown(0))
            {
                gameOverseer.EndGame();
            }
        }
    }

    void ChangeColor(Image image)
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a + (rateOfColor * Time.deltaTime));
    }

    void ChangeColor(Text image)
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a + (rateOfColor * Time.deltaTime));
    }

    void ChangeWidth(Image image)
    {
        image.transform.localScale = new Vector3(image.transform.localScale.x + (rateOfWidth* Time.deltaTime), image.transform.localScale.y, image.transform.localScale.z);
    }

    void ChangeSize(GameObject image)
    {
        image.transform.localScale = new Vector3(image.transform.localScale.x + (rateOfSize * Time.deltaTime), image.transform.localScale.y + (rateOfSize * Time.deltaTime), image.transform.localScale.z);
    }

    public void ActivateAnim(string victText)
    {
        time = 0f;
        text.text = victText;
    }
}
