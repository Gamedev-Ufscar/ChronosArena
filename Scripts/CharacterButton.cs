using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterButton : Button
{
    public SelectionOverseer selectionOverseer;
    int characterIndex;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Getter
    public int GetCharIndex()
    {
        return characterIndex;
    }

    public override void PointerDown()
    {
        selectionOverseer.CharacterClicked(this);
    }

    public override void PointerEnter()
    {
        selectionOverseer.CharacterHover(this);
    }

    public override void PointerExit()
    {
        selectionOverseer.CharacterStopHover();
    }
}
