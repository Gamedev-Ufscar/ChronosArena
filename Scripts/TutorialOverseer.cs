using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialOverseer : MonoBehaviour
{
    [SerializeField]
    private GameObject textPrefab;
    [SerializeField]
    private GameObject UIManager;
    [SerializeField]
    private GameOverseer gameOverseer;
    [SerializeField]
    private Sprite profile;
    [SerializeField]
    private Sprite enemyProfile;

    private List<CardTypes> attackDisableList = new List<CardTypes>();

    Hashtable tutHashtable = new Hashtable();

    struct tutorialBox
    {
        public float x;
        public float y;
        public float scaleX;
        public float scaleY;
        public string text;
        public int? nextButton;

        public tutorialBox(float x, float y, float scaleX, float scaleY, string text, int? nextButton)
        {
            this.x = x;
            this.y = y;
            this.scaleX = scaleX;
            this.scaleY = scaleY;
            this.text = text;
            this.nextButton = nextButton;
        }
    }

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        attackDisableList.Add(CardTypes.Attack); attackDisableList.Add(CardTypes.Nullification); attackDisableList.Add(CardTypes.Skill);
        attackDisableList.Add(CardTypes.Ultimate); attackDisableList.Add(CardTypes.Ultimate);

        gameOverseer.GetMyPlayer().CreatePlayer(HeroEnum.Uga, 7, 1, 0, 0, attackDisableList, profile);
        gameOverseer.GetEnemyPlayer().CreatePlayer(HeroEnum.Timothy, 8, 2, 0, 2, attackDisableList, enemyProfile);

        CreateText(180, 0, 200, 100, "Bem-Vindo ao Tutorial. Clique sobre este retângulo para avançar.", 1);

        Setup();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene((int)SceneList.Menu);
        }
    }

    void Setup()
    {
        tutHashtable.Add(1, new tutorialBox(-227f, -166f, 200f, 100f, "Existem 7 tipos de cartas no jogo: Ataque, Defesa, Carga, Skill, " +
            "Anulação, Ultimate e Item.", 2));
        tutHashtable.Add(2, new tutorialBox(-227f, -166f, 300f, 100f, "Ataque é denotado pela cor vermelha. Esta carta causa dano direto ao" +
            "inimigo, mas possui um limite de usos consecutivos.", 3));
        tutHashtable.Add(3, new tutorialBox(-227f, -166f, 200f, 100f, "Esses dois valores, Dano e Limite, são mostrados na parte inferior da " +
            "carta de Ataque.", 4));
        tutHashtable.Add(4, new tutorialBox(-346f, 76f, 200f, 100f, "Tente jogar Pancada do Tronco arrastando a carta para o centro da tela.", null));
    }

    void CreateText(tutorialBox tb)
    {
        CreateText(tb.x, tb.y, tb.scaleX, tb.scaleY, tb.text, tb.nextButton);
    }

    void CreateText(float x, float y, float scaleX, float scaleY, string text, int? nextButton)
    {
        GameObject tp = Instantiate(textPrefab, new Vector3(x, y), Quaternion.identity, UIManager.transform);
        tp.transform.localPosition = new Vector3(x, y);
        tp.GetComponent<RectTransform>().sizeDelta = new Vector2(scaleX, scaleY);
        tp.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(0.875f*scaleX, 0.8f*scaleY);
        tp.GetComponentInChildren<Text>().text = text;

        tp.GetComponent<TutorialButton>().SetTutorialOverseer(this);
        tp.GetComponent<TutorialButton>().SetNextButton(nextButton);
    }

    public void ReceiveClick(TutorialButton button)
    {
        if (button.GetNextButton() != null)
            CreateText((tutorialBox)tutHashtable[button.GetNextButton()]);
        Destroy(button.gameObject);
    }
}
