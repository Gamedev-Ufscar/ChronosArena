using UnityEngine;
using UnityEngine.UI;

public class Credits : MonoBehaviour
{
    [SerializeField]
    private MenuOverseer mainMenu;
    [SerializeField]
    private FadingScript gameDevUfscar;
    [SerializeField]
    private Text rollingCredits;

    bool running = false;
    Vector3 pos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (running)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                mainMenu.BringMenuBack();
                rollingCredits.transform.position = pos;
                gameDevUfscar.SetMinguant(true);
                running = false;
            }

            rollingCredits.transform.position = new Vector3(rollingCredits.transform.position.x, rollingCredits.transform.position.y + 50f*Time.deltaTime);
        }
    }

    public void StartCredits()
    {
        pos = rollingCredits.transform.position;
        running = true;
        gameDevUfscar.gameObject.SetActive(true);
        gameDevUfscar.SetMinguant(false);
    }
}
