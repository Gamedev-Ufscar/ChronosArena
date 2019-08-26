using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageStash : MonoBehaviour
{
    public static ImageStash IS;
    public Sprite[] UgaList;
    public Sprite[] TimothyList;
    public Sprite[] HaroldList;
    public Sprite[] YuriList;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake() {
        ImageStash.IS = this;
    }

    private void OnEnable() {
        if (ImageStash.IS == null) {
            ImageStash.IS = this;
        } else {
            if (ImageStash.IS != this) {
                Destroy(ImageStash.IS);
                ImageStash.IS = this;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
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
