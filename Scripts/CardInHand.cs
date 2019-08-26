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
    public int cardCategory = 0; // 0 = Normal Card; 1 = Reaction Card; 2 = Viewing Card (Board)

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
        if (cardCategory != 2)
        {
            cardSprite = HeroDecks.HD.myManager.cardList[thisCard].image;
            transform.GetComponent<Image>().sprite = HeroDecks.HD.myManager.cardList[thisCard].image;
            transform.GetChild(0).GetComponent<Text>().text = HeroDecks.HD.myManager.cardList[thisCard].name;
            transform.GetChild(1).GetComponent<Text>().text = HeroDecks.HD.myManager.cardList[thisCard].typeString(HeroDecks.HD.myManager.cardList[thisCard].type);
            transform.GetChild(2).GetComponent<Text>().text = HeroDecks.HD.myManager.cardList[thisCard].text.Replace("\\n", "\n");
            transform.GetChild(3).GetComponent<Text>().text = HeroDecks.HD.value(HeroDecks.HD.myManager.cardList[thisCard], 1);
            transform.GetChild(4).GetComponent<Text>().text = HeroDecks.HD.value(HeroDecks.HD.myManager.cardList[thisCard], 2);
            transform.GetChild(5).GetComponent<Text>().text = HeroDecks.HD.myManager.cardList[thisCard].heroString(HeroDecks.HD.myManager.hero);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (cardCategory == 2 && GameOverseer.GO.state == GameState.Purchase)
        {
            Destroy(gameObject);
        }

        // Moving the card around and stuff
        // Zoom Card = Pointer Over; Move Card = Pointer Down
        if (((zoomCard == true || moveCard == true) && HeroDecks.HD.interfaceScript.gameObject.activeInHierarchy == false)
            || cardCategory == 2) {
            ZoomCard();
        } else {
            ReturnCard();
        }

        if (HeroDecks.HD.myManager.cardList[thisCard] != null)
        {
            // Summon card to board
            if (outOfHand == true && Input.GetMouseButtonUp(0) && HeroDecks.HD.myManager.cardList[thisCard].turnsTillPlayable <= 0 &&
                cardCategory == 0) {
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
        if (GameOverseer.GO.state == GameState.Purchase && cardCategory == 1)
        {
            discardingReaction();
        }
    }



    // Moving the card around and stuff

    public void ZoomCard()
    {
        //0.8665, 1.177
        transform.localScale = new Vector3(HeroDecks.cardZoomSize * 0.8665f, HeroDecks.cardZoomSize * 1.177f, 1f);
        transform.SetAsLastSibling();
        if (cardCategory != 2)
        {
            GameOverseer.GO.hoveringCard = cardIndex;
            GameOverseer.GO.hoveringCardPos = transform.position;
            GameOverseer.GO.hoveringCardLocalPos = transform.localPosition;
        }
        GameObject.Find("Main UI").GetComponent<MainUIManager>().hoveredCard = HeroDecks.HD.myManager.cardList[thisCard].name;

        // Holding down the card
        if (Input.GetMouseButton(0) && cardCategory == 0) {
            Debug.Log(HeroDecks.HD.myManager.cardList[thisCard].name);
            HoldCard();
        } else {
            cardBeingHeld = false;
            deckManager.holdingCard = false;
            if (cardCategory == 0)
                transform.localPosition = Vector2.Lerp(transform.localPosition + new Vector3(0f, HeroDecks.cardMoveUp),
                                                    deckManager.cardLocations[cardIndex], 0.1f);
            else if (cardCategory == 1)
                transform.localPosition = Vector2.Lerp(transform.localPosition + new Vector3(0f, HeroDecks.cardMoveUp),
                                                    deckManager.reactionLocations[0], 0.1f);
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
        if (cardCategory == 0) {
            transform.localPosition = Vector2.Lerp(transform.localPosition, deckManager.cardLocations[cardIndex], Time.deltaTime * 5f);

            // When card gets back into position, remove override
            if (transform.localPosition.x >= deckManager.cardLocations[cardIndex].x - 1f &&
                transform.localPosition.x <= deckManager.cardLocations[cardIndex].x + 1f &&
                transform.localPosition.y >= deckManager.cardLocations[cardIndex].y - 1f &&
                transform.localPosition.y <= deckManager.cardLocations[cardIndex].y + 1f) {
                transform.SetAsFirstSibling();
            }
        } else if (cardCategory == 1) {
            transform.localPosition = Vector2.Lerp(transform.localPosition, deckManager.reactionLocations[0], Time.deltaTime * 5f);
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
        g.GetComponent<CardInBoard>().cardSprite = cardSprite;
        g.transform.GetChild(6).GetComponent<Renderer>().material.mainTexture = ImageStash.IS.textureFromSprite(cardSprite);
        g.transform.GetChild(0).GetComponent<TextMesh>().text = HeroDecks.HD.myManager.cardList[thisCard].name;
        g.transform.GetChild(1).GetComponent<TextMesh>().text = HeroDecks.HD.myManager.cardList[thisCard].typeString(HeroDecks.HD.myManager.cardList[thisCard].type);
        g.transform.GetChild(2).GetComponent<TextMesh>().text = HeroDecks.HD.myManager.cardList[thisCard].text.Replace("\\n", "\n");
        g.transform.GetChild(3).GetComponent<TextMesh>().text = HeroDecks.HD.value(HeroDecks.HD.myManager.cardList[thisCard], 1);
        g.transform.GetChild(4).GetComponent<TextMesh>().text = HeroDecks.HD.value(HeroDecks.HD.myManager.cardList[thisCard], 2);
        g.transform.GetChild(5).GetComponent<TextMesh>().text = HeroDecks.HD.myManager.cardList[thisCard].heroString(HeroDecks.HD.myManager.cardList[thisCard].hero);

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
        cardCategory = 1;
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
