using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuOverseer : MonoBehaviour
{
    [SerializeField]
    private QuickStartLobbyController qslc;
    [SerializeField]
    private FadingScript logo;
    [SerializeField]
    private FadingScript back;
    [SerializeField]
    private ManualOverseer manualOverseer;
    [SerializeField]
    private SlidingParent mainMenu;
    [SerializeField]
    private SlidingParent libraryMenu;

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

            case 0: // Cancel
                GetComponent<WaitingLobbyController>().LobbyCancel();
                break;

            case 2: // Characters
                mainMenu.Recede();
                libraryMenu.Slide();
                libraryMenu.SetWaitTime(0.4f);
                libraryMenu.GetComponent<LibraryHub>().enabled = true;
                back.gameObject.SetActive(true);
                back.SetMinguant(false);
                back.SetWaitTime(0.5f);
                logo.SetMinguant(true);
                break;

            case 3: // Manual
                mainMenu.Recede();
                manualOverseer.OpenManualPage(0);
                logo.SetMinguant(true);
                break;

            case 4: // Tutorial
                SceneManager.LoadScene((int)SceneList.Tutorial);
                break;

            case 7: // Quit
                Application.Quit();
                break;

            case 8: // Back Menu
                libraryMenu.Recede();
                libraryMenu.GetComponent<LibraryHub>().enabled = false;
                BringMenuBack();
                break;

            default:
                break;
        }
    }

    public void ButtonHover(MenuButton menuButton)
    {
        menuButton.ChangeTone(1f);
        menuButton.ChangeChildTone(menuButton.transform.childCount, 1f);
        menuButton.transform.SetAsLastSibling();
    }

    public void ButtonStopHover(MenuButton menuButton)
    {
        menuButton.ChangeTone(0.57f);
        menuButton.ChangeChildTone(menuButton.transform.childCount, 0.57f);
        menuButton.transform.SetAsFirstSibling();
    }

    public void BringMenuBack()
    {
        mainMenu.Slide();
        mainMenu.SetWaitTime(0.4f);
        logo.gameObject.SetActive(true);
        logo.SetMinguant(false);
        logo.SetWaitTime(0.5f);
        back.SetMinguant(true);
    }
}
