using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LibraryHub : MonoBehaviour
{
    [SerializeField]
    private FadingScript back;
    [SerializeField]
    private List<FadingScript> sheetList = new List<FadingScript>();

    private int currentSheet = 200;
    private int formerSheet = 200;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetCurrentSheet(int currentSheet)
    {
        this.currentSheet = currentSheet;
    }

    public void UpdateStatus()
    {
        if (currentSheet == 200)
        {
            back.gameObject.SetActive(true);
            back.SetMinguant(false);
            back.SetWaitTime(0.25f);
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
            Debug.Log(currentSheet);
        }
    }
}
