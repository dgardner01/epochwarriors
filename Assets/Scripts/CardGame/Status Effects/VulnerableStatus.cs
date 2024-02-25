using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VulnerableStatus : StatusEffect
{
    float damageModifierAtStart;
    float damageModifier;
    public VulnerableStatus(int duration, float damageModifier) : base(duration)
    {
        this.damageModifier = damageModifier;
    }
    public override void OnApply(Fighter fighter)
    {
        base.OnApply(fighter);
        damageModifierAtStart = fighter.damageModifier;
        fighter.damageModifier = damageModifierAtStart + damageModifier;
    }
    public override void OnRemove(Fighter fighter)
    {
        fighter.damageModifier = damageModifierAtStart;
        base.OnRemove(fighter);
    }
}
