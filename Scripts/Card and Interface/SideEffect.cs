using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SideEffect : MonoBehaviour
{
    // Phases: 0 Choice, 1 Revelation, 2 EffectsBefore, 3 EffectsAfter
    [SerializeField]
    protected Player player;
    private int phase; 

    public SideEffect(int phase)
    {
        this.phase = phase;
    }

    public int GetPhase()
    {
        return phase;
    }
}

public abstract class SideEffectTimed : SideEffect
{
    public int timer;

    public SideEffectTimed(int timer, int phase) : base(phase)
    {
        this.timer = timer;
    }

    public int GetTimer()
    {
        return timer;
    }

    public abstract void effect(Player user, Player enemy, int priority);
}

public abstract class SideEffectVariable : SideEffect
{
    public int variable;

    public SideEffectVariable(int phase) : base(phase)
    {
    }

}

public class Vodka : SideEffectTimed
{
    public Vodka(int timer, int phase) : base(timer, phase)
    {
    }

    public override void effect(Player user, Player enemy, int priority)
    {
        if (player.GetHero() == HeroEnum.Yuri)
        {
            if (player.GetCardPlayed().GetIsNullified())
                player.DealDamage(1, true);
        }
    }
}
