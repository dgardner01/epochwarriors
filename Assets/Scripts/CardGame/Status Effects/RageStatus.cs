using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RageStatus : StatusEffect
{
    float vignetteIntensity = 0.4f;
    public RageStatus(int duration, Sprite symbol) : base(duration, symbol)
    {
        this.duration = duration;
        this.symbol = symbol;
        id = "Rage";
    }
    public override void OnApply(Fighter fighter)
    {
        fighter.strength += duration;
        magnitude += duration;
        fighter.battleSystem.vfx.SetVignetteIntensity(vignetteIntensity);
    }
    public override void OnTurnUpdate(Fighter fighter)
    {

    }
}
