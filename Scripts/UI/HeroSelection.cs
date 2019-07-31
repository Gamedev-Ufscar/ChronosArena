using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroSelection : MonoBehaviour
{
    public Image myPortrait;
    public Text myTitle;
    public Image enemyPortrait;
    public Text enemyTitle;
    public Sprite noHero;
    public Image enemyConfirmButton;
    public Sprite confirm;
    public Sprite confirmed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameOverseer.GO.enemyConfirm)
        {
            enemyConfirmButton.sprite = confirmed;
        } else
        {
            enemyConfirmButton.sprite = confirm;
        }
    }
}
