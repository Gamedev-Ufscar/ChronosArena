using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HeroEnum
{
    None,
    Timothy,
    Harold,
    Uga,
    Guujaw,
    Muerte,
    Medusa,
    Bhesaj,
    Mordrea,
    Khalid,
    Wukong,
    Thrain,
    Diana,
    Tupa,
    Dante,
    Kyu,
    Praga,
    Matakite,
    Siphamandla,
    Ghonadur,
    Mercer,
    Sangrenta,
    Yuri,
    Eugene,
    Otto,
    Gerador,
    Elizabeth,
    Albert,
    Yeong,
    Philips,
    Jekyll,
    Uranio,
    Zarnada,
    Espectral

}

public enum CardTypes
{
    Profile,
    Attack,
    Defense,
    Charge,
    Nullification,
    Skill,
    Ultimate,
    Item,
    Passive,
    Structure
}

public enum SlotsOnBoard
{
    PlayerCard,
    PlayerCardAbove,
    PlayerSecondary,
    PlayerUltimate,
    PlayerDiscard,
    PlayerPassive,
    EnemyCard,
    EnemyCardAbove,
    EnemySecondary,
    EnemyUltimate,
    EnemyDiscard,
    EnemyPassive

}

public enum GameState
{
    Purchase,
    Choice,
    Revelation,
    Reaction,
    Effects,
    Reset
}

public enum WinCondition
{
    Victory,
    Loss,
    Draw
}