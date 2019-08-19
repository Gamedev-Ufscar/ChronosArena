using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EnemyCardInHand : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    public DeckManager deckManager;
    public int cardIndex;
    public int thisCard;
    public bool reactionCard = false;
    public Vector3 adaptedEnemyHoverPos;
    public GameObject cardPrefab;
    private float zValue = 12f;

    public GameObject ultiCard;

    private bool zoomCard = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (reactionCard && zoomCard) { // Jogador dar zoom na carta de reação
            transform.localScale = new Vector3(HeroDecks.HD.cardZoomSize * 0.8665f, HeroDecks.HD.cardZoomSize * 1.177f, 1f);
            transform.SetAsLastSibling();
            transform.localPosition = Vector2.Lerp(transform.localPosition, deckManager.reactionLocations[0] + new Vector2(0f, 5f),
                Time.deltaTime * 5f);

        // Get back into position
        } else if (GameOverseer.GO.enemyHoveringCard != cardIndex) // Se a carta do inimigo não for esta...
        {
            transform.localScale = new Vector3(0.8665f, 1.177f, 1f);
            if (!reactionCard)
                transform.localPosition = Vector2.Lerp(transform.localPosition, deckManager.cardLocations[cardIndex], Time.deltaTime * 5f);
            else
                transform.localPosition = Vector2.Lerp(transform.localPosition, deckManager.reactionLocations[0], Time.deltaTime * 5f);
            transform.SetAsFirstSibling();
        }
        else if (reactionCard) { // Inimigo dar zoom na carta de reação
            transform.localScale = new Vector3(HeroDecks.HD.cardZoomSize * 0.8665f, HeroDecks.HD.cardZoomSize * 1.177f, 1f);
            transform.SetAsLastSibling();
            transform.localPosition = Vector2.Lerp(transform.localPosition, deckManager.reactionLocations[0] + new Vector2(0f, 5f),
                Time.deltaTime * 5f);

        } else { // Mover carta do inimigo
            transform.localScale = new Vector3(HeroDecks.HD.cardZoomSize * 0.8665f, HeroDecks.HD.cardZoomSize * 1.177f, 1f);
            adaptedEnemyHoverPos = new Vector3(-GameOverseer.GO.enemyHoveringCardLocalPos.x, 2-GameOverseer.GO.enemyHoveringCardLocalPos.y);
            transform.localPosition = Vector2.Lerp(transform.localPosition, adaptedEnemyHoverPos, Time.deltaTime * 5f);
            transform.SetAsLastSibling();

            // Summonar carta
            if (GameOverseer.GO.enemySentCard == true)
            {
                if (GameOverseer.GO.state == GameState.Choice)
                {
                    GameOverseer.GO.enemySentCard = false;
                    GameOverseer.GO.alreadyReceived = true;
                    Summon();
                } else if (GameOverseer.GO.state == GameState.Revelation)
                {
                    GameOverseer.GO.enemySentCard = false;
                    GameOverseer.GO.alreadyReceived = true;
                    RevealReaction();
                }
            }
        }

        // When card gets back into position, remove override
        if (transform.localPosition.x >= deckManager.cardLocations[cardIndex].x - 1f &&
            transform.localPosition.x <= deckManager.cardLocations[cardIndex].x + 1f &&
            transform.localPosition.y >= deckManager.cardLocations[cardIndex].y - 1f &&
            transform.localPosition.y <= deckManager.cardLocations[cardIndex].y + 1f)
        {
            transform.SetAsFirstSibling();
        }

        // Cores - se inimigo estiver olhando, fica mais vermelho
        if (GameOverseer.GO.enemyHoveringCard == cardIndex) {
            GetComponent<Image>().color = new Color(1f, 0.8f, 0.8f);
        } else if (zoomCard) {
            GetComponent<Image>().color = new Color(1f, 1f, 1f);
        } else {
            GetComponent<Image>().color = new Color(0.8f, 0.8f, 0.8f);
        }

        // If reaction, discard at the end of turn
        if (GameOverseer.GO.state == GameState.Purchase && reactionCard)
        {
            discardingReaction();
        }
    }

    // Summon board card
    public void Summon()
    {
        GameOverseer.GO.enemyCardPlayed = thisCard;
        //Debug.Log(thisCard);

        // Invoke physical card
        Vector3 v = Camera.main.ScreenToWorldPoint(new Vector3(adaptedEnemyHoverPos.x, adaptedEnemyHoverPos.y, zValue));
        GameObject g = Instantiate(cardPrefab, new Vector3(v.x, v.y, v.z), Quaternion.LookRotation(Vector3.back, Vector3.up));

        // Setup variables
        g.GetComponent<CardInBoard>().thisCard = thisCard;
        g.GetComponent<CardInBoard>().owner = HeroDecks.HD.enemyManager;


        // Show Predicted Card
        if (GameOverseer.GO.enemyPredicted == false) { g.GetComponent<CardInBoard>().Activate(SlotsOnBoard.EnemyCard, false); }
        else { g.GetComponent<CardInBoard>().Activate(SlotsOnBoard.EnemyCard, true); }

        // Setup variables
        g.GetComponent<CardInBoard>().thisCardInHand = gameObject;
        if (HeroDecks.HD.enemyManager.cardList[GameOverseer.GO.enemyCardPlayed].type == CardTypes.Ultimate) {
            g.GetComponent<CardInBoard>().thisUltimateCard = ultiCard;
        }

        // Setup text
        g.GetComponentInChildren<TextMesh>().text = HeroDecks.HD.enemyManager.cardList[thisCard].text;

        // Show Predicted card
        if (GameOverseer.GO.enemyPredicted) {
            GameObject.Find("Main UI").GetComponent<MainUIManager>().enemyRevealedCard = HeroDecks.HD.enemyManager.cardList[thisCard].name;
        }

        // Turn off card in hand
        gameObject.SetActive(false);
    }

    // Reveal reaction card
    public void RevealReaction()
    {
        reactionCard = true;
        GetComponent<Image>().sprite = HeroDecks.HD.enemyManager.cardList[thisCard].image; 
        HeroDecks.HD.enemyManager.cardList[thisCard].effect(HeroDecks.HD.enemyManager, HeroDecks.HD.myManager, 0);
    }

    void discardingReaction()
    {
        HeroDecks.HD.enemyManager.myHand.GetComponent<DeckManager>().recedeDeck(cardIndex);
        HeroDecks.HD.enemyManager.myHand.GetComponent<DeckManager>().activeCardCount--;

        // Destroy Card in Hand if Ultimate
        if (HeroDecks.HD.enemyManager.cardList[thisCard].type == CardTypes.Ultimate) {

            // Recover ultimate card
            ultiCard.SetActive(true);
            Destroy(gameObject);
        } else {
            gameObject.SetActive(false);
        }

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // If not already holding something, zoom the card
        if (deckManager.holdingCard == false)
        {
            zoomCard = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        zoomCard = false;
    }
}