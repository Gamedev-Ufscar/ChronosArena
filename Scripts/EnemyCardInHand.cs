using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCardInHand : MonoBehaviour
{
    public DeckManager deckManager;
    public int cardIndex;
    public int thisCard;
    public Canvas canvas;
    public Vector3 adaptedEnemyHoverPos;
    public GameObject cardPrefab;
    private float zValue = 12f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Summonar carta
        if (GameOverseer.GO.enemySentCard == true) { Debug.Log("EnemySentCard: " + GameOverseer.GO.enemySentCard); }
        if (transform.localScale == new Vector3(2 * 0.8665f, 2 * 1.177f, 1f) && GameOverseer.GO.enemySentCard == true && GameOverseer.GO.state == GameState.Choice)
        {
            GameOverseer.GO.enemySentCard = false;
            Debug.Log("Enemy card summoned");
            Summon();
        }

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
        }

        // When card gets back into position, remove override
        if (transform.localPosition.x >= deckManager.cardLocations[cardIndex].x - 1f &&
            transform.localPosition.x <= deckManager.cardLocations[cardIndex].x + 1f &&
            transform.localPosition.y >= deckManager.cardLocations[cardIndex].y - 1f &&
            transform.localPosition.y <= deckManager.cardLocations[cardIndex].y + 1f)
        {
            gameObject.GetComponent<Canvas>().overrideSorting = false;
        }
    }

    // Summon board card
    public void Summon()
    {
        GameOverseer.GO.enemyCardPlayed = HeroDecks.HD.RobotoDeck(thisCard);
        Vector3 v = Camera.main.ScreenToWorldPoint(new Vector3(adaptedEnemyHoverPos.x, adaptedEnemyHoverPos.y, zValue));
        GameObject g = Instantiate(cardPrefab, new Vector3(v.x, v.y, v.z), Quaternion.LookRotation(Vector3.back, Vector3.up));
        g.GetComponent<CardInBoard>().Activate(SlotsOnBoard.EnemyCard);

        Destroy(gameObject);
    }
}