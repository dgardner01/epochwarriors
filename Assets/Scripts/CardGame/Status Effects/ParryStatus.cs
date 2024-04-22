using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryStatus : StatusEffect
{
    public ParryStatus(int duration, Sprite symbol, string id) : base(duration, symbol)
    {
        this.duration = duration;
        this.symbol = symbol;
        this.id = id;
        description = "Block half of the next attack's damage, then deal that damage to the opponent.";
    }
}
