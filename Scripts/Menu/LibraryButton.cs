using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LibraryButton : MenuButton
{
    [SerializeField]
    private LibraryOverseer libraryOverseer;
    [SerializeField]
    private int sheet = 0;
    [SerializeField]
    private HeroEnum hero;

    // Start is called before the first frame update
    void Start()
    {

    }

    public override void PointerDown()
    {
        libraryOverseer.OpenInfo(hero);
    }

    public override void RightPointerDown() { }

    public override void PointerEnter()
    {
        base.PointerEnter();
        libraryOverseer.SetCurrentSheet(sheet);
        libraryOverseer.UpdateStatus();
    }

    public override void PointerExit()
    {
        base.PointerExit();
        libraryOverseer.SetCurrentSheet(200);
        libraryOverseer.UpdateStatus();
    }

}