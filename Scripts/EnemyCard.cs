using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EnemyCard : DeckCard
{
    private Card card;
    private Deck deck;
    private bool beingHeld = false;
    private Vector2 center = new Vector2(0f, 0f);
    private bool isReaction = false;
    private bool outOfHand = false;

    private GameObject ultiCard;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Control Position
        if (beingHeld && !isReaction) {

        } else {
            ReturnCard();
        }
    }

    // Constructor
    public EnemyCard(Card card, Deck deck) : base(card, deck)
    {
    }
}
