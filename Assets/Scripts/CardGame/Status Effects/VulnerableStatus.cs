using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VulnerableStatus : StatusEffect
{
    float damageModifierAtStart;
    float damageModifier;
    public VulnerableStatus(int duration, Sprite symbol, float damageModifier) : base(duration, symbol)
    {
        this.damageModifier = damageModifier;
    }
    public override void OnApply(Fighter fighter)
    {
    }
    public override void OnRemove(Fighter fighter)
    {
        
    }
}
