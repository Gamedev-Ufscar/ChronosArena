using UnityEngine;
using UnityEngine.UI;

public class InfoArea : MonoBehaviour
{
    [SerializeField]
    private GameObject interffaceArea;
    [SerializeField]
    private Interface interfface;
    [SerializeField]
    private Text interffaceTitle;

    private bool openInfo = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Interface GetInterface()
    {
        return interfface;
    }

    public bool GetOpenInfo()
    {
        return openInfo;
    }

    public void OpenInfo(HeroEnum hero)
    {
        openInfo = true;

        // Setup Interface
        Card[] cardList = new Card[Constants.maxCardAmount];
        int count = 0;

        for (int i = 0; i < Constants.maxCardAmount; i++)
        {
            if (CardMaker.CM.MakeCard(hero, i) != null)
            {
                cardList[i] = CardMaker.CM.MakeCard(hero, i);
                count++;
            }
        }

        // Open Interface
        GetInterface().gameObject.SetActive(true);
        interfface.GetPlayerBackButton().SetActive(true);
        GetInterface().Setup(cardList, null, count);

        interffaceTitle.text = CardMaker.CM.HeroName(hero);
        interffaceArea.transform.SetAsFirstSibling();
    }

    public void CloseInfo()
    {
        openInfo = false;
    }
}
