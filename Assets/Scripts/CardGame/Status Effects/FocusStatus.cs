using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusStatus : StatusEffect
{
    public FocusStatus(int duration, Sprite symbol) : base(duration, symbol)
    {
        this.duration = duration;
        this.symbol = symbol;
        id = "Rage";
    }
    public override void OnApply(Fighter fighter)
    {
        magnitude++;
        fighter.strength++;
    }
}
