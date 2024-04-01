using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritGainStatus : StatusEffect
{
    string id = "SpiritGain";
    int spiritGained;
    public SpiritGainStatus(int duration, Sprite symbol, int spiritGained) : base(duration, symbol)
    {
        this.spiritGained = spiritGained;
        this.symbol = symbol;
    }
    public override void OnApply(Fighter fighter)
    {
        base.OnApply(fighter);
        fighter.gameObject.GetComponent<Player>().spirit += spiritGained;
    }
}
