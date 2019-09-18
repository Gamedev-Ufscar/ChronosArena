using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoverCard : UICard
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public HoverCard(Card card) : base(card)
    {

    }

    public void ConstructHoverCard(GameObject cardInBoard)
    {
        GetComponent<Image>().sprite = cardInBoard.GetComponent<BoardCard>().GetCardPlayed().GetImage();
        transform.GetChild(0).GetComponent<Text>().text = cardInBoard.transform.GetChild(0).GetComponent<TextMesh>().text;
        transform.GetChild(1).GetComponent<Text>().text = cardInBoard.transform.GetChild(1).GetComponent<TextMesh>().text;
        transform.GetChild(2).GetComponent<Text>().text = cardInBoard.transform.GetChild(2).GetComponent<TextMesh>().text;
        transform.GetChild(3).GetComponent<Text>().text = cardInBoard.transform.GetChild(3).GetComponent<TextMesh>().text;
        transform.GetChild(4).GetComponent<Text>().text = cardInBoard.transform.GetChild(4).GetComponent<TextMesh>().text;
        transform.GetChild(5).GetComponent<Text>().text = cardInBoard.transform.GetChild(5).GetComponent<TextMesh>().text;
    }
}
