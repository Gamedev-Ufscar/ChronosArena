using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Character : MonoBehaviour
{
    [SerializeField]
    private HeroEnum hero = HeroEnum.None;
    [SerializeField]
    private Sprite profile;
    [SerializeField]
    private int cardCount = 0;
    [SerializeField]
    private int ultiCount = 0;
    [SerializeField]
    private int passiveCount = 0;
    [SerializeField]
    private List<CardTypes> attackDisableList = new List<CardTypes>();

    // HeroEnum -> String
    public string GetHeroString()
    {
        switch (hero)
        {
            case HeroEnum.Timothy:
                return "TIMOTHY";

            case HeroEnum.Harold:
                return "DR. HAROLD";

            case HeroEnum.Uga:
                return "UGA";

            case HeroEnum.Yuri:
                return "YURI";

            case HeroEnum.Zarnada:
                return "ZARNADA";

            case HeroEnum.Tupa:
                return "TUPÃ";

            case HeroEnum.Gerador:
                return "MECHA-GERADOR";

            case HeroEnum.Eugene:
                return "EUGENE";

            default:
                return "ROBOTO";
        }
    }

    // Getters
    public Sprite GetProfile()
    {
        return profile;
    }

    public int GetCount(CountEnum count)
    {
        switch(count)
        {
            case CountEnum.CardCount:
                return cardCount;

            case CountEnum.UltiCount:
                return ultiCount;

            case CountEnum.PassiveCount:
                return passiveCount;

            default:
                return cardCount;
        }
    }

}
