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
    int cardSent = 0;
    static float timerSet = 0.75f;

    int chargeCount = 0;
    bool attacked = false;

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
        AudioManager.AM.StopAll();
        AudioManager.AM.Play("BattleTheme");

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

        boxHashtable.Add(TutorialBox.Introducao, new tutorialBox(-180, 90, 200f, 100f, "Existem 7 tipos de cartas no jogo: Ataque, Defesa, Carga, Skill," +
            " Anulação, Ultimate e Item."));
        boxHashtable.Add(TutorialBox.AtaquePrimeiro, new tutorialBox(-180, 90, 300f, 100f, "Ataque é denotado pela cor vermelha. Esta carta causa dano direto ao" +
            " inimigo, mas possui um limite de usos consecutivos."));
        boxHashtable.Add(TutorialBox.DanoELimite, new tutorialBox(-180, 90, 200f, 100f, "Esses dois valores, Dano e Limite, são mostrados na parte inferior da" +
            " carta de Ataque."));
        boxHashtable.Add(TutorialBox.JogarAtaquePrimeiro, new tutorialBox(-376, 65, 150f, 105f, "Tente jogar Pancada do Tronco arrastando a carta para o centro da tela."));
        boxHashtable.Add(TutorialBox.ParabensAtaque, new tutorialBox(-376, 65, 170f, 145f, "Parabéns, você causou dano ao inimigo. A quantidade de dano causada é mostrada" +
            " no lado esquerdo de sua carta."));
        boxHashtable.Add(TutorialBox.BotaoCentro, new tutorialBox(-366, 65, 150, 100f, "Para avançar, clique no botão no centro da tela."));
        boxHashtable.Add(TutorialBox.Limite, new tutorialBox(-180, 90, 200f, 100f, "A carte de Ataque é poderosa, mas possui um porém - o LIMITE."));
        boxHashtable.Add(TutorialBox.LimiteExplicacao, new tutorialBox(-180, 90, 300f, 100f, "O Limite sinaliza a quantidade de ataques CONSECUTIVOS que o usuário pode executar." +
            " Caso não respeitado, uma penalidade é aplicada."));
        boxHashtable.Add(TutorialBox.JogarAtaqueSegundo, new tutorialBox(-376, 65, 170f, 125f, "Jogue a carta de Ataque novamente para ilustrar este ponto. O limite está no lado" +
            " direito da carta."));
        boxHashtable.Add(TutorialBox.LimiteContexto, new tutorialBox(-366, 65, 170f, 145f, "Por ter jogado Ataque duas vezes, as únicas cartas disponíveis no próximo turno são" +
            " Defesa e Carga. Esta é a penalidade."));
        boxHashtable.Add(TutorialBox.EscolhaDuas, new tutorialBox(180, 85, 200f, 100f, "Escolha uma das duas para jogar."));
        boxHashtable.Add(TutorialBox.Defesa, new tutorialBox(-366, 65, 150, 100f, "A carta de Defesa reduz o dano total que o usuário receberia."));
        boxHashtable.Add(TutorialBox.Carga, new tutorialBox(-366, 65, 150, 135f, "A carta de Carga dá Energia ao usuário, que é usada para obter Ultimates e Itens. Ela" +
            " também tem um Limite."));
        boxHashtable.Add(TutorialBox.TenteOutra, new tutorialBox(180, 90, 200f, 100f, "Agora, tente jogar a outra carta."));
        boxHashtable.Add(TutorialBox.InimigoAtacou, new tutorialBox(180, 90, 300f, 100f, "Agora que Yuri atacou, ele atingiu seu limite de Ataque. Como ele pode apenas jogar" +
            " Defesa ou Carga este turno, você está seguro. Jogue Carne Crua."));
        boxHashtable.Add(TutorialBox.Skill, new tutorialBox(-366, 65, 155, 135f, "Cartas de Skill, estrelas laranjas, são exclusivas de cada personagem. Depois de jogadas, elas são" +
            " descartadas."));
        boxHashtable.Add(TutorialBox.SkillDownfall, new tutorialBox(-180, 90, 200f, 100f, "Skills são poderosas, mas elas têm uma fraqueza - Anulações."));
        boxHashtable.Add(TutorialBox.Anulacao, new tutorialBox(-180, 90, 300f, 100f, "Anulações são cartas arriscadas. Denotadas por uma cruz azul, essas cartas anulam os" +
            " símbolos mostrados na área inferior da carta."));
        boxHashtable.Add(TutorialBox.JogueAnulacao, new tutorialBox(-346, 90, 200f, 100f, "Porém, se o usuário errar a Anulação, ele perde essa carta. Dito isto, jogue a anulação."));
        boxHashtable.Add(TutorialBox.ParabensAnulacao, new tutorialBox(-376, 65, 160, 130f, "Parabéns, você anulou a carta do inimigo. Como sua anulação foi bem-sucedida," +
            " a carta retorna para a sua mão."));
        boxHashtable.Add(TutorialBox.PercebaPassiva, new tutorialBox(-180, 90, 200f, 100f, "Perceba que Yuri levou dano neste turno. Isso é por causa de sua PASSIVA."));
        boxHashtable.Add(TutorialBox.Passiva, new tutorialBox(-180, 90, 300f, 100f, "Passivas não são cartas jogáveis, e sim efeitos ou ações que não necessitam de cartas. No caso de" +
            " Yuri, sua Passiva retira vida quando ele é anulado."));
        boxHashtable.Add(TutorialBox.Resumo, new tutorialBox(-180, 90, 300f, 100f, "Para ver esta Passiva (e outras cartas), clique com o botão DIREITO sobre o Retrato do" +
            " inimigo. Você pode fazer o mesmo com o próprio Retrato."));
        boxHashtable.Add(TutorialBox.Shuffle, new tutorialBox(-180, 90, 300f, 100f, "Além disso, ao clicar com o botão ESQUERDO sobre seu próprio Retrato, você pode embaralhar" +
            " suas cartas."));
        boxHashtable.Add(TutorialBox.Ultimate, new tutorialBox(-180, 80, 280f, 115f, "A última carta a ser vista é a Ultimate. Esta carta é muito parecida com a Skill, com a diferença" +
            " de que ela precisa ser comprada com Energia para ser obtida ou recuperada."));
        boxHashtable.Add(TutorialBox.CargaNovamente, new tutorialBox(-346, 90, 200f, 100f, "Jogue Carga novamente para obter mais Energia."));
        boxHashtable.Add(TutorialBox.CompraUltimate, new tutorialBox(-180, 90, 300f, 125f, "Na Fase de Compra, você pode comprar sua Ultimate. Para isso, clique uma vez sobre ela. Ao avançar" +
            " para a próxima fase (com o botão no centro), a Ultimate entrará para a sua mão."));
        boxHashtable.Add(TutorialBox.MuitoBem, new tutorialBox(-180, 80, 270f, 105f, "Muito bem! O Tutorial está concluido. Se quiser mais informações, acesse o Manual. Você também pode" +
            " continuar jogando aqui ou apertar ESC para sair."));
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
                    currentBox = CreateText((tutorialBox)boxHashtable[TutorialBox.Introducao]);
                    break;

                case 1: // Ataque
                    state++;
                    currentBox = CreateText((tutorialBox)boxHashtable[TutorialBox.AtaquePrimeiro]);
                    HighlightCard(0);
                    if (cardID == 0)
                    {
                        state = 4;
                        timer = timerSet;
                    }
                    break;

                case 2:
                    if (cardID == null)
                    {
                        state++;
                        currentBox = CreateText((tutorialBox)boxHashtable[TutorialBox.DanoELimite]);
                    }
                    else if (cardID == 0)
                    {
                        state = 4;
                        timer = timerSet;
                    }
                    break;

                case 3:
                    if (cardID == null)
                    {
                        state++;
                        currentBox = CreateText((tutorialBox)boxHashtable[TutorialBox.JogarAtaquePrimeiro]);
                    }
                    else if (cardID == 0)
                    {
                        state = 4;
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
                        currentBox = CreateText((tutorialBox)boxHashtable[TutorialBox.ParabensAtaque]);
                        SummonCard(2);
                    }
                    break;

                case 5: // After both play cards
                    if (cardID == null)
                    {
                        state++;
                        currentBox = CreateText((tutorialBox)boxHashtable[TutorialBox.BotaoCentro]);
                    }
                    else if (cardID == 1000 && gameOverseer.GetState() == GameState.Effects)
                    {
                        state = 7;
                        currentBox = CreateText((tutorialBox)boxHashtable[TutorialBox.Limite]);
                        gameOverseer.SetEnemyConfirm(true);
                        DisableAllCards();
                    }
                    break;

                case 6: // Press the center button, please
                    if (cardID == 1000 && gameOverseer.GetState() == GameState.Effects)
                    {
                        state++;
                        currentBox = CreateText((tutorialBox)boxHashtable[TutorialBox.Limite]);
                        gameOverseer.SetEnemyConfirm(true);
                        DisableAllCards();
                    }
                    break;

                case 7: // Limits
                    if (cardID == null)
                    {
                        state++;
                        currentBox = CreateText((tutorialBox)boxHashtable[TutorialBox.LimiteExplicacao]);
                    }
                    break;

                case 8: // Play Attack again, please
                    if (cardID == null)
                    {
                        state++;
                        currentBox = CreateText((tutorialBox)boxHashtable[TutorialBox.JogarAtaqueSegundo]);
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
                        currentBox = CreateText((tutorialBox)boxHashtable[TutorialBox.LimiteContexto]);
                        SummonCard(2);
                    }
                    break;

                case 10: // Escolha uma
                    if (cardID == 1000 && gameOverseer.GetState() == GameState.Effects)
                    {
                        state++;
                        currentBox = CreateText((tutorialBox)boxHashtable[TutorialBox.EscolhaDuas]);
                        gameOverseer.SetEnemyConfirm(true);
                        ActivateCard(1);
                        ActivateCard(2);
                    }
                    break;

                default:
                    break;
            }
        else if (state > 10 && state <= 20)
            switch (state)
            {
                case 11: // Jogou Defesa ou Carga?
                    if (cardID == 1)
                    {
                        state++;
                        timer = timerSet;
                        currentBox = CreateText((tutorialBox)boxHashtable[TutorialBox.Defesa]);
                    } else if (cardID == 2)
                    {
                        state += 2;
                        timer = timerSet;
                        currentBox = CreateText((tutorialBox)boxHashtable[TutorialBox.Carga]);
                    }
                    break;

                case 12: // Defesa
                    if (cardID == 2000)
                    {
                        SummonCard(5);
                    }
                    else if (cardID == 1000 && gameOverseer.GetState() == GameState.Effects)
                    {
                        state += 2;
                        gameOverseer.SetEnemyConfirm(true);
                        currentBox = CreateText((tutorialBox)boxHashtable[TutorialBox.TenteOutra]);
                        DisableAllCards();
                        HighlightCard(2);
                    }
                    break;

                case 13: // Carga
                    if (cardID == 2000)
                    {
                        SummonCard(1);
                    }
                    else if (cardID == 1000 && gameOverseer.GetState() == GameState.Effects)
                    {
                        state++;
                        gameOverseer.SetEnemyConfirm(true);
                        currentBox = CreateText((tutorialBox)boxHashtable[TutorialBox.TenteOutra]);
                        DisableAllCards();
                        HighlightCard(1);
                    }
                    break;

                case 14: // Other card
                    if (cardID == 1)
                    {
                        timer = timerSet;
                        currentBox = CreateText((tutorialBox)boxHashtable[TutorialBox.Defesa]);
                    } else if (cardID == 2)
                    {
                        timer = timerSet;
                        currentBox = CreateText((tutorialBox)boxHashtable[TutorialBox.Carga]);
                    }
                    else if (cardID == 2000)
                    {
                        state++;
                        SummonCard(0);
                    }
                    else if (cardID == null)
                        Destroy(currentBox);
                    break;

                case 15: // Agora que inimigo atacou...
                    if (cardID == 1000 && gameOverseer.GetState() == GameState.Effects)
                    {
                        currentBox = CreateText((tutorialBox)boxHashtable[TutorialBox.InimigoAtacou]);
                        gameOverseer.SetEnemyConfirm(true);
                        DisableAllCards();
                        HighlightCard(6);
                    } else if (cardID == 6)
                    {
                        currentBox = CreateText((tutorialBox)boxHashtable[TutorialBox.Skill]);
                        timer = timerSet;
                    } else if (cardID == 2000)
                    {
                        state++;
                        SummonCard(1);
                    }
                    break;

                case 16: // Skill Downfall
                    if (cardID == 1000 && gameOverseer.GetState() == GameState.Effects)
                    {
                        state++;
                        currentBox = CreateText((tutorialBox)boxHashtable[TutorialBox.SkillDownfall]);
                        gameOverseer.SetEnemyConfirm(true);
                    }
                    break;

                case 17: // Anulação
                    if (cardID == null)
                    {
                        state++;
                        currentBox = CreateText((tutorialBox)boxHashtable[TutorialBox.Anulacao]);
                        HighlightCard(3);
                    }
                    break;

                case 18: // Jogue Anulação e Parabens Anulacao
                    if (cardID == null)
                    {
                        currentBox = CreateText((tutorialBox)boxHashtable[TutorialBox.JogueAnulacao]);
                    }
                    else if (cardID == 3)
                        timer = timerSet;
                    else if (cardID == 2000)
                    {
                        state++;
                        SummonCard(4);
                        currentBox = CreateText((tutorialBox)boxHashtable[TutorialBox.ParabensAnulacao]);
                    }
                    break;

                case 19: // Perceba Passiva
                    if (cardID == 1000 && gameOverseer.GetState() == GameState.Effects)
                    {
                        state++;
                        currentBox = CreateText((tutorialBox)boxHashtable[TutorialBox.PercebaPassiva]);
                        gameOverseer.SetEnemyConfirm(true);
                    }
                    break;

                case 20: // Passiva
                    if (cardID == null)
                    {
                        state++;
                        currentBox = CreateText((tutorialBox)boxHashtable[TutorialBox.Passiva]);
                    }
                    break;

            }
        else
            switch (state)
            {
                case 21: // Resumo
                    if (cardID == null)
                    {
                        state++;
                        currentBox = CreateText((tutorialBox)boxHashtable[TutorialBox.Resumo]);
                    }
                    break;

                case 22: // Shuffle
                    if (cardID == null)
                    {
                        state++;
                        currentBox = CreateText((tutorialBox)boxHashtable[TutorialBox.Shuffle]);
                    }
                    break;

                case 23: // Ultimate
                    if (cardID == null)
                    {
                        state++;
                        currentBox = CreateText((tutorialBox)boxHashtable[TutorialBox.Ultimate]);
                    }
                    break;

                case 24: // Carga Novamente
                    if (cardID == null)
                    {
                        state++;
                        currentBox = CreateText((tutorialBox)boxHashtable[TutorialBox.CargaNovamente]);
                        HighlightCard(2);
                    }
                    break;

                case 25: // Carga Novamente
                    if (cardID == 2)
                    {
                        timer = timerSet;
                        Destroy(currentBox);
                    }
                    else if (cardID == 2000)
                    {
                        state++;
                        SummonCard(1);
                    }
                    break;

                case 26: // Compra Ultimate?
                    if (cardID == 1000 && gameOverseer.GetState() == GameState.Effects)
                    {
                        state++;
                        currentBox = CreateText((tutorialBox)boxHashtable[TutorialBox.CompraUltimate]);
                        gameOverseer.SetEnemyConfirm(true);
                    }
                    else if (cardID == null)
                        Destroy(currentBox);
                    break;

                case 27: // Compra Ultimate?
                    if (cardID == 3000)
                        state++;
                    else if (cardID == null)
                        Destroy(currentBox);
                    break;

                case 28: // Muito bem
                    if (cardID == 1000)
                    {
                        state++;
                        currentBox = CreateText((tutorialBox)boxHashtable[TutorialBox.MuitoBem]);
                        gameOverseer.SetEnemyConfirm(true);
                    }
                    break;

                case 29: // Fim
                    if (cardID == null)
                        Destroy(currentBox);
                    else if (cardID == 1000 && (gameOverseer.GetState() == GameState.Effects))
                    {
                            gameOverseer.SetEnemyConfirm(true);
                    }
                    else if (cardID == 2000)
                    {
                        if (cardSent == 0) { // Jogou Ataque
                            if (chargeCount < 3) {
                                SummonCard(2);
                                chargeCount++;
                            }
                            else {
                                SummonCard(0);
                                chargeCount = 0;
                            }
                        }
                        else if (cardSent == 1) { // Jogou Defesa
                            SummonCard(1);
                            chargeCount = 0;
                        } else if (cardSent == 2) { // Jogou Carga
                            SummonCard(1);
                            chargeCount = 0;
                        } else { 
                            if (chargeCount < 3) {
                                SummonCard(2);
                                chargeCount++;
                            }
                            else
                            {
                                SummonCard(0);
                                chargeCount = 0;
                            }
                        }
                    }
                    else if (cardID < 1000)
                    {
                        cardSent = (int)cardID;
                        timer = timerSet;
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
        ActivateCard(id);
    }

    void ActivateCard(int id)
    {
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
