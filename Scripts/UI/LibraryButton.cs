using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LibraryButton : MenuButton
{
    [SerializeField]
    private LibraryHub libraryHub;
    [SerializeField]
    private int sheet = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    public override void PointerDown()
    {
    }

    public override void RightPointerDown() { }

    public override void PointerEnter()
    {
        base.PointerEnter();
        libraryHub.SetCurrentSheet(sheet);
        libraryHub.UpdateStatus();
    }

    public override void PointerExit()
    {
        base.PointerExit();
        libraryHub.SetCurrentSheet(200);
        libraryHub.UpdateStatus();
    }

}