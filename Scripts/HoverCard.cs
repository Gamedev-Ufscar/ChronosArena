using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoverCard : UICard
{
    // Start is called before the first frame update
    new void Start()
    {
        ChangeScale(Constants.cardBigSize(GetIsMobile()));
        ChangePosition(transform.localPosition);

        base.Start();
    }

    public void ConstructHoverCard(GameObject cardInBoard)
    {
        GetComponent<Image>().sprite = cardInBoard.GetComponent<BoardCard>().GetCardPlayed().GetImage();

        for (int i = 0; i <= 5; i++) {
            transform.GetChild(i).GetComponent<Text>().text = cardInBoard.transform.GetChild(i).GetComponent<TextMesh>().text;
        }
    }

    public void ConstructHoverCard(DeckCard deckCard)
    {
        GetComponent<Image>().sprite = deckCard.GetCard().GetImage();

        for (int i = 0; i <= 5; i++)
        {
            transform.GetChild(i).GetComponent<Text>().text = deckCard.transform.GetChild(i).GetComponent<Text>().text;
        }
    }
}
