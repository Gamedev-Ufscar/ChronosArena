using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageStash : MonoBehaviour
{
    public static ImageStash IS;
    [SerializeField]
    private Sprite[] timothyList;
    [SerializeField]
    private Sprite[] haroldList;
    [SerializeField]
    private Sprite[] ugaList;
    [SerializeField]
    private Sprite[] yuriList;
    Hashtable heroHashtable;

    // Start is called before the first frame update
    void Start()
    {
        SetupHash();
    }

    void Awake () {
        DontDestroyOnLoad(this);
        if (IS == null) {
            IS = this;
            SetupHash();
        } else {
            if (IS != this) {
                Destroy(this);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetupHash()
    {
        // Hero HT
        heroHashtable = new Hashtable();
        heroHashtable.Add(HeroEnum.Timothy, timothyList);
        heroHashtable.Add(HeroEnum.Harold, haroldList);
        heroHashtable.Add(HeroEnum.Uga, ugaList);
        heroHashtable.Add(HeroEnum.Yuri, yuriList);
    }

    public Sprite GetImage(HeroEnum hero, int id)
    {
        
        //Sprite[] list;
        Sprite spr = ((Sprite[])heroHashtable[hero])[id];
        return spr;

        /*switch (hero)
        {
            case HeroEnum.Timothy:
                list = timothyList;
                Debug.Log("Timothy");
                break;

            case HeroEnum.Harold:
                list = haroldList;
                Debug.Log("Harold");
                break;

            case HeroEnum.Uga:
                list = ugaList;
                Debug.Log("Uga");
                break;

            case HeroEnum.Yuri:
                list = yuriList;
                Debug.Log("Yuri");
                break;

            default:
                list = timothyList;
                break;
        }*/
    }

    public Texture2D textureFromSprite(Sprite sprite)
    {
        if (sprite.rect.width != sprite.texture.width)
        {
            Texture2D newText = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
            Color[] newColors = sprite.texture.GetPixels((int)sprite.textureRect.x,
                                                         (int)sprite.textureRect.y,
                                                         (int)sprite.textureRect.width,
                                                         (int)sprite.textureRect.height);
            newText.SetPixels(newColors);
            newText.Apply();
            return newText;
        }
        else
            return sprite.texture;
    }
}
