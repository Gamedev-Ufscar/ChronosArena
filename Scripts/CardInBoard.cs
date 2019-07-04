using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardInBoard : MonoBehaviour
{
    private Card thisCard;
    private GameObject slot;

    //public bool isPlayer = true;

    public void Activate(SlotsOnBoard place)
    {
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
