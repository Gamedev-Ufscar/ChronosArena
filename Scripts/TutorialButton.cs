using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialButton : Button
{
    private TutorialOverseer tutorialOverseer = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
