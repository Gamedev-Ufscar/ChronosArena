using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoButton : Button
{
    [SerializeField]
    private SelectionOverseer selectionOverseer;


    public override void PointerDown() {
        transform.localScale = new Vector2(1.1f, 1.1f);
        selectionOverseer.OpenInfo();
    }

    public override void RightPointerDown() { }

    public override void PointerUp() {
        transform.localScale = new Vector2(1.2f, 1.2f);
    }

    public override void RightPointerUp() { }

    public override void PointerEnter() { }

    public override void PointerExit() {
        transform.localScale = new Vector2(1.2f, 1.2f);
    }

}