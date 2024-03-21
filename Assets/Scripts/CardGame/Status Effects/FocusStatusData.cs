using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FocusStatusData.asset", menuName = "Status Effects/Focus")]
public class FocusStatusData : StatusEffectData
{
    public override StatusEffect CreateStatusEffect()
    {
        return new FocusStatus(duration, symbol);
    }
}