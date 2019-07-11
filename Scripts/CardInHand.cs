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
    public Canvas canvas;

    public GameObject cardPrefab;
    public Sprite cardSprite;
    public int thisCard;

    private bool outOfHand = false;
    private bool cardBeingHeld = false;
    private Vector2 center = new Vector2(0f, 0f);
    private float zValue = 12f;

    // Start is called before the first frame update
    void Start()
    {
        // Check PointerHandler for Pointer-related problems
    }

    // Update is called once per frame
    void Update()
    {
        //moveCard = EventSystem.current.IsPointerOverGameObject();

        // Moving the card around and stuff
        if (zoomCard == true || moveCard == true) // Zoom Card = Pointer Over; Move Card = Pointer Down
        {
            ZoomCard();
        } else
        {
            ReturnCard();
        }

        if (outOfHand == true && Input.GetMouseButtonUp(0) && GameOverseer.GO.state == GameState.Choice)
        {
            Debug.Log("Activated sentCard");
            GameOverseer.GO.sentCard = true;
            Summon();
            outOfHand = false;
        }
    }


    // Custom Card

    public void CustomCard(Sprite cardSprite)
    {

    }



    // Moving the card around and stuff

    public void ZoomCard()
    {
        //0.8665, 1.177
        transform.localScale = new Vector3(2 * 0.8665f, 2 * 1.177f, 1f);
        gameObject.GetComponent<Canvas>().overrideSorting = true;
        GameOverseer.GO.hoveringCard = cardIndex;
        GameOverseer.GO.hoveringCardPos = transform.position;
        GameOverseer.GO.hoveringCardLocalPos = transform.localPosition;

        // Holding down the card
        if (Input.GetMouseButton(0))
        {
            HoldCard();
        }
        else
        {
            cardBeingHeld = false;
            deckManager.holdingCard = false;
            transform.localPosition = Vector2.Lerp(transform.localPosition + new Vector3(0f, 5f, 0f),
                                                    deckManager.cardLocations[cardIndex], Time.deltaTime * 5f);
            moveCard = false;
        }
    }

    public void HoldCard()
    {
        Debug.Log(HeroDecks.HD.RobotoDeck(thisCard).name);
        // Determine initial offset
        if (cardBeingHeld == false)
        {
            center = transform.position - Input.mousePosition;
        }

        // Control offset
        transform.position = new Vector3(center.x, center.y) + Input.mousePosition;
        center = Vector3.Lerp(center, new Vector3(0f, 0f, 0f), Time.deltaTime * 5f);

        // Control variables and override
        deckManager.holdingCard = true;
        moveCard = true;
        cardBeingHeld = true;
        canvas.sortingOrder = 3;
    }

    public void ReturnCard()
    {
        // Stop enemy network hovering
        if (GameOverseer.GO.hoveringCard == cardIndex)
        {
            GameOverseer.GO.hoveringCard = -1;
        }

        // Get back into position
        transform.localScale = new Vector3(0.8665f, 1.177f, 1f);
        transform.localPosition = Vector2.Lerp(transform.localPosition, deckManager.cardLocations[cardIndex], Time.deltaTime * 5f);

        // When card gets back into position, remove override
        if (transform.localPosition.x >= deckManager.cardLocations[cardIndex].x - 1f &&
            transform.localPosition.x <= deckManager.cardLocations[cardIndex].x + 1f &&
            transform.localPosition.y >= deckManager.cardLocations[cardIndex].y - 1f &&
            transform.localPosition.y <= deckManager.cardLocations[cardIndex].y + 1f)
        {
            gameObject.GetComponent<Canvas>().overrideSorting = false;
            canvas.sortingOrder = 1;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // If not already holding something, zoom the card
        if (deckManager.holdingCard == false)
        {
            GetComponent<Image>().color = new Color(0.6f, 0.6f, 0.6f);
            transform.localPosition = transform.localPosition + new Vector3(0f, 5f, 0f);
            zoomCard = true;
            canvas.sortingOrder = 3;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {

        GetComponent<Image>().color = new Color(1f, 1f, 1f);
        zoomCard = false;
    }

    // Summon board card
    public void Summon()
    {
        GameOverseer.GO.myCardPlayed = HeroDecks.HD.RobotoDeck(thisCard);
        Vector3 v = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, zValue));
        GameObject g = Instantiate(cardPrefab, new Vector3(v.x, v.y, v.z), Quaternion.LookRotation(Vector3.back, Vector3.down));
        g.GetComponent<CardInBoard>().Activate(SlotsOnBoard.PlayerCard);

        deckManager.holdingCard = false;
        Destroy(gameObject);
    }

    public void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Trigger Exit");
        if (other.tag == "Hand")
        {
            outOfHand = false;
            Debug.Log("Hand Enter " + cardIndex);
        }
    }


    public void OnTriggerExit(Collider other)
    {
        //Debug.Log("Trigger Exit");
        if (other.tag == "Hand")
        {
            //Summon();
            outOfHand = true;
            Debug.Log("Hand Exit " + cardIndex);
        }
    }

}
