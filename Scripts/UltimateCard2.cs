/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UltimateCard2 : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    public bool zoomCard = false;
    public DeckManager deckManager;

    public GameObject cardPrefab;
    public Sprite cardSprite;
    public int staticCardIndex = 100;
    public int cardIndex = 100;
    public int thisCard;
    public bool bought = false;
    private float red = 0f;

    private int textColor = 50;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Image>().color = new Color(0.6f, 0.6f, 0.6f);
        if (deckManager.gameObject == HeroDecks.HD.myManager.myHand) {
            cardSprite = HeroDecks.HD.myManager.cardList[thisCard].image;
            transform.GetComponent<Image>().sprite = HeroDecks.HD.myManager.cardList[thisCard].image;
            transform.GetChild(0).GetComponent<Text>().text = HeroDecks.HD.myManager.cardList[thisCard].name;
            transform.GetChild(1).GetComponent<Text>().text = HeroDecks.HD.myManager.cardList[thisCard].typeString(HeroDecks.HD.myManager.cardList[thisCard].type);
            transform.GetChild(2).GetComponent<Text>().text = HeroDecks.HD.myManager.cardList[thisCard].text.Replace("\\n", "\n");
            transform.GetChild(3).GetComponent<Text>().text = HeroDecks.HD.value(HeroDecks.HD.myManager.cardList[thisCard], 1);
            transform.GetChild(4).GetComponent<Text>().text = HeroDecks.HD.value(HeroDecks.HD.myManager.cardList[thisCard], 2);
            transform.GetChild(5).GetComponent<Text>().text = HeroDecks.HD.myManager.cardList[thisCard].heroString(HeroDecks.HD.myManager.hero);
        } else {
            cardSprite = HeroDecks.HD.enemyManager.cardList[thisCard].image;
            transform.GetComponent<Image>().sprite = HeroDecks.HD.enemyManager.cardList[thisCard].image;
            transform.GetChild(0).GetComponent<Text>().text = HeroDecks.HD.enemyManager.cardList[thisCard].name;
            transform.GetChild(1).GetComponent<Text>().text = HeroDecks.HD.enemyManager.cardList[thisCard].typeString(HeroDecks.HD.enemyManager.cardList[thisCard].type);
            transform.GetChild(2).GetComponent<Text>().text = HeroDecks.HD.enemyManager.cardList[thisCard].text.Replace("\\n", "\n");
            transform.GetChild(3).GetComponent<Text>().text = HeroDecks.HD.value(HeroDecks.HD.enemyManager.cardList[thisCard], 1);
            transform.GetChild(4).GetComponent<Text>().text = HeroDecks.HD.value(HeroDecks.HD.enemyManager.cardList[thisCard], 2);
            transform.GetChild(5).GetComponent<Text>().text = HeroDecks.HD.enemyManager.cardList[thisCard].heroString(HeroDecks.HD.enemyManager.hero);
        }
    }

    private void OnEnable()
    {
        bought = false;
        GameOverseer.GO.ultiBuy[staticCardIndex - 100] = false;
        GameOverseer.GO.enemyUltiBuy[staticCardIndex - 100] = false;
        if (deckManager != null)
            cardIndex = deckManager.placeUltimate(staticCardIndex);
    }

    // Update is called once per frame
    void Update()
    {
        // Enemy Card stuff
        if (deckManager.gameObject == HeroDecks.HD.enemyManager.myHand) {
            if (GameOverseer.GO.enemyUltiBuy[staticCardIndex - 100]) { bought = true; }
            else { bought = false; }
        }


        // Hovering the card
        if ((zoomCard && !HeroDecks.HD.interfaceScript.gameObject.activeInHierarchy) ||
           (GameOverseer.GO.enemyHoveringCard == cardIndex && 
           ((GameOverseer.GO.isEnemyHoveringHimself == true && deckManager.gameObject == HeroDecks.HD.enemyManager.myHand) ||
           (GameOverseer.GO.isEnemyHoveringHimself == false && deckManager.gameObject == HeroDecks.HD.myManager.myHand)))) { // Ou eu to hoverando ou o inimigo ta
            ZoomCard();
        } else {

            // Reset hovering Ulti
            if (deckManager.hoveringUlti == cardIndex) {
                deckManager.hoveringUlti = 200;
            }

            if (deckManager.hoveringUlti != 200) {
                UltimatesHighlighted();
            } else {
                UltimatesHidden();
            }
        }

        // Cores
        Cores();

        // Acquire card (After buying it)
        acquireCard();

    }

    // Cores -> 1 (Comprado) / 0.8 (Visualizando) / 0.6 (Nada) / 0.3 (Não comprável)
    void Cores ()
    {

        if (bought) { // Already bought
            GetComponent<Image>().color = new Color(1f, 1f - red, 1f - red);
        } else if (GameOverseer.GO.state != GameState.Purchase) { // Out of purchase phase
            GetComponent<Image>().color = new Color(0.3f, 0.3f - red, 0.3f - red);
        } else {
            if (HeroDecks.HD.myManager.cardList[thisCard] != null) { 
                if (HeroDecks.HD.myManager.Charge < HeroDecks.HD.myManager.cardList[thisCard].cost) { // Unpurchaseable
                    GetComponent<Image>().color = new Color(0.3f, 0.3f - red, 0.3f - red);
                    transformColor(16);
                } else if (zoomCard) {  // Reading card
                    GetComponent<Image>().color = new Color(0.8f, 0.8f - red, 0.8f - red);
                    transformColor(50);
                } else {  // Available card
                    GetComponent<Image>().color = new Color(0.6f, 0.6f - red, 0.6f - red);
                    transformColor(32);
                }
            } else {
                if (zoomCard) { // Reading enemy card
                    GetComponent<Image>().color = new Color(0.8f, 0.8f - red, 0.8f - red);
                    transformColor(50);
                } else {  // Resting enemy card
                    GetComponent<Image>().color = new Color(0.6f, 0.6f - red, 0.6f - red);
                    transformColor(32);
                }
            }
        }

        // Se inimigo estiver olhando, fica mais vermelho
        if (GameOverseer.GO.enemyHoveringCard == cardIndex &&
           ((GameOverseer.GO.isEnemyHoveringHimself == true && deckManager.gameObject == HeroDecks.HD.enemyManager.myHand) ||
           (GameOverseer.GO.isEnemyHoveringHimself == false && deckManager.gameObject == HeroDecks.HD.myManager.myHand))) {
            red = 0.2f;
        } else {
            red = 0f;
        }
    }

    void transformColor (byte color)
    {
        transform.GetChild(0).GetComponent<Text>().color = new Color32(color, color, color, 255);
        transform.GetChild(1).GetComponent<Text>().color = new Color32(color, color, color, 255);
        transform.GetChild(2).GetComponent<Text>().color = new Color32(color, color, color, 255);
        transform.GetChild(3).GetComponent<Text>().color = new Color32(color, color, color, 255);
        transform.GetChild(4).GetComponent<Text>().color = new Color32(color, color, color, 255);
        transform.GetChild(5).GetComponent<Text>().color = new Color32(color, color, color, 255);
    }


    // Acquire card (After buying it)
    void acquireCard()
    {
        if (GameOverseer.GO.state == GameState.Choice && bought)
        {
            if (deckManager.gameObject == HeroDecks.HD.myManager.myHand) // If Player
            {
                GameObject card = HeroDecks.HD.myManager.CreateCard(deckManager.activeCardCount, thisCard);
                card.transform.parent = deckManager.transform;
                deckManager.deckList[deckManager.cardTotalCount] = card;
                deckManager.cardTotalCount++;
                deckManager.activeCardCount++;
                card.GetComponent<CardInHand>().zoomCard = false; card.GetComponent<CardInHand>().moveCard = false;
                card.GetComponent<CardInHand>().ultiCard = gameObject;

            
            } else if (deckManager.gameObject == HeroDecks.HD.enemyManager.myHand) { // If Enemy
                GameObject card = HeroDecks.HD.enemyManager.EnemyCreateCard(deckManager.activeCardCount, thisCard);
                card.transform.parent = deckManager.transform;
                deckManager.deckList[deckManager.cardTotalCount] = card;
                deckManager.cardTotalCount++;
                deckManager.activeCardCount++;
                card.GetComponent<EnemyCardInHand>().ultiCard = gameObject;
            }
            bought = false;
            GameOverseer.GO.enemyUltiBuy[staticCardIndex - 100] = false;
            deckManager.recedeUlti(cardIndex);
            HeroDecks.HD.audioManager.CardSound();
            gameObject.SetActive(false);
        }
    }

    // Zooming the card and stuff

    public void ZoomCard()
    {
        // Control location and scale
        transform.localPosition = Vector2.Lerp(transform.localPosition + new Vector3(0f, HeroDecks.cardMoveUp-3f, 0f),
                                                    deckManager.ultiLocations[cardIndex - 100], 0.1f);
        transform.localScale = new Vector3(HeroDecks.cardZoomSize * 0.8665f, HeroDecks.cardZoomSize * 1.177f, 1f); //0.8665, 1.177

        // Put it on the forefront
        transform.SetAsLastSibling();
        if (deckManager.gameObject == HeroDecks.HD.myManager.myHand && zoomCard == true) {
            GameObject.Find("Main UI").GetComponent<MainUIManager>().hoveredCard = HeroDecks.HD.myManager.cardList[thisCard].name;
        } else if (deckManager.gameObject == HeroDecks.HD.enemyManager.myHand && zoomCard == true) { 
            GameObject.Find("Main UI").GetComponent<MainUIManager>().hoveredCard = HeroDecks.HD.enemyManager.cardList[thisCard].name;
        }

        // Set hovering ulti to true
        deckManager.hoveringUlti = cardIndex;

        // Buy the card
        //Debug.Log(deckManager.gameObject.name + ", " + HeroDecks.HD.myManager.myHand.name);
        if (Input.GetMouseButtonDown(0) && deckManager.gameObject == HeroDecks.HD.myManager.myHand && GameOverseer.GO.state == GameState.Purchase) {
            Debug.Log("mouse down");
            if (!bought) {
                if (HeroDecks.HD.myManager.Charge >= HeroDecks.HD.myManager.cardList[thisCard].cost) {
                    HeroDecks.HD.myManager.Charge -= HeroDecks.HD.myManager.cardList[thisCard].cost;
                    bought = true;

                    GameOverseer.GO.ultiBuy[staticCardIndex-100] = true;
                }
            } else {
                HeroDecks.HD.myManager.Charge += HeroDecks.HD.myManager.cardList[thisCard].cost;
                bought = false;
                GameOverseer.GO.ultiBuy[staticCardIndex - 100] = false;
            }
        }
    }

    public void UltimatesHighlighted()
    {
        // Stop enemy network hovering
        if (GameOverseer.GO.hoveringCard == cardIndex &&
           ((GameOverseer.GO.amIHoveringMyself == true && deckManager.gameObject == HeroDecks.HD.myManager.myHand) ||
           (GameOverseer.GO.amIHoveringMyself == false && deckManager.gameObject == HeroDecks.HD.enemyManager.myHand))) {
            GameOverseer.GO.hoveringCard = 200;
        }

        int actualLocation = cardIndex - 100;
        if (actualLocation < 0) { actualLocation = 0; }

        // Get back into position
        transform.localScale = new Vector3(0.8665f, 1.177f, 1f);
        transform.localPosition = Vector2.Lerp(transform.localPosition, deckManager.ultiLocations[actualLocation], Time.deltaTime * 5f);

        // When card gets back into position, remove override

        if (transform.localPosition.x >= deckManager.ultiLocations[actualLocation].x - 1f &&
            transform.localPosition.x <= deckManager.ultiLocations[actualLocation].x + 1f &&
            transform.localPosition.y >= deckManager.ultiLocations[actualLocation].y - 1f &&
            transform.localPosition.y <= deckManager.ultiLocations[actualLocation].y + 1f) {
            transform.SetAsFirstSibling();
        }
    }

    public void UltimatesHidden()
    {
        // Stop enemy network hovering
        if (GameOverseer.GO.hoveringCard == cardIndex &&
           ((GameOverseer.GO.amIHoveringMyself == true && deckManager.gameObject == HeroDecks.HD.myManager.myHand) ||
           (GameOverseer.GO.amIHoveringMyself == false && deckManager.gameObject == HeroDecks.HD.enemyManager.myHand)))
        {
            GameOverseer.GO.hoveringCard = 200;
        }

        // Get back into ultimate default position
        transform.localScale = new Vector3(0.8665f, 1.177f, 1f);
        transform.localPosition = Vector2.Lerp(transform.localPosition, deckManager.ultiLocations[0], Time.deltaTime * 5f);

        // Sorting order
        if (cardIndex != 100)
            transform.SetAsFirstSibling();
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        // If not already holding something, zoom the card
        if (deckManager.holdingCard == false)
        {
            GetComponent<Image>().color = new Color(0.8f, 0.8f, 0.8f);
            transform.localPosition = transform.localPosition + new Vector3(0f, 5f, 0f);
            zoomCard = true;
            GameOverseer.GO.hoveringCard = cardIndex;
            if (deckManager.gameObject == HeroDecks.HD.myManager.myHand) {  // Hovering my card
                GameOverseer.GO.amIHoveringMyself = true;
            } else if (deckManager.gameObject == HeroDecks.HD.enemyManager.myHand) {  // Hovering enemy card
                GameOverseer.GO.amIHoveringMyself = false;
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GetComponent<Image>().color = new Color(0.6f, 0.6f, 0.6f);
        zoomCard = false;
    }


}*/