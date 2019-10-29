using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class Constants
{
    public const int maxHandSize = 10;
    public const int maxCardAmount = 14;
    public const int maxUltiAreaSize = 3;
    public const int maxSideListSize = 12;
    public const int SpeedOfLight = 300000;
    public const float cardRiseHeight = 100f;
    public const float cardBigSize = 2.9f;
}

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
    Interface,
    Reaction,
    Effects,
    Reset
}

public enum SEPhase
{
    Choice,
    Reaction,
    EffectsBefore,
    EffectsAfter
}

public enum CountEnum
{
    CardCount,
    UltiCount,
    PassiveCount
}

public enum WinCondition
{
    Victory,
    Loss,
    Draw
}

public enum ManualButtonType
{
    Link,
    Left,
    Right,
    Back
}

public enum SceneList
{
    Menu,
    Lobby,
    Selection,
    Game,
    Tutorial
}

public enum ManualPage
{
    Indice,
    Introducao,
    Ataque,
    Defesa,
    Carga,
    Skill,
    Anulacao,
    Ultimate,
    Item
}

public enum TutorialBox
{
    Introducao,
    AtaquePrimeiro,
    DanoELimite,
    JogarAtaquePrimeiro,
    ParabensAtaque,
    BotaoCentro,
    Limite,
    LimiteExplicacao,
    JogarAtaqueSegundo,
    LimiteContexto,
    EscolhaDuas,
    Defesa,
    Carga,
    TenteOutra,
    InimigoAtacou,
    Skill,
    SkillDownfall,
    Anulacao,
    JogueAnulacao,
    ParabensAnulacao,
    PercebaPassiva,
    Passiva,
    Resumo,
    Shuffle,
    Ultimate,
    CargaNovamente,
    CompraUltimate,
    MuitoBem
}