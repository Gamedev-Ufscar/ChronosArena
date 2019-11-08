using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SliderOption : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool isClicking = false;

    [SerializeField]
    private float centerRatio;
    [SerializeField]
    private float radius;
    [SerializeField]
    private Option option;
    [SerializeField]
    private GameObject line;

    float center;
    float oldWidth;

    // Start is called before the first frame update
    void Start()
    {
        UpdateToScreenWidth();
    }

    private void OnEnable()
    {

        if (option == Option.Master && NyarScript.NS.GetMasterVolume() != null)
        {
            float newX = (float)(NyarScript.NS.GetMasterVolume() * (radius * 2)) - radius + center;
            GetComponent<RectTransform>().position = new Vector3(newX, GetComponent<RectTransform>().position.y);
        } else

        if (option == Option.Music && NyarScript.NS.GetMusicVolume() != null)
        {
            float newX = (float)(NyarScript.NS.GetMusicVolume() * (radius * 2)) - radius + center;
            GetComponent<RectTransform>().position = new Vector3(newX, GetComponent<RectTransform>().position.y);
        } else

        if (option == Option.Sound && NyarScript.NS.GetSoundVolume() != null)
        {
            float newX = (float)(NyarScript.NS.GetSoundVolume() * (radius * 2)) - radius + center;
            GetComponent<RectTransform>().position = new Vector3(newX, GetComponent<RectTransform>().position.y);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Update to screen width
        if (Screen.width != oldWidth)
        {
            UpdateToScreenWidth();
        }

        // If holding slider
        if (isClicking)
        {
            float newX = Mathf.Clamp(Input.mousePosition.x, center - radius, center + radius);

            GetComponent<RectTransform>().position = new Vector3(newX, GetComponent<RectTransform>().position.y);
            float effectValue = (newX - center + radius) / (radius * 2);
            Effect(effectValue);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isClicking = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isClicking = false;
    }

    void UpdateToScreenWidth()
    {
        center = Screen.width * centerRatio;
        line.GetComponent<RectTransform>().position = new Vector3(center, line.GetComponent<RectTransform>().position.y);
        float newX = (float)(NyarScript.NS.GetMasterVolume() * (radius * 2)) - radius + center;
        GetComponent<RectTransform>().position = new Vector3(newX, GetComponent<RectTransform>().position.y);
        oldWidth = Screen.width;
    }

    void Effect(float value)
    {
        switch (option)
        {
            case Option.Master:
                NyarScript.NS.SetMasterVolume(value);
                AudioListener.volume = (float)NyarScript.NS.GetMasterVolume();
                break;

            case Option.Music:
                NyarScript.NS.SetMusicVolume(value);
                AudioManager.AM.ChangeVolume(true, value);
                break;

            case Option.Sound:
                NyarScript.NS.SetSoundVolume(value);
                AudioManager.AM.ChangeVolume(false, value);
                break;
        }
    }
}
