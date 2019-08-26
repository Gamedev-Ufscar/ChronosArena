using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HoverBoardCard : MonoBehaviour
{
    public LayerMask layerMask;
    public GameObject viewCard;
    private GameObject boardCardReader;
    private List<GameObject> destroyList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 3
            && (GameOverseer.GO.state == GameState.Revelation || GameOverseer.GO.state == GameState.Effects) 
            && !HeroDecks.HD.myManager.myHand.GetComponent<DeckManager>().holdingCard)
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            Physics.Raycast(mouseRay, out hitInfo, 300, layerMask, QueryTriggerInteraction.Collide);

            Debug.Log("I'm alive");

            if (hitInfo.collider != null)
            {
                if (hitInfo.collider.transform.parent != null)
                {
                    Debug.Log("HoverCard");
                    GameObject cardInBoard = hitInfo.collider.transform.parent.gameObject;

                    boardCardReader = Instantiate(viewCard, Camera.main.WorldToScreenPoint(hitInfo.collider.transform.position), Quaternion.identity, HeroDecks.HD.myManager.myHand.transform);
                    destroyList.Add(boardCardReader);
                    boardCardReader.GetComponent<CardInHand>().cardCategory = 2;
                    boardCardReader.GetComponent<CardInHand>().deckManager = HeroDecks.HD.myManager.myHand.GetComponent<DeckManager>();
                    boardCardReader.GetComponent<Image>().sprite = cardInBoard.GetComponent<CardInBoard>().cardSprite;
                    boardCardReader.transform.GetChild(0).GetComponent<Text>().text = cardInBoard.transform.GetChild(0).GetComponent<TextMesh>().text;
                    boardCardReader.transform.GetChild(1).GetComponent<Text>().text = cardInBoard.transform.GetChild(1).GetComponent<TextMesh>().text;
                    boardCardReader.transform.GetChild(2).GetComponent<Text>().text = cardInBoard.transform.GetChild(2).GetComponent<TextMesh>().text;
                    boardCardReader.transform.GetChild(3).GetComponent<Text>().text = cardInBoard.transform.GetChild(3).GetComponent<TextMesh>().text;
                    boardCardReader.transform.GetChild(4).GetComponent<Text>().text = cardInBoard.transform.GetChild(4).GetComponent<TextMesh>().text;
                    boardCardReader.transform.GetChild(5).GetComponent<Text>().text = cardInBoard.transform.GetChild(5).GetComponent<TextMesh>().text;
                }
            } else if (boardCardReader != null)
            {
                foreach (GameObject destroyed in destroyList)
                {
                    Destroy(destroyed);
                }
            }
        }

        
        
    }
}
