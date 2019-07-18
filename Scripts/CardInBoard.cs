using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardInBoard : MonoBehaviour
{
    private GameObject slot;

    [HideInInspector]
    public GameObject thisCardInHand;
    public GameObject thisUltimateCard;
    public int thisCard;
    public PlayerManager owner;

    //public bool isPlayer = true;

    // Update is called once per frame
    void Update()
    {
        if (GameOverseer.GO.state == GameState.Purchase)
            Resett();
    }

    public void Resett()
    {
        bool returning = false;

        // Should this card return?
        if (owner.cardList[thisCard].type == CardTypes.Nullification) {
            Nullification cc = (Nullification)owner.cardList[thisCard];
            if (cc.wronged == false)
            {
                returning = true;
            }
        } else if (owner.cardList[thisCard].type != CardTypes.Skill && owner.cardList[thisCard].type != CardTypes.NeutralSkill
                     && owner.cardList[thisCard].type != CardTypes.Ultimate) {
            returning = true;
        }

        // Returning...
        if (returning) {
            // Enable card again
            thisCardInHand.SetActive(true);

            // Dont have it zoomed
            if (owner == HeroDecks.HD.myManager && thisCardInHand != null) {
                thisCardInHand.GetComponent<CardInHand>().zoomCard = false;
                thisCardInHand.GetComponent<CardInHand>().moveCard = false;
                thisCardInHand.GetComponent<Image>().color = new Color(0.6f, 0.6f, 0.6f);
            } else if (owner == HeroDecks.HD.enemyManager) {
                GameOverseer.GO.enemyHoveringCard = -1;
            }

        // Recede deck if not returning
        } else {
            if (owner == HeroDecks.HD.myManager) {
                owner.myHand.GetComponent<DeckManager>().recedeDeck(thisCardInHand.GetComponent<CardInHand>().cardIndex);
            } else if (owner == HeroDecks.HD.enemyManager) {
                owner.myHand.GetComponent<DeckManager>().recedeDeck(thisCardInHand.GetComponent<EnemyCardInHand>().cardIndex);
            }

            owner.myHand.GetComponent<DeckManager>().activeCardCount--;

            // Destroy Card in Hand if Ultimate
            if (owner.cardList[thisCard].type == CardTypes.Ultimate) {
                thisUltimateCard.SetActive(true);
                Destroy(thisCardInHand);
            }

        }

        Destroy(gameObject);
    }

    public void Activate(SlotsOnBoard place) {
        switch (place)
        {
            case SlotsOnBoard.PlayerCard:
                slot = GameObject.FindWithTag("Slot/PlayerCard");
                break;

            case SlotsOnBoard.EnemyCard:
                slot = GameObject.FindWithTag("Slot/EnemyCard");
                break;
        }
        slot.GetComponent<PlaceCard>().PlaceOnSlot(gameObject);
    }
}
