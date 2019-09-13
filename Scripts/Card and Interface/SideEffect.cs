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

    public void TickTimer() {
        this.timer--;
    }

    public void SetTimer(int timer) {
        this.timer = timer;
    }

    public abstract void Effect(Player user, Player enemy);
}

public abstract class SideEffectVariable : SideEffect
{
    public int variable;

    public SideEffectVariable(int variable, int phase) : base(phase)
    {
        this.variable = variable;
    }

    public void SetVariable(int variable)
    {
        this.variable = variable;
    }

    public int GetVariable()
    {
        return variable;
    }

}

public class Vodka : SideEffectTimed
{
    public Vodka(int timer, int phase) : base(timer, phase)
    {
    }

    public override void Effect(Player user, Player enemy)
    {
        if (player.GetHero() == HeroEnum.Yuri)
        {
            if (player.GetCardPlayed().GetIsNullified())
                player.DealDamage(1, true);
        }
    }
}

public class WeakSpot : SideEffectTimed
{
    public WeakSpot(int timer, int phase) : base(timer, phase)
    {
    }

    public override void Effect(Player user, Player enemy)
    {
        if (player.GetCardPlayed() is Damage) {
            Damage damage = (Damage)player.GetCardPlayed();

            damage.SetIsUnblockable(true);
        }
    }
}

public class DejaVuSE : SideEffectTimed
{

    public DejaVuSE(int timer, int phase) : base(timer, phase)
    {
    }

    public override void Effect(Player user, Player enemy)
    {
        if (timer >= 2)
        {
            user.SetPredicted(true);
        } else 
        {
            user.SetPredicted(false);
        }
        TickTimer();
    }
}

public class Chronos : SideEffectVariable
{

    public Chronos(int variable, int phase) : base(variable, phase)
    {
    }

    
}
