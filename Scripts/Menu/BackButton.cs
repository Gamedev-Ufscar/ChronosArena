using UnityEngine;

public class BackButton : Button
{
    [SerializeField]
    private Interface interfface;

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
        interfface.Close(null);
    }

    public override void RightPointerDown() { }

    public override void PointerUp()
    {

    }

    public override void RightPointerUp() { }

    public override void PointerEnter() {
        ChangeTone(1f);
        ChangeChildTone(0, 1f);
        ChangeChildTone(1, 1f);
    }

    public override void PointerExit() {
        ChangeTone(0.6f);
        ChangeChildTone(0, 0.6f);
        ChangeChildTone(1, 0.6f);
    }
}
