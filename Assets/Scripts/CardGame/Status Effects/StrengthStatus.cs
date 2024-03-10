using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrengthStatus : StatusEffect
{
    string id = "Strength";
    public StrengthStatus(int duration, Sprite symbol) : base(duration, symbol)
    {
        this.duration = duration;
        this.symbol = symbol;
    }
    public override void OnApply(Fighter fighter)
    {
        fighter.strength = duration;
    }
    public override void OnTurnUpdate(Fighter fighter)
    {
        fighter.strength = duration;
    }
}
