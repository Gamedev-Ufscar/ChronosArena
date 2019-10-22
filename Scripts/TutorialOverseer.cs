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
    private GameObject UITutorial;
    [SerializeField]
    private GameOverseer gameOverseer;
    [SerializeField]
    private Sprite profile;
    [SerializeField]
    private Sprite enemyProfile;
    [SerializeField]
    private GameObject interfaceDark;

    private List<CardTypes> attackDisableList = new List<CardTypes>();

    Hashtable boxHashtable = new Hashtable();
    Hashtable eventHashtable = new Hashtable();

    GameObject currentBox;
    int state;
    float? timer = null;
    static float timerSet = 0.75f;

    struct tutorialBox
    {
        public float x;
        public float y;
        public float scaleX;
        public float scaleY;
        public string text;

        public tutorialBox(float x, float y, float scaleX, float scaleY, string text)
        {
            this.x = x;
            this.y = y;
            this.scaleX = scaleX;
            this.scaleY = scaleY;
            this.text = text;
        }
    }

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        // Create attack disable list
        attackDisableList.Add(CardTypes.Attack); attackDisableList.Add(CardTypes.Nullification); attackDisableList.Add(CardTypes.Skill);
        attackDisableList.Add(CardTypes.Ultimate); attackDisableList.Add(CardTypes.Ultimate);

        // Create players
        gameOverseer.GetMyPlayer().CreatePlayer(HeroEnum.Uga, 7, 1, 0, 0, attackDisableList, profile);
        gameOverseer.GetEnemyPlayer().CreatePlayer(HeroEnum.Yuri, 7, 1, 1, 2, attackDisableList, enemyProfile);

        // Create textbox
        currentBox = CreateText(180, 0, 200, 100, "Bem-Vindo ao Tutorial. Clique sobre este retângulo para avançar.");

        // Disable ALL cards
        DisableAllCards();

        Setup();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene((int)SceneList.Menu);
        }

        if (timer != null)
        {
            if (timer > 0f)
                timer -= Time.deltaTime;
            else
            {
                StateMachine(2000);
                timer = null;
            }
        }
    }

    void Setup()
    {
        state = 0;

        boxHashtable.Add(1, new tutorialBox(-180, 90, 200f, 100f, "Existem 7 tipos de cartas no jogo: Ataque, Defesa, Carga, Skill," +
            " Anulação, Ultimate e Item."));
        boxHashtable.Add(2, new tutorialBox(-180, 90, 300f, 100f, "Ataque é denotado pela cor vermelha. Esta carta causa dano direto ao" +
            " inimigo, mas possui um limite de usos consecutivos."));
        boxHashtable.Add(3, new tutorialBox(-180, 90, 200f, 100f, "Esses dois valores, Dano e Limite, são mostrados na parte inferior da" +
            " carta de Ataque."));
        boxHashtable.Add(4, new tutorialBox(-346, 85, 200f, 100f, "Tente jogar Pancada do Tronco arrastando a carta para o centro da tela."));
        boxHashtable.Add(5, new tutorialBox(-346, 85, 200f, 100f, "Parabéns, você causou dano ao inimigo. A quantidade de dano causada é mostrada" +
            " no lado esquerdo de sua carta."));
        boxHashtable.Add(6, new tutorialBox(-346, 85, 200f, 100f, "Para avançar, clique no botão no centro da tela."));
        boxHashtable.Add(7, new tutorialBox(-180, 90, 200f, 100f, "A carte de Ataque é poderosa, mas possui um porém - o LIMITE."));
        boxHashtable.Add(8, new tutorialBox(-180, 90, 300f, 100f, "O Limite sinaliza a quantidade de ataques CONSECUTIVOS que o usuário pode executar." +
            " Caso não respeitado, uma penalidade é aplicada."));
        boxHashtable.Add(9, new tutorialBox(-346, 85, 200f, 100f, "Jogue a carta de Ataque novamente para ilustrar este ponto. O limite está no lado" +
            " direito da carta."));
        boxHashtable.Add(10, new tutorialBox(-346, 85, 200f, 100f, "Por ter jogado Ataque duas vezes, as únicas cartas disponíveis no próximo turno são" +
            " Defesa e Carga. Esta é a penalidade."));
        boxHashtable.Add(11, new tutorialBox(-346, 85, 200f, 100f, "Escolha entre uma das duas para jogar."));
        boxHashtable.Add(12, new tutorialBox(-346, 85, 200f, 100f, "A carta de Defesa reduz o dano total que o usuário receberia."));
        boxHashtable.Add(13, new tutorialBox(-346, 85, 200f, 100f, "A carta de Carga dá Energia ao usuário, que é usada para obter Ultimates e Itens. Ela" +
            " também tem um Limite."));
        boxHashtable.Add(14, new tutorialBox(-180, 85, 200f, 100f, "Agora, tente jogar a outra carta."));
    }

    public void ReceiveClick(TutorialButton button)
    {
        StateMachine(null);
    }

    public void ReceiveCardPlayed(int cardID)
    {
        StateMachine(cardID);
    }

    void StateMachine(int? cardID) // null = press textbox; 0-999 = cards; 1000 = confirmButton; 2000 = timer
    {

        Debug.Log("State: " + state);
        if (state <= 10)
            switch (state)
            {
                case 0: // Introdução
                    state++;
                    interfaceDark.SetActive(false);
                    currentBox = CreateText((tutorialBox)boxHashtable[1]);
                    break;

                case 1: // Ataque
                    state++;
                    currentBox = CreateText((tutorialBox)boxHashtable[2]);
                    HighlightCard(0);
                    break;

                case 2:
                    if (cardID == null)
                    {
                        state++;
                        currentBox = CreateText((tutorialBox)boxHashtable[3]);
                    }
                    else if (cardID == 0)
                    {
                        state = 5;
                        timer = timerSet;
                    }
                    break;

                case 3:
                    if (cardID == null)
                    {
                        state++;
                        currentBox = CreateText((tutorialBox)boxHashtable[4]);
                    }
                    else if (cardID == 0)
                    {
                        state = 5;
                        timer = timerSet;
                    }
                    break;

                case 4: // Wait Attack Card
                    if (cardID == null)
                        Destroy(currentBox);
                    else if (cardID == 0)
                        timer = timerSet;
                    else if (cardID == 2000)
                    {
                        state++;
                        currentBox = CreateText((tutorialBox)boxHashtable[5]);
                        SummonCard(2);
                    }
                    break;

                case 5: // After both play cards
                    if (cardID == null)
                    {
                        state++;
                        currentBox = CreateText((tutorialBox)boxHashtable[6]);
                    }
                    else if (cardID == 1000 && gameOverseer.GetState() == GameState.Effects)
                    {
                        state = 7;
                        currentBox = CreateText((tutorialBox)boxHashtable[7]);
                        gameOverseer.SetEnemyConfirm(true);
                    }
                    break;

                case 6: // Press the center button, please
                    if (cardID == 1000 && gameOverseer.GetState() == GameState.Effects)
                    {
                        state++;
                        currentBox = CreateText((tutorialBox)boxHashtable[7]);
                        gameOverseer.SetEnemyConfirm(true);
                    }
                    break;

                case 7: // Limits
                    if (cardID == null)
                    {
                        state++;
                        currentBox = CreateText((tutorialBox)boxHashtable[8]);
                    }
                    break;

                case 8: // Play Attack again, please
                    if (cardID == null)
                    {
                        state++;
                        currentBox = CreateText((tutorialBox)boxHashtable[9]);
                        HighlightCard(0);
                    }
                    break;

                case 9: // Wait Attack Card
                    if (cardID == null)
                        Destroy(currentBox);
                    else if (cardID == 0)
                        timer = timerSet;
                    else if (cardID == 2000)
                    {
                        state++;
                        currentBox = CreateText((tutorialBox)boxHashtable[10]);
                        SummonCard(2);
                    }
                    break;

                case 10:
                    if (cardID == 1000 && gameOverseer.GetState() == GameState.Effects)
                    {
                        state++;
                        currentBox = CreateText((tutorialBox)boxHashtable[11]);
                        gameOverseer.SetEnemyConfirm(true);
                    }
                    break;

                default:
                    break;
            }
        else
            switch (state)
            {
                case 11:
                    if (cardID == 1)
                    {
                        state++;
                        timer = timerSet;
                        currentBox = CreateText((tutorialBox)boxHashtable[12]);
                    } else if (cardID == 2)
                    {
                        state += 2;
                        timer = timerSet;
                        currentBox = CreateText((tutorialBox)boxHashtable[13]);
                    }
                    break;

                case 12:
                    if (cardID == 2000)
                    {
                        SummonCard(0);
                    }
                    else if (cardID == 1000)
                    {
                        state += 2;
                        gameOverseer.SetEnemyConfirm(true);
                        currentBox = CreateText((tutorialBox)boxHashtable[14]);
                        DisableAllCards();
                        HighlightCard(2);
                    }
                    break;

                case 13:
                    if (cardID == 2000)
                    {
                        SummonCard(1);
                    }
                    else if (cardID == 1000)
                    {
                        state += 2;
                        gameOverseer.SetEnemyConfirm(true);
                        currentBox = CreateText((tutorialBox)boxHashtable[14]);
                        DisableAllCards();
                        HighlightCard(1);
                    }
                    break;
            }
    }

    GameObject CreateText(tutorialBox tb)
    {
        return CreateText(tb.x, tb.y, tb.scaleX, tb.scaleY, tb.text);
    }

    GameObject CreateText(float x, float y, float scaleX, float scaleY, string text)
    {
        if (currentBox != null) { Destroy(currentBox); }

        GameObject tp = Instantiate(textPrefab, new Vector3(x, y), Quaternion.identity, UITutorial.transform);
        tp.transform.localPosition = new Vector3(x, y);
        tp.GetComponent<RectTransform>().sizeDelta = new Vector2(scaleX, scaleY);
        tp.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(0.875f * scaleX, 0.8f * scaleY);
        tp.GetComponentInChildren<Text>().text = text;

        tp.GetComponent<TutorialButton>().SetTutorialOverseer(this);

        return tp;
    }

    void HighlightCard(int id)
    {
        gameOverseer.ForceHandCardHover(id);
        gameOverseer.GetMyPlayer().GetCard(id).SetTurnsTill(0);
        gameOverseer.GetMyPlayer().GetDeckCard(id).SetDarkened(false);
    }

    void SummonCard(int id)
    {
        gameOverseer.ReceiveSummon(id);
        gameOverseer.SetEnemyConfirm(true);
    }

    void DisableAllCards()
    {
        for (int i = 0; i < Constants.maxCardAmount; i++)
        {
            if (gameOverseer.GetMyPlayer().GetCard(i) != null)
            {
                DisableCard(i);
            }
        }
    }

    void DisableCard(int id)
    {
        gameOverseer.GetMyPlayer().GetCard(id).SetTurnsTill(2);
        gameOverseer.GetMyPlayer().GetDeckCard(id).SetDarkened(true);
    }
}
