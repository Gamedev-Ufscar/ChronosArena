using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialButton : Button
{
    private TutorialOverseer tutorialOverseer = null;

    // Setters
    public void SetTutorialOverseer(TutorialOverseer overseer)
    {
        this.tutorialOverseer = overseer;
    }

    public override void PointerEnter()
    {
        ChangeScale(1.1f);
    }

    public override void PointerExit()
    {
        ChangeScale(1f);
    }

    public override void PointerUp()
    {

   
    }

    public override void RightPointerUp()
    {
    }

    public override void PointerDown()
    {
        if (tutorialOverseer != null)
            tutorialOverseer.ReceiveClick(this);
    }

    public override void RightPointerDown() { }
}
