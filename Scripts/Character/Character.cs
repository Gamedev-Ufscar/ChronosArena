using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Character {
    [SerializeField]
    private HeroEnum hero = HeroEnum.None;
    [SerializeField]
    private Sprite profile;
    [SerializeField]
    private int sideListSize = 0;
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
                return "Timothy";

            case HeroEnum.Harold:
                return "Dr. Harold";

            case HeroEnum.Uga:
                return "Uga";

            case HeroEnum.Yuri:
                return "Yuri";

            case HeroEnum.Zarnada:
                return "Zarnada";

            case HeroEnum.Tupa:
                return "Tupã";

            case HeroEnum.Gerador:
                return "Mecha-Gerador";

            case HeroEnum.Eugene:
                return "Eugene";

            default:
                return "Roboto";
        }
    }

    // Getters
    public HeroEnum GetHero()
    {
        return hero;
    }

    public Sprite GetProfile()
    {
        return profile;
    }

    public int GetSideListSize()
    {
        return sideListSize;
    }

    public int GetCardCount()
    {
        return cardCount;
    }

    public int GetUltiCount()
    {
        return ultiCount;
    }

    public int GetPassiveCount()
    {
        return passiveCount;
    }

    public List<CardTypes> GetAttackDisableList()
    {
        return attackDisableList;
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
