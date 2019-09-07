using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuOverseer : MonoBehaviour
{
    public QuickStartLobbyController qslc;
    public FadingScript logo;
    public FadingScript back;
    public SlidingParent mainMenu;
    public SlidingParent libraryMenu;
    public InterfaceScript interfaceScript;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Selected(int type)
    {
        switch (type)
        {
            case 1: // Quick Play
                qslc.QuickStart();
                break;

            case 2: // Cancel
                GetComponent<WaitingLobbyController>().LobbyCancel();
                break;

            case 4: // Characters
                GetComponentInParent<SlidingParent>().Recede();
                libraryMenu.Slide();
                libraryMenu.waitTime = 0.4f;
                libraryMenu.GetComponent<LibraryHub>().enabled = true;
                back.gameObject.SetActive(true);
                back.setMinguant(false);
                back.waitTime = 0.5f;
                logo.setMinguant(true);
                break;

            case 6: // Quit
                Application.Quit();
                break;

            case 7: // Back Menu
                libraryMenu.Recede();
                libraryMenu.GetComponent<LibraryHub>().enabled = false;
                mainMenu.Slide();
                mainMenu.waitTime = 0.4f;
                logo.gameObject.SetActive(true);
                logo.setMinguant(false);
                logo.waitTime = 0.5f;
                back.setMinguant(true);
                break;

            case 8: // Back Summary
                interfaceScript.Close();
                break;

            default:
                break;
        }
    }
}
