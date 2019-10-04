using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardCard : MonoBehaviour
{
    private GameObject slot;
    private Player player;

    private DeckCard thisDeckCard;
    private UltimateCard thisUltiCard;
    private int revealAnimState = 0;
    private bool waiting = true;

    private Card cardPlayed;

    public void ConstructBoardCard(Card cardPlayed, Player player, DeckCard deckCard)
    {
        this.cardPlayed = cardPlayed;
        this.player = player;
        thisDeckCard = deckCard;

        transform.GetChild(0).GetComponent<TextMesh>().text = cardPlayed.GetName();
        transform.GetChild(1).GetComponent<TextMesh>().text = cardPlayed.typeString(cardPlayed.GetCardType());
        transform.GetChild(2).GetComponent<TextMesh>().text = cardPlayed.GetText().Replace("\\n", "\n");
        transform.GetChild(3).GetComponent<TextMesh>().text = cardPlayed.Value(1);
        transform.GetChild(4).GetComponent<TextMesh>().text = cardPlayed.Value(2);
        transform.GetChild(5).GetComponent<TextMesh>().text = cardPlayed.heroString(cardPlayed.GetHero());
    }

    public void ConstructBoardCard(Card cardPlayed, Player player, DeckCard deckCard, UltimateCard ultiCard)
    {
        thisUltiCard = ultiCard;
        ConstructBoardCard(cardPlayed, player, deckCard);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (waiting == false)
        {
            waiting = true;
            if (revealAnimState == 0)
            {
                if (thisDeckCard is HandCard)
                    Activate(SlotsOnBoard.PlayerCardAbove, true);
                else if (thisDeckCard is EnemyCard)
                    Activate(SlotsOnBoard.EnemyCardAbove, true);
            }
            else if (revealAnimState == 1)
            {
                if (thisDeckCard is HandCard)
                    Activate(SlotsOnBoard.PlayerCard, true);
                else if (thisDeckCard is EnemyCard)
                    Activate(SlotsOnBoard.EnemyCard, true);
            }
            else if (revealAnimState == 2)
            {
                if (thisDeckCard is HandCard)
                    Activate(SlotsOnBoard.PlayerCard, false);
                else if (thisDeckCard is EnemyCard)
                    Activate(SlotsOnBoard.EnemyCard, false);
            }
        }
    }

    // Getter
    public Card GetCardPlayed()
    {
        return cardPlayed;
    }

    public DeckCard GetThisDeckCard()
    {
        return thisDeckCard;
    }

    public UltimateCard GetThisUltiCard()
    {
        return thisUltiCard;
    }

    // Setter
    public void SetAnimState(int animState)
    {
        this.revealAnimState = animState;
    }

    public void RaiseAnimState()
    {
        revealAnimState++;
    }

    public void SetWaiting(bool waiting)
    {
        this.waiting = waiting;
    }

    public void Activate(SlotsOnBoard place, bool faceUp)
    {
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
        AudioManager.AM.CardSound();
        slot.GetComponent<PlaceCard>().PlaceOnSlot(gameObject, faceUp);
    }
}
