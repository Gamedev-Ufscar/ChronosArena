using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LibraryHub : MonoBehaviour
{
    public FadingScript back;
    public List<FadingScript> sheetList = new List<FadingScript>();

    [HideInInspector]
    public int currentSheet = 200;
    [HideInInspector]
    public int formerSheet = 200;
    [HideInInspector]
    public bool update = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (update) { 
            if (currentSheet == 200) {
                back.gameObject.SetActive(true);
                back.setMinguant(false);
                back.waitTime = 0.25f;
                sheetList[formerSheet].setMinguant(true);
            } else {
                if (formerSheet != currentSheet && formerSheet != 200)
                {
                    sheetList[formerSheet].setMinguant(true);
                }
                sheetList[currentSheet].gameObject.SetActive(true);
                sheetList[currentSheet].setMinguant(false);
                sheetList[currentSheet].waitTime = 0.25f;
                back.setMinguant(true);
                formerSheet = currentSheet;
                Debug.Log(currentSheet);
            }
            update = false;
        }
    }
}
