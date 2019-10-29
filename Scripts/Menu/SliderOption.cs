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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isClicking)
        {
            float newX = Mathf.Clamp(Input.mousePosition.x, center - radius, center + radius);

            GetComponent<RectTransform>().position = new Vector3(newX, GetComponent<RectTransform>().position.y);
            AudioListener.volume = (newX - center + radius)/(radius*2);
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
}
