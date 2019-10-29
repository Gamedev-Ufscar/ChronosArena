using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interface : MonoBehaviour
{
    [SerializeField]
    private GameOverseer gameOverseer;
    [SerializeField]
    private SelectionOverseer selectionOverseer;
    [SerializeField]
    private LibraryOverseer libraryOverseer;

    [SerializeField]
    private GameObject backButton1;
    [SerializeField]
    private GameObject backButton2;

    [SerializeField]
    private GameObject optionPrefab;
    private List<Vector2> cardLocations;

    private Card invoker;

    private List<GameObject> destructionList = new List<GameObject>();
    private bool setup = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Setup -----
    public void Setup(Card baseCard, string[] textList, Card invoker, int cardAmount)
    {
        DestroyAllCards();

        for (int i = 0; i < cardAmount; i++)
        {
            // Create card
            GameObject optionCreated = CreateInterfaceCard(i, setupCardLocations(cardAmount));

            // Setup text
            optionCreated.GetComponent<Image>().sprite = baseCard.GetImage();
            optionCreated.transform.GetChild(0).GetComponent<Text>().text = baseCard.GetName();
            optionCreated.transform.GetChild(1).GetComponent<Text>().text = baseCard.typeString(baseCard.GetCardType());
            optionCreated.transform.GetChild(2).GetComponent<Text>().text = textList[i].Replace("\\n", "\n");
            optionCreated.transform.GetChild(3).GetComponent<Text>().text = baseCard.Value(1);
            optionCreated.transform.GetChild(4).GetComponent<Text>().text = baseCard.Value(2);
            optionCreated.transform.GetChild(5).GetComponent<Text>().text = baseCard.heroString(baseCard.GetHero());
            optionCreated.GetComponent<InterfaceCard>().SetOption(i);

            SetupClickable(optionCreated, invoker);
            destructionList.Add(optionCreated);
        }
    }

    public void Setup(Card[] cardList, Card invoker, int cardAmount)
    {
        DestroyAllCards();

        for (int i = 0; i < cardAmount; i++)
        {
            // Create card
            GameObject optionCreated = CreateInterfaceCard(i, setupCardLocations(cardAmount));

            // Setup text
            optionCreated.GetComponent<Image>().sprite = cardList[i].GetImage();
            optionCreated.transform.GetChild(0).GetComponent<Text>().text = cardList[i].GetName();
            optionCreated.transform.GetChild(1).GetComponent<Text>().text = cardList[i].typeString(cardList[i].GetCardType());
            optionCreated.transform.GetChild(2).GetComponent<Text>().text = cardList[i].GetText().Replace("\\n", "\n");
            optionCreated.transform.GetChild(3).GetComponent<Text>().text = cardList[i].Value(1);
            optionCreated.transform.GetChild(4).GetComponent<Text>().text = cardList[i].Value(2);
            optionCreated.transform.GetChild(5).GetComponent<Text>().text = cardList[i].heroString(cardList[i].GetHero());
            optionCreated.GetComponent<InterfaceCard>().SetOption(i);

            SetupClickable(optionCreated, invoker);
            destructionList.Add(optionCreated);
        }
    }

    public GameObject CreateInterfaceCard(int i, List<Vector2> cardLocations)
    {
        // Create card
        GameObject optionCreated = Instantiate(optionPrefab, gameObject.transform);
        optionCreated.transform.parent = gameObject.transform;
        optionCreated.GetComponent<InterfaceCard>().SetInterface(this);
        optionCreated.GetComponent<InterfaceCard>().ChangePosition(cardLocations[i]);

        return optionCreated;
    }

    public void DestroyAllCards()
    {
        foreach (GameObject option in destructionList)
        {
            Destroy(option);
        }
    }

    public void SetupClickable(GameObject optionCreated, Card invoker)
    {
        // Set true if clickable
        if (invoker == null)
        {
            optionCreated.GetComponent<InterfaceCard>().SetIsClickable(false);
        }
        else
        {
            optionCreated.GetComponent<InterfaceCard>().SetIsClickable(true);
            this.invoker = invoker;
        }
    }

    List<Vector2> setupCardLocations(int cardAmount)
    {
        List<Vector2> ccardLocations = new List<Vector2>();
        float baseDistanceX, actualDistanceX, initialPositionX, baseDistanceY, actualDistanceY, initialPositionY;
        baseDistanceX = 80 / Mathf.Pow(2, Mathf.Min(cardAmount, 5) - 2);
        actualDistanceX = baseDistanceX + 85;
        baseDistanceY = 25 / Mathf.Pow(2, (1 + ((cardAmount - 1) / 5)) - 2);
        actualDistanceY = baseDistanceY + 110;

        // Initial X
        if (Mathf.Min(cardAmount, 5) % 2 == 0)
        { // Par
            initialPositionX = (Mathf.Min(cardAmount, 5) * actualDistanceX) / 4;
        }
        else
        { // Impar
            initialPositionX = ((Mathf.Min(cardAmount, 5) - 1) / 2) * actualDistanceX;
        }

        // Initial Y
        if (((cardAmount - 1) / 5) <= 0)
        {
            initialPositionY = 0f;
        }
        else if ((1 + ((cardAmount - 1) / 5)) % 2 == 0)
        { // Par
            initialPositionY = ((1 + ((cardAmount - 1) / 5)) * actualDistanceY) / 4;
            Debug.Log("par");
        }
        else
        { // Impar
            initialPositionY = ((1 + ((cardAmount - 1) / 5)) / 2) * actualDistanceY;
            Debug.Log("impar");
        }

        for (int i = 0; i < cardAmount; i++)
        {
            int x = i % 5; int y = i / 5;
            ccardLocations.Add(new Vector2(-initialPositionX + (x * actualDistanceX), initialPositionY - (y * actualDistanceY)));
        }

        return ccardLocations;


    }

    // Close ---------
    public void Close(int? interfaceSignal)
    {
        // Send Interface Signal
        if (interfaceSignal != null)
        {
            Interfacer cc = (Interfacer)invoker;
            cc.SetSignal((int)interfaceSignal);
            gameOverseer.SendInterfaceSignal((int)interfaceSignal);
            invoker = (Card)cc;
        }

        // Close in Library
        if (libraryOverseer != null)
        {
            libraryOverseer.CloseInfo();
        }

        // Close in Selection
        if (selectionOverseer != null)
        {
            selectionOverseer.CloseInfo();
        }

        // Destroy all cards
        foreach (GameObject option in destructionList)
        {
            Destroy(option);
        }

        // Disable everything
        setup = false;
        if (GetPlayerBackButton() != null)
            GetPlayerBackButton().SetActive(false);
        if (GetEnemyBackButton() != null)
            GetEnemyBackButton().SetActive(false);
        gameObject.SetActive(false);
    }

    // Getter
    public GameObject GetPlayerBackButton()
    {
        return backButton1;
    }

    public GameObject GetEnemyBackButton()
    {
        return backButton2;
    }
}
