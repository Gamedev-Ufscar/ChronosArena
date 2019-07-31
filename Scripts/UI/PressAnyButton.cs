using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PressAnyButton : MonoBehaviour
{
    float time = 0f;
    bool minguant = false;
    bool deactivate = false;
    Text text;
    public SlidingParent mainMenu;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();

        if (NyarScript.NS.anyButtoned) { gameObject.SetActive(false); }
        else { AudioListener.volume = 0.6f; }
    }

    // Update is called once per frame
    void Update()
    {
        // Pulse
        Pulse();

        // Press any button
        if (Input.anyKeyDown)
        {
            mainMenu.Slide();
            deactivate = true;
            minguant = true;
            NyarScript.NS.anyButtoned = true;
        }

    }

    void Pulse()
    {
        time += Time.deltaTime;
        if (time > 0.07f) { 
            if (minguant) { 
                if (text.color.a > 0f) {
                    text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - 0.05f);
                    if (deactivate) { text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - 0.01f); }
                } else {
                    minguant = false;
                    if (deactivate) { gameObject.SetActive(false); }
                }
            } else {
                if (text.color.a < 1f) {
                    text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a + 0.05f);
                } else {
                    minguant = true;
                }
            }
            time = 0f;
        }
    }
}
