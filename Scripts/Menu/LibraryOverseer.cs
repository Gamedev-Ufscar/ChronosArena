using System.Collections.Generic;
using UnityEngine;

public class LibraryOverseer : MonoBehaviour
{
    [SerializeField]
    private FadingScript back;
    [SerializeField]
    private List<FadingScript> sheetList = new List<FadingScript>();
    [SerializeField]
    private InfoArea infoArea;

    private int currentSheet = 200;
    private int formerSheet = 200;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void SetCurrentSheet(int currentSheet)
    {
        this.currentSheet = currentSheet;
    }

    public void UpdateStatus()
    {
        Debug.Log(currentSheet);
        if (currentSheet == 200)
        {
            if (!infoArea.GetOpenInfo())
            {
                back.gameObject.SetActive(true);
                back.SetMinguant(false);
                back.SetWaitTime(0.25f);
            }

            if (formerSheet != 200)
                sheetList[formerSheet].SetMinguant(true);
        }
        else
        {
            if (formerSheet != currentSheet && formerSheet != 200)
            {
                sheetList[formerSheet].SetMinguant(true);
            }

            sheetList[currentSheet].gameObject.SetActive(true);
            sheetList[currentSheet].SetMinguant(false);
            sheetList[currentSheet].SetWaitTime(0.25f);
            back.SetMinguant(true);
            formerSheet = currentSheet;
        }
    }

    public void Recede()
    {
        GetComponent<SlidingParent>().Recede();
    }

    public void OpenInfo(HeroEnum hero)
    {
        infoArea.OpenInfo(hero);

        // Recede Char Menu
        Recede();
        SetCurrentSheet(200);
        UpdateStatus();

    }

    public void CloseInfo()
    {
        infoArea.CloseInfo();
        GetComponent<SlidingParent>().Slide();
        GetComponent<SlidingParent>().SetWaitTime(0.4f);
        enabled = true;
        back.gameObject.SetActive(true);
        back.SetMinguant(false);
        back.SetWaitTime(0.5f);
    }
}