using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritGainStatus : StatusEffect
{
    int spiritGained;
    public SpiritGainStatus(int duration, Sprite symbol, int spiritGained) : base(duration, symbol)
    {
        this.spiritGained = spiritGained;
        this.symbol = symbol;
        id = "Spirit gain";
        magnitude = 1;
        description = "You gained x additional spirit this turn. Spirit gain is reset at the start of your turn.";
    }
    public override void OnApply(Fighter fighter)
    {
        base.OnApply(fighter);
        fighter.gameObject.GetComponent<Player>().spirit += spiritGained;
    }
}
