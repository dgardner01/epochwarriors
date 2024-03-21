using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeStatus : StatusEffect
{
    public DodgeStatus(int duration, Sprite symbol) : base(duration, symbol)
    {
        this.duration = duration;
        this.symbol = symbol;
        id = "Dodge";
    }
    public override void OnTurnUpdate(Fighter fighter)
    {
    }
    public override void OnAttacked(Fighter fighter, Fighter enemy)
    {

    }
}
