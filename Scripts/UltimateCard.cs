using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UltimateCard : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    public bool zoomCard = false;
    public DeckManager deckManager;
    public Canvas canvas;

    public GameObject cardPrefab;
    public Sprite cardSprite;
    public int cardIndex;
    public int thisCard;
    public bool bought = false;
    private float red = 0f;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Image>().color = new Color(0.6f, 0.6f, 0.6f);
    }

    // Update is called once per frame
    void Update()
    {
        // Enemy Card stuff
        if (deckManager.gameObject == HeroDecks.HD.enemyManager.myHand) {
            bought = GameOverseer.GO.enemyUltiBuy;
        }


        // Moving the card around and stuff
        if (zoomCard == true || 
           (GameOverseer.GO.enemyHoveringCard == cardIndex && 
           ((GameOverseer.GO.isEnemyHoveringHimself == true && deckManager.gameObject == HeroDecks.HD.enemyManager.myHand) ||
           (GameOverseer.GO.isEnemyHoveringHimself == false && deckManager.gameObject == HeroDecks.HD.myManager.myHand)))) { // Ou eu to hoverando ou o inimigo ta
            ZoomCard();
        } else {
            ReturnCard();
        }

        // Cores
        Cores();

        // Acquire card (After buying it)
        acquireCard();

    }

    // Cores -> 1 (Comprado) / 0.8 (Visualizando) / 0.6 (Nada) / 0.3 (Não comprável)
    void Cores ()
    {

        if (bought) {
            GetComponent<Image>().color = new Color(1f, 1f - red, 1f - red);
        } else if (GameOverseer.GO.state != GameState.Purchase) {
            GetComponent<Image>().color = new Color(0.3f, 0.3f - red, 0.3f - red);
        } else {
            if (HeroDecks.HD.myManager.cardList[thisCard] != null) { 
                if (HeroDecks.HD.myManager.Charge < HeroDecks.HD.myManager.cardList[thisCard].cost) {
                    GetComponent<Image>().color = new Color(0.3f, 0.3f - red, 0.3f - red);
                } else if (zoomCard) {
                    GetComponent<Image>().color = new Color(0.8f, 0.8f - red, 0.8f - red);
                } else {
                    GetComponent<Image>().color = new Color(0.6f, 0.6f - red, 0.6f - red);
                }
            } else {
                if (zoomCard) {
                    GetComponent<Image>().color = new Color(0.8f, 0.8f - red, 0.8f - red);
                } else {
                    GetComponent<Image>().color = new Color(0.6f, 0.6f - red, 0.6f - red);
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

    // Acquire card (After buying it)
    void acquireCard()
    {
        if (GameOverseer.GO.state == GameState.Choice && bought)
        {
            bought = false;
            GameOverseer.GO.ultiBuy = false;
            GameOverseer.GO.enemyUltiBuy = false;
            if (deckManager.gameObject == HeroDecks.HD.myManager.myHand) // If Player
            {
                GameObject card = HeroDecks.HD.myManager.CreateCard(deckManager.cardAmount);
                card.GetComponent<CardInHand>().ultiCard = gameObject;

            
            } else if (deckManager.gameObject == HeroDecks.HD.enemyManager.myHand) { // If Enemy
                GameObject card = HeroDecks.HD.enemyManager.EnemyCreateCard(deckManager.cardAmount);
                card.GetComponent<EnemyCardInHand>().ultiCard = gameObject;
            }
            gameObject.SetActive(false);
        }
    }

    // Moving the card around and stuff

    public void ZoomCard()
    {
        //0.8665, 1.177
        transform.localPosition = Vector2.Lerp(transform.localPosition + new Vector3(0f, 5f, 0f),
                                                    deckManager.ultiLocation, Time.deltaTime * 5f);
        transform.localScale = new Vector3(2 * 0.8665f, 2 * 1.177f, 1f);
        gameObject.GetComponent<Canvas>().overrideSorting = true;

        // Buy the card
        //Debug.Log(deckManager.gameObject.name + ", " + HeroDecks.HD.myManager.myHand.name);
        if (Input.GetMouseButtonDown(0) && deckManager.gameObject == HeroDecks.HD.myManager.myHand && GameOverseer.GO.state == GameState.Purchase) {
            Debug.Log("mouse down");
            if (!bought) {
                if (HeroDecks.HD.myManager.Charge >= HeroDecks.HD.myManager.cardList[thisCard].cost) {
                    HeroDecks.HD.myManager.Charge -= HeroDecks.HD.myManager.cardList[thisCard].cost;
                    bought = true;
                    GameOverseer.GO.ultiBuy = true;
                }
            } else {
                HeroDecks.HD.myManager.Charge += HeroDecks.HD.myManager.cardList[thisCard].cost;
                bought = false;
                GameOverseer.GO.ultiBuy = false;
            }
        }
    }

    public void ReturnCard()
    {
        // Stop enemy network hovering
        if (GameOverseer.GO.hoveringCard == cardIndex &&
           ((GameOverseer.GO.amIHoveringMyself == true && deckManager.gameObject == HeroDecks.HD.myManager.myHand) ||
           (GameOverseer.GO.amIHoveringMyself == false && deckManager.gameObject == HeroDecks.HD.enemyManager.myHand))) {
            GameOverseer.GO.hoveringCard = -1;
        }

        // Get back into position
        transform.localScale = new Vector3(0.8665f, 1.177f, 1f);
        transform.localPosition = Vector2.Lerp(transform.localPosition, deckManager.ultiLocation, Time.deltaTime * 5f);

        // When card gets back into position, remove override
        if (transform.localPosition.x >= deckManager.ultiLocation.x - 1f &&
            transform.localPosition.x <= deckManager.ultiLocation.x + 1f &&
            transform.localPosition.y >= deckManager.ultiLocation.y - 1f &&
            transform.localPosition.y <= deckManager.ultiLocation.y + 1f) {
            gameObject.GetComponent<Canvas>().overrideSorting = false;
            canvas.sortingOrder = 1;
        }
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
            canvas.sortingOrder = 3;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GetComponent<Image>().color = new Color(0.6f, 0.6f, 0.6f);
        zoomCard = false;
    }


}