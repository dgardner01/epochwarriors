using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RageStatus : StatusEffect
{
    int maxMagnitude = 3;
    public RageStatus(int duration, Sprite symbol) : base(duration, symbol)
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
    public override void OnTurnUpdate(Fighter fighter)
    {
        magnitude++;
        fighter.strength++;
        if (magnitude > maxMagnitude)
        {
            fighter.strength -= magnitude;
            duration = -1;
        }
    }
}
