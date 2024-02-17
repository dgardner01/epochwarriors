using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffect
{
    public string id;
    public int duration;

    public StatusEffect(int duration)
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
    public void DecreaseDuration()
    {
        duration--;
    }
}

