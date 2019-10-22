public class TutorialEvent
{
    public TutorialEvent(int? nextButton, int? nextCard, int? highlightedCard)
    {
        this.nextButton = nextButton;
        this.nextCard = nextCard;
        this.highlightedCard = highlightedCard;
    }


    int? nextButton;
    int? nextCard;
    int? highlightedCard;

    int nextState;

    public void Event(int cardID)
    {

    }
}
