using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Profile : Button
{
    [SerializeField]
    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetImage(Sprite image)
    {
        GetComponent<Image>().sprite = image;
    }

    public override void PointerDown()
    {
        player.OnShufflePress();
    }

    public override void RightPointerDown() {
        player.InvokeSummary();
    }

    public override void PointerUp()
    {
    }

    public override void RightPointerUp()
    {
    }

    public override void PointerEnter()
    {

    }

    public override void PointerExit()
    {

    }
}
