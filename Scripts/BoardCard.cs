using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardCard : MonoBehaviour
{
    private GameObject slot;
    private Player player;

    private DeckCard thisDeckCard;
    private UltimateCard thisUltiCard;

    private Card cardPlayed;

    public void ConstructBoardCard(Card cardPlayed, Player player, DeckCard deckCard)
    {
        this.cardPlayed = cardPlayed;
        this.player = player;
        thisDeckCard = deckCard;

        transform.GetChild(6).GetComponent<Renderer>().material.mainTexture = ImageStash.IS.textureFromSprite(cardPlayed.GetImage());

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
        
    }

    public void RevealAnimation(int state) {
        if (state == -1)
        {
            if (thisDeckCard is HandCard)
                Activate(SlotsOnBoard.PlayerCard, false, null);
            else if (thisDeckCard is EnemyCard)
                Activate(SlotsOnBoard.EnemyCard, false, null);
        }
        else if (state == -2)
        {
            if (thisDeckCard is HandCard)
                Activate(SlotsOnBoard.PlayerCard, true, null);
            else if (thisDeckCard is EnemyCard)
                Activate(SlotsOnBoard.EnemyCard, true, null);
        }
        else if (state == 0)
        {
            if (thisDeckCard is HandCard) {
                Deactivate(SlotsOnBoard.PlayerCard);
                Activate(SlotsOnBoard.PlayerCardAbove, true, 1);
            } else if (thisDeckCard is EnemyCard) {
                Deactivate(SlotsOnBoard.EnemyCard);
                Activate(SlotsOnBoard.EnemyCardAbove, true, 1);
            }
        }
        else if (state == 1)
        {
            if (thisDeckCard is HandCard)
                Activate(SlotsOnBoard.PlayerCard, true, null);
            else if (thisDeckCard is EnemyCard)
                Activate(SlotsOnBoard.EnemyCard, true, null);
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

    public void Activate(SlotsOnBoard place, bool faceUp, int? nextState)
    {
        slot = FindSlot(place);
        AudioManager.AM.CardSound();
        slot.GetComponent<PlaceCard>().PlaceOnSlot(gameObject, faceUp, nextState);
    }

    public void Deactivate(SlotsOnBoard place)
    {
        slot = FindSlot(place);
        slot.GetComponent<PlaceCard>().Stop();
    }

    GameObject FindSlot(SlotsOnBoard place)
    {
        switch (place)
        {
            case SlotsOnBoard.PlayerCard:
                return GameObject.FindWithTag("Slot/PlayerCard");

            case SlotsOnBoard.EnemyCard:
                return GameObject.FindWithTag("Slot/EnemyCard");

            case SlotsOnBoard.PlayerCardAbove:
                return GameObject.FindWithTag("Slot/PlayerCardAbove");

            case SlotsOnBoard.EnemyCardAbove:
                return GameObject.FindWithTag("Slot/EnemyCardAbove");

            default:
                return null;
        }
    }
}
