using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeStatus : StatusEffect
{
    string id = "Dodge";
    public DodgeStatus(int duration, Sprite symbol) : base(duration, symbol)
    {
        this.duration = duration;
        this.symbol = symbol;
    }
    public override void OnTurnUpdate(Fighter fighter)
    {
    }
    public override void OnAttacked(Fighter fighter, Fighter enemy)
    {

    }
}
