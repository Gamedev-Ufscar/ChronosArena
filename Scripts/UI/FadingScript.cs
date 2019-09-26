using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadingScript : MonoBehaviour
{
    float time;
    bool minguant = false;
    Image image;
    Text text;
    [SerializeField]
    private List<Text> textList = new List<Text>();
    [SerializeField]
    private List<Image> imageList = new List<Image>();

    private float waitTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        Fade();
    }

    void Fade()
    {
        time += Time.deltaTime;
        if (time > 0.07f) { 
            if (minguant) { 
                // Fading
                if (image.color.a > 0f) {
                    image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a - 0.25f);
                    foreach (Text text in textList) {
                        text.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a);
                    }
                    foreach (Image iimage in imageList) {
                        iimage.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a);
                    }
                } else {
                    gameObject.SetActive(false);
                }
            } else {
                // Appearing
                if (waitTime > 0f) { waitTime -= 0.07f; }
                else if (image.color.a < 1f) {
                    image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a + 0.25f);
                    foreach (Text text in textList) {
                        text.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a);
                    }
                    foreach (Image iimage in imageList) {
                        iimage.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a);
                    }
                }
            }
            time = 0f;
        }
    }

    // Setter
    public void SetMinguant(bool setter)
    {
        minguant = setter;
    }

    public void SetWaitTime(float waitTime)
    {
        this.waitTime = waitTime;
    }
}