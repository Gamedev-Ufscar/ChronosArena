using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCardInHand : MonoBehaviour
{
    public DeckManager deckManager;
    public int cardIndex;
    public int thisCard;
    public Canvas canvas;
    public Vector3 adaptedEnemyHoverPos;
    public GameObject cardPrefab;
    private float zValue = 12f;

    public GameObject ultiCard;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Get back into position
        if (GameOverseer.GO.enemyHoveringCard != cardIndex) // Se a carta do inimigo não for esta...
        {
            transform.localScale = new Vector3(0.8665f, 1.177f, 1f);
            transform.localPosition = Vector2.Lerp(transform.localPosition, deckManager.cardLocations[cardIndex], Time.deltaTime * 5f);
            gameObject.GetComponent<Canvas>().overrideSorting = false;
        }
        else { // Mover carta do inimigo
            transform.localScale = new Vector3(2 * 0.8665f, 2 * 1.177f, 1f);
            adaptedEnemyHoverPos = new Vector3(-GameOverseer.GO.enemyHoveringCardLocalPos.x, 2-GameOverseer.GO.enemyHoveringCardLocalPos.y);
            transform.localPosition = Vector2.Lerp(transform.localPosition, adaptedEnemyHoverPos, Time.deltaTime * 5f);
            gameObject.GetComponent<Canvas>().overrideSorting = true;

            // Summonar carta
            if (GameOverseer.GO.enemySentCard == true && GameOverseer.GO.state == GameState.Choice)
            {
                GameOverseer.GO.enemySentCard = false;
                Debug.Log("Enemy card summoned " + cardIndex);
                Summon();
            }
        }

        // When card gets back into position, remove override
        if (transform.localPosition.x >= deckManager.cardLocations[cardIndex].x - 1f &&
            transform.localPosition.x <= deckManager.cardLocations[cardIndex].x + 1f &&
            transform.localPosition.y >= deckManager.cardLocations[cardIndex].y - 1f &&
            transform.localPosition.y <= deckManager.cardLocations[cardIndex].y + 1f)
        {
            gameObject.GetComponent<Canvas>().overrideSorting = false;
        }

        // Se inimigo estiver olhando, fica mais vermelho
        if (GameOverseer.GO.enemyHoveringCard == cardIndex) {
            GetComponent<Image>().color = new Color(1f, 0.8f, 0.8f);
        } else {
            GetComponent<Image>().color = new Color(1f, 1f, 1f);
        }
    }

    // Summon board card
    public void Summon()
    {
        GameOverseer.GO.enemyCardPlayed = thisCard;
        Vector3 v = Camera.main.ScreenToWorldPoint(new Vector3(adaptedEnemyHoverPos.x, adaptedEnemyHoverPos.y, zValue));
        GameObject g = Instantiate(cardPrefab, new Vector3(v.x, v.y, v.z), Quaternion.LookRotation(Vector3.back, Vector3.up));
        if (HeroDecks.HD.enemyManager.cardList[GameOverseer.GO.enemyCardPlayed].type != CardTypes.Ultimate) { 
            g.GetComponent<CardInBoard>().thisCardInHand = gameObject;
        } else {
            g.GetComponent<CardInBoard>().thisCardInHand = ultiCard;
            Debug.Log("Enemy ult summoned");
        }
        g.GetComponent<CardInBoard>().thisCard = thisCard;
        g.GetComponent<CardInBoard>().owner = HeroDecks.HD.enemyManager;
        g.GetComponent<CardInBoard>().Activate(SlotsOnBoard.EnemyCard);

        gameObject.SetActive(false);
    }
}