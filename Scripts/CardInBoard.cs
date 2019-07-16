using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardInBoard : MonoBehaviour
{
    private GameObject slot;

    [HideInInspector]
    public GameObject thisCardInHand;
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

        if (owner.cardList[thisCard].type == CardTypes.Nullification) {
            Nullification cc = (Nullification)owner.cardList[thisCard];
            if (cc.wronged == false)
                returning = true;
        } else if (owner.cardList[thisCard].type != CardTypes.Skill && owner.cardList[thisCard].type != CardTypes.NeutralSkill) {
            returning = true;
        }

        if (returning) {
            thisCardInHand.SetActive(true);
            if (owner.name == "Player Manager" && thisCardInHand.GetComponent<CardInHand>() != null) {
                thisCardInHand.GetComponent<CardInHand>().zoomCard = false;
                thisCardInHand.GetComponent<CardInHand>().moveCard = false;
                thisCardInHand.GetComponent<Image>().color = new Color(0.6f, 0.6f, 0.6f);
            } else {
                GameOverseer.GO.enemyHoveringCard = -1;
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
