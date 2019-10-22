using UnityEngine;
using UnityEngine.UI;

public class ManualOverseer : MonoBehaviour
{
    [SerializeField]
    private MenuOverseer menuOverseer;
    [SerializeField]
    private FadingScript initialPage;
    [SerializeField]
    private FadingScript directPage;
    [SerializeField]
    private FadingScript alternatePage;
    [SerializeField]
    private FadingScript leftArrow;
    [SerializeField]
    private FadingScript rightArrow;
    [SerializeField]
    private FadingScript backButton;

    [SerializeField]
    private Sprite[] pageList;

    [SerializeField]
    private FadingScript[] linkList;

    private bool isInitial = true;
    private int currentPage = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OpenManual()
    {
        // Arrows
        RevealPage(leftArrow, 0.5f);
        RevealPage(rightArrow, 0.5f);
        RevealPage(backButton, 0.5f);

        // Execution
        initialPage.GetComponent<Image>().sprite = pageList[0];
        RevealPage(initialPage, 0.5f);

        directPage.SetMinguant(true);
        alternatePage.SetMinguant(true);
        currentPage = 0;
    }

    public void OpenManualPage(int page)
    {
        currentPage = page;
        if (page == 0) { OpenManual(); return; }

        FadingScript openingPage, closingPage;

        // Initial or Alternate setup
        openingPage = isInitial ? directPage : alternatePage;
        closingPage = isInitial ? alternatePage : directPage;

        // Execution
        openingPage.GetComponent<Image>().sprite = pageList[page];
        RevealPage(openingPage, 0.25f);

        HidePage(initialPage, 0.125f);
        HidePage(closingPage, 0.125f);

        // Activate Links
        foreach (FadingScript fs in linkList)
        {
            if (fs.GetComponentInChildren<ManualButton>().GetOwnPage() == page)
            {
                RevealPage(fs, 0.25f);
            } else
            {
                HidePage(fs, 0.125f);
            }
        }

        // Invert initial
        isInitial = !isInitial;
    }

    public void TurnPage(bool toLeft)
    {
        if (toLeft)
            if (currentPage > 0)
            {
                OpenManualPage(currentPage - 1);
            } else
            {
                OpenManualPage(pageList.Length - 1);
            }
            else if (!toLeft)
            {
                if (currentPage < pageList.Length - 1)
                {
                    OpenManualPage(currentPage + 1);
                }
                else
                {
                    OpenManualPage(0);
                }
            }
    }

    public void CloseManual()
    {
        // Execution
        leftArrow.SetMinguant(true);
        rightArrow.SetMinguant(true);
        backButton.SetMinguant(true);
        initialPage.SetMinguant(true);
        directPage.SetMinguant(true);
        alternatePage.SetMinguant(true);
        foreach (FadingScript fs in linkList)
        {
            fs.SetMinguant(true);
        }

        menuOverseer.BringMenuBack();
    }

    void RevealPage(FadingScript fs, float waitTime)
    {
        fs.gameObject.SetActive(true);
        fs.SetMinguant(false);
        fs.SetWaitTime(waitTime);
    }

    void HidePage(FadingScript fs, float fadeOutTime)
    {
        fs.SetMinguant(true);
        fs.SetFadeOutSpeed(fadeOutTime);
    }
}
