using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SliderOption : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool isClicking = false;

    [SerializeField]
    private float center;
    [SerializeField]
    private float radius;
    [SerializeField]
    private Option option;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        if (NyarScript.NS.GetMasterVolume() != null)
        {
            float newX = (float)(NyarScript.NS.GetMasterVolume() * (radius * 2)) - radius + center;
            GetComponent<RectTransform>().position = new Vector3(newX, GetComponent<RectTransform>().position.y);
        }
    }

    // Update is called once per frame
    void Update()
    {
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

    void Effect(float value)
    {
        switch (option)
        {
            case Option.Master:
                NyarScript.NS.SetMasterVolume(value);
                AudioListener.volume = (float)NyarScript.NS.GetMasterVolume();
                break;

            case Option.Music:
                AudioManager.AM.ChangeVolume(true, value);
                break;

            case Option.Sound:
                AudioManager.AM.ChangeVolume(false, value);
                break;
        }
    }
}
