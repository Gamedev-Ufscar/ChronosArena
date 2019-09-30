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

    public void ChangeScale(float scale)
    {
        transform.localScale = new Vector3(1.210431f*scale, 1.272552f*scale);
    }

    public override void PointerDown()
    {
        player.OnShufflePress();
        ChangeScale(0.9f);
    }

    public override void RightPointerDown() {
        player.InvokeSummary();
        ChangeScale(0.9f);
    }

    public override void PointerUp()
    {
        ChangeScale(1f);
    }

    public override void RightPointerUp()
    {
    }

    public override void PointerEnter()
    {

    }

    public override void PointerExit()
    {
        ChangeScale(1f);
    }
}
