using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class StatusEffect
{
    public string id;
    public string description;
    public int duration;
    public int magnitude;
    public Sprite symbol;
    public StatusEffect(int duration, Sprite symbol)
    {
        this.duration = duration;
    }

    public virtual void OnApply(Fighter fighter)
    {

    }

    public virtual void OnRemove(Fighter fighter)
    {

    }

    public virtual void OnTurnUpdate(Fighter fighter)
    {

    }

    public virtual void OnAttacked(Fighter fighter, Fighter attacker)
    {

    }
}

