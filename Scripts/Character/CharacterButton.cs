using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterButton : Button
{
    [SerializeField]
    private SelectionOverseer selectionOverseer;
    [SerializeField]
    int characterIndex;

    bool selected = false;
    
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

    public bool GetSelected()
    {
        return selected;
    }

    // Setter
    public void ChangeScale(float scale)
    {
        transform.localScale = new Vector3(scale, scale);
    }

    public void SetSelected(bool selected)
    {
        this.selected = selected;
    }

    // Pointer
    public override void PointerDown()
    {
        selectionOverseer.JuicyButton(this);
    }

    public override void RightPointerDown() { }

    public override void PointerUp()
    {
        selectionOverseer.HeroClicked(this);
        Debug.Log("Up!");
    }

    public override void RightPointerUp()
    {
    }

    public override void PointerEnter()
    {
        selectionOverseer.HeroHover(this);
    }

    public override void PointerExit()
    {
        selectionOverseer.HeroStopHover(this);
    }
}
