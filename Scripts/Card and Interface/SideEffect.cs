using UnityEngine;

public abstract class SideEffect
{
    // Phases: 0 Choice, 1 Revelation, 2 EffectsBefore, 3 EffectsAfter
    private int value;

    public SideEffect(int value)
    {
        this.value = value;
    }

    public int GetValue()
    {
        return value;
    }

    public void ReduceValue()
    {
        value--;
        Debug.Log("Reduced Value: " + value);
    }

    public void SetValue(int value)
    {
        this.value = value;
    }
}

public interface Effecter
{
    SEPhase phase { get; set; }

    void Effect(Player user, Player enemy);
}

public class Vodka : SideEffect, Effecter
{
    public SEPhase phase { get; set; }

    public Vodka(int value, SEPhase phase) : base(value)
    {
        this.phase = phase;
    }

    public void Effect(Player user, Player enemy)
    {
        if (user.GetHero() == HeroEnum.Yuri)
        {
            if (user.GetCardPlayed().GetIsNullified())
            {
                user.DealDamage(1, true);
                Debug.Log("Vodka");
            }
        }
    }
}

public class WeakSpot : SideEffect, Effecter
{
    public SEPhase phase { get; set; }
    public Damage affectedCard { get; set; }

    public WeakSpot(int value, SEPhase phase) : base(value)
    {
        this.phase = phase;
    }

    public void Effect(Player user, Player enemy)
    {
        if (GetValue() >= 2)
        {
            if (user.GetCardPlayed() is Damage)
            {
                Damage damage = (Damage)user.GetCardPlayed();
                affectedCard = damage;

                damage.SetIsUnblockable(true);
            }
        }
        else if (GetValue() == 1)
        {
            if (affectedCard != null)
            {
                affectedCard.SetIsUnblockable(false);
            }
        }
        ReduceValue();
    }
}

public class DejaVuSE : SideEffect, Effecter
{
    public SEPhase phase { get; set; }

    public DejaVuSE(int value, SEPhase phase) : base(value)
    {
        this.phase = phase;
    }

    public void Effect(Player user, Player enemy)
    {
        if (GetValue() >= 2)
        {
            enemy.SetPredicted(true);
        } else if (GetValue() == 1)
        {
            enemy.SetPredicted(false);
        }
        ReduceValue();
    }
}

public class Chronos : SideEffect
{

    public Chronos(int variable) : base(variable)
    {
    }

    
}
