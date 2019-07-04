using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragObj : MonoBehaviour
{
    private float distance = 12f;
    private bool moveCard = true;
    public GameObject reference;

    void Update()
    {
        // Soltar carta
        moveCard = false;
        //reference.GetComponent<CardInBoard>().Activate(SlotsOnBoard.PlayerCard);
        Destroy(gameObject);
    }

}
