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
    public int revealAnimState = 0;
    public bool waiting = false;

    //public bool isPlayer = true;

    // Update is called once per frame
    void Update()
    {
        if (GameOverseer.GO.state == GameState.Purchase) {
            Resett();
        } else if (GameOverseer.GO.state == GameState.Revelation && waiting == false) {
            waiting = true;
            if (revealAnimState == 0) {
                if (thisCardInHand.GetComponent<CardInHand>() != null)
                    Activate(SlotsOnBoard.PlayerCardAbove, true);
                else if (thisCardInHand.GetComponent<EnemyCardInHand>() != null)
                    Activate(SlotsOnBoard.EnemyCardAbove, true);
            } else if (revealAnimState == 1) {
                if (thisCardInHand.GetComponent<CardInHand>() != null)
                    Activate(SlotsOnBoard.PlayerCard, true);
                else if (thisCardInHand.GetComponent<EnemyCardInHand>() != null)
                    Activate(SlotsOnBoard.EnemyCard, true);
            } else if (revealAnimState == 2) {
                if (thisCardInHand.GetComponent<CardInHand>() != null)
                    Activate(SlotsOnBoard.PlayerCard, false);
                else if (thisCardInHand.GetComponent<EnemyCardInHand>() != null)
                    Activate(SlotsOnBoard.EnemyCard, false);
            }
        }
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
            HeroDecks.HD.audioManager.CardSound();
            thisCardInHand.SetActive(true);

            // Dont have it zoomed
            if (owner == HeroDecks.HD.myManager && thisCardInHand != null) {
                thisCardInHand.GetComponent<CardInHand>().zoomCard = false;
                thisCardInHand.GetComponent<CardInHand>().moveCard = false;
                thisCardInHand.GetComponent<Image>().color = new Color(0.6f, 0.6f, 0.6f);
            } else if (owner == HeroDecks.HD.enemyManager) {
                GameOverseer.GO.enemyHoveringCard = -1;
            }

        // Recede deck if not returning & add this to discard deck list
        } else {
            if (owner == HeroDecks.HD.myManager) {
                owner.myHand.GetComponent<DeckManager>().recedeDeck(thisCardInHand.GetComponent<CardInHand>().cardIndex);
            } else if (owner == HeroDecks.HD.enemyManager) {
                owner.myHand.GetComponent<DeckManager>().recedeDeck(thisCardInHand.GetComponent<EnemyCardInHand>().cardIndex);
            }

            // Destroy Card in Hand if Ultimate
            if (owner.cardList[thisCard].type == CardTypes.Ultimate) {
                owner.myHand.GetComponent<DeckManager>().ultisInHand--;
                thisUltimateCard.SetActive(true);
                Destroy(thisCardInHand);
            }

            owner.myHand.GetComponent<DeckManager>().activeCardCount--;

        }

        Destroy(gameObject);
    }

    public void Activate(SlotsOnBoard place, bool faceUp) {
        switch (place)
        {
            case SlotsOnBoard.PlayerCard:
                slot = GameObject.FindWithTag("Slot/PlayerCard");
                break;

            case SlotsOnBoard.EnemyCard:
                slot = GameObject.FindWithTag("Slot/EnemyCard");
                break;

            case SlotsOnBoard.PlayerCardAbove:
                slot = GameObject.FindWithTag("Slot/PlayerCardAbove");
                break;

            case SlotsOnBoard.EnemyCardAbove:
                slot = GameObject.FindWithTag("Slot/EnemyCardAbove");
                break;
        }
        HeroDecks.HD.audioManager.CardSound();
        slot.GetComponent<PlaceCard>().PlaceOnSlot(gameObject, faceUp);
    }
}
