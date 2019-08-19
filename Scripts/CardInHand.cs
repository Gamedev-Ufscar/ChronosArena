using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardInHand : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    public bool zoomCard = false;
    public DeckManager deckManager;
    public int cardIndex;
    public bool moveCard;
    public bool reactionCard = false;

    public GameObject cardPrefab;
    public Sprite cardSprite;
    public int thisCard;

    private bool outOfHand = false;
    private bool cardBeingHeld = false;
    private Vector2 center = new Vector2(0f, 0f);
    private float zValue = 12f;

    [HideInInspector]
    public GameObject ultiCard;

    // Start is called before the first frame update
    void Start()
    {
        // Check PointerHandler for Pointer-related problems
        GetComponent<Image>().color = new Color(0.6f, 0.6f, 0.6f);
        GetComponentInChildren<Text>().text = HeroDecks.HD.myManager.cardList[thisCard].text;
    }

    // Update is called once per frame
    void Update()
    {
        // Moving the card around and stuff
        // Zoom Card = Pointer Over; Move Card = Pointer Down
        if ((zoomCard == true || moveCard == true) && HeroDecks.HD.interfaceScript.gameObject.activeInHierarchy == false) {
            ZoomCard();
        } else {
            ReturnCard();
        }

        if (HeroDecks.HD.myManager.cardList[thisCard] != null)
        {
            // Summon card to board
            if (outOfHand == true && Input.GetMouseButtonUp(0) && HeroDecks.HD.myManager.cardList[thisCard].turnsTillPlayable <= 0 &&
                !reactionCard) {
                if (HeroDecks.HD.myManager.cardList[thisCard].isReaction == false && GameOverseer.GO.state == GameState.Choice
                    && GameOverseer.GO.myCardPlayed == 200) {
                    Debug.Log("Activated sentCard");
                    GameOverseer.GO.sentCard = 5;
                    Summon();
                    outOfHand = false;
                } else if (HeroDecks.HD.myManager.cardList[thisCard].isReaction && GameOverseer.GO.state == GameState.Revelation) {
                    GameOverseer.GO.sentCard = 5;
                    RevealReaction();
                }
            }

           // Se não é jogável, fica BEM escuro
            if (HeroDecks.HD.myManager.cardList[thisCard].turnsTillPlayable > 0) {
                GetComponent<Image>().color = new Color(0.3f, 0.3f, 0.3f);
            } else if (GetComponent<Image>().color == new Color(0.3f, 0.3f, 0.3f)) {
                GetComponent<Image>().color = new Color(0.6f, 0.6f, 0.6f);
            }
        }

        // If reaction, discard at the end of turn
        if (GameOverseer.GO.state == GameState.Purchase && reactionCard)
        {
            discardingReaction();
        }
    }



    // Moving the card around and stuff

    public void ZoomCard()
    {
        //0.8665, 1.177
        transform.localScale = new Vector3(HeroDecks.HD.cardZoomSize * 0.8665f, HeroDecks.HD.cardZoomSize * 1.177f, 1f);
        transform.SetAsLastSibling();
        GameOverseer.GO.hoveringCard = cardIndex;
        GameOverseer.GO.hoveringCardPos = transform.position;
        GameOverseer.GO.hoveringCardLocalPos = transform.localPosition;
        GameObject.Find("Main UI").GetComponent<MainUIManager>().hoveredCard = HeroDecks.HD.myManager.cardList[thisCard].name;

        // Holding down the card
        if (Input.GetMouseButton(0) && !reactionCard) {
            Debug.Log(HeroDecks.HD.myManager.cardList[thisCard].name);
            HoldCard();
        } else {
            cardBeingHeld = false;
            deckManager.holdingCard = false;
            if (!reactionCard)
                transform.localPosition = Vector2.Lerp(transform.localPosition + new Vector3(0f, 7.5f, 0f),
                                                    deckManager.cardLocations[cardIndex], Time.deltaTime * 6f);
            else
                transform.localPosition = Vector2.Lerp(transform.localPosition + new Vector3(0f, 7.5f, 0f),
                                                    deckManager.reactionLocations[0], Time.deltaTime * 6f);
            moveCard = false;
        }
    }

    public void HoldCard()
    {
        // Determine initial offset
        if (cardBeingHeld == false) {
            center = transform.position - Input.mousePosition;
        }

        // Control offset
        transform.position = new Vector3(center.x, center.y) + Input.mousePosition;
        center = Vector3.Lerp(center, new Vector3(0f, 0f, 0f), Time.deltaTime * 5f);

        // Control variables and override
        deckManager.holdingCard = true;
        moveCard = true;
        cardBeingHeld = true;
    }

    public void ReturnCard()
    {
        // Stop enemy network hovering
        if (GameOverseer.GO.hoveringCard == cardIndex)
        {
            GameOverseer.GO.hoveringCard = 200;
        }

        // Get back into position (or reaction position)
        transform.localScale = new Vector3(0.8665f, 1.177f, 1f);
        if (!reactionCard) 
            transform.localPosition = Vector2.Lerp(transform.localPosition, deckManager.cardLocations[cardIndex], Time.deltaTime * 5f);
        else
            transform.localPosition = Vector2.Lerp(transform.localPosition, deckManager.reactionLocations[0], Time.deltaTime * 5f);

        // When card gets back into position, remove override
        if (transform.localPosition.x >= deckManager.cardLocations[cardIndex].x - 1f &&
            transform.localPosition.x <= deckManager.cardLocations[cardIndex].x + 1f &&
            transform.localPosition.y >= deckManager.cardLocations[cardIndex].y - 1f &&
            transform.localPosition.y <= deckManager.cardLocations[cardIndex].y + 1f) {
            transform.SetAsFirstSibling();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // If not already holding something, zoom the card
        if (deckManager.holdingCard == false)
        {
            GetComponent<Image>().color = new Color(1f, 1f, 1f);
            transform.localPosition = transform.localPosition + new Vector3(0f, 5f, 0f);
            zoomCard = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GetComponent<Image>().color = new Color(0.6f, 0.6f, 0.6f);
        zoomCard = false;
    }


    // Summon board card
    public void Summon()
    {
        GameOverseer.GO.myCardPlayed = thisCard;

        // Invoke physical card
        Vector3 v = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, zValue));
        GameObject g = Instantiate(cardPrefab, new Vector3(v.x, v.y, v.z), Quaternion.LookRotation(Vector3.back, Vector3.down));

        // Setup variables
        g.GetComponent<CardInBoard>().thisCard = thisCard;
        g.GetComponent<CardInBoard>().owner = HeroDecks.HD.myManager;
        deckManager.holdingCard = false;

        // Setup text
        g.GetComponentInChildren<TextMesh>().text = HeroDecks.HD.myManager.cardList[thisCard].text;

        // Activate slot
        if (GameOverseer.GO.predicted == false) { g.GetComponent<CardInBoard>().Activate(SlotsOnBoard.PlayerCard, false); }
        else { g.GetComponent<CardInBoard>().Activate(SlotsOnBoard.PlayerCard, true); }

        // Preparing to turn this on later
        g.GetComponent<CardInBoard>().thisCardInHand = gameObject;
        if (HeroDecks.HD.myManager.cardList[GameOverseer.GO.myCardPlayed].type == CardTypes.Ultimate) {
            g.GetComponent<CardInBoard>().thisUltimateCard = ultiCard;
        }
        gameObject.SetActive(false);
    }

    // Reveal reaction card
    public void RevealReaction()
    {
        reactionCard = true;
        HeroDecks.HD.myManager.cardList[thisCard].effect(HeroDecks.HD.myManager, HeroDecks.HD.enemyManager, 0);
    }

    void discardingReaction()
    {
        HeroDecks.HD.myManager.myHand.GetComponent<DeckManager>().recedeDeck(cardIndex);
        HeroDecks.HD.myManager.myHand.GetComponent<DeckManager>().activeCardCount--;

        // Destroy Card in Hand if Ultimate
        if (HeroDecks.HD.myManager.cardList[thisCard].type == CardTypes.Ultimate)
        {
            // Recover ultimate card
            ultiCard.SetActive(true);
            Destroy(gameObject);
        } else
        {
            gameObject.SetActive(false);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Trigger Exit");
        if (other.tag == "Hand")
        {
            outOfHand = false;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        //Debug.Log("Trigger Exit");
        if (other.tag == "Hand")
        {
            //Summon();
            outOfHand = true;
        }
    }

}
