using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceScript : MonoBehaviour
{
    public GameObject backButton;
    public int cardAmount = 0;
    public Sprite[] interfaceList;
    public string[] textList;
    public int interfaceSignal = 200;
    public GameObject optionPrefab;
    public List<Vector2> cardLocations;
    public Card invoker;

    private List<GameObject> destructionList = new List<GameObject>();
    private bool setup = false;

    [HideInInspector]
    public bool optionMenu = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        if (optionMenu) { backButton.SetActive(false); }
        else { backButton.SetActive(true); }
    }

    // Update is called once per frame
    void Update()
    {
        // Setup
        Setup(optionMenu);

        // Player chose an option
        if (interfaceSignal != 200 && invoker is Interfacer)
        {
            Close();
        }
       
    }

    public void Setup(bool optionMenu)
    {
        if (!setup)
        {
            foreach (GameObject option in destructionList) {
                Destroy(option);
            }

            interfaceSignal = 200;
            setup = true;
            //Debug.Log("IS enabled, card amount: " + cardAmount);
            cardLocations = setupCardLocations(cardAmount);
            for (int i = 0; i < cardAmount; i++) {
                GameObject optionCreated = Instantiate(optionPrefab, gameObject.transform);
                optionCreated.transform.parent = gameObject.transform;
                optionCreated.GetComponent<InterfaceCard>().index = i;
                optionCreated.GetComponent<Image>().sprite = interfaceList[i];
                optionCreated.GetComponentInChildren<Text>().text = textList[i];
                optionCreated.GetComponent<InterfaceCard>().option = optionMenu;
                destructionList.Add(optionCreated);
            }
        }
    }

    public void Close()
    {
        if (optionMenu) {
            Interfacer cc = (Interfacer)invoker;
            cc.interfaceSignal = interfaceSignal;
            invoker = (Card)cc;
        }

        foreach (GameObject option in destructionList) {
            Destroy(option);
        }

        setup = false;
        gameObject.SetActive(false);
    }

    List<Vector2> setupCardLocations(int cardAmount) {
        List<Vector2> ccardLocations = new List<Vector2>();
        float baseDistanceX, actualDistanceX, initialPositionX, baseDistanceY, actualDistanceY, initialPositionY;
        baseDistanceX = 80 /Mathf.Pow(2, Mathf.Min(cardAmount, 5) - 2);
        actualDistanceX = baseDistanceX + 85;
        baseDistanceY = 50 / Mathf.Pow(2, ((cardAmount - 1) / 5) - 2);
        actualDistanceY = baseDistanceY + 110;

        // Initial X
        if(Mathf.Min(cardAmount, 5) % 2 == 0) { // Par
            initialPositionX = (Mathf.Min(cardAmount, 5) * actualDistanceX) / 4;
        } else { // Impar
            initialPositionX = ((Mathf.Min(cardAmount, 5) - 1) / 2) * actualDistanceX;
        }

        // Initial Y
        if (((cardAmount - 1) / 5) == 0) {
            initialPositionY = 0f;
        } else if (((cardAmount - 1) / 5) % 2 == 0) { // Par
            initialPositionY = (((cardAmount - 1) / 5) * actualDistanceY) / 4;
        } else { // Impar
            initialPositionY = ((((cardAmount - 1) / 5)) / 2) * actualDistanceY;
        }

        for (int i = 0; i < cardAmount; i++) {
            int x = i % 5; int y = i / 5;
            ccardLocations.Add(new Vector2(-initialPositionX + (x * actualDistanceX), initialPositionY - (y * actualDistanceY)));
        }

        return ccardLocations;

               
    }
}
