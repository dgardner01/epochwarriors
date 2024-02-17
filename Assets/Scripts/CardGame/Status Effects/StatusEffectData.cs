using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffectData : ScriptableObject
{
    public abstract StatusEffect CreateStatusEffect();
}
