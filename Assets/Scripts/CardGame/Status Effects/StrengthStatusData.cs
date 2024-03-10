using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StrengthStatusData.asset", menuName = "Status Effects/Strength")]
public class StrengthStatusData : StatusEffectData
{
    public override StatusEffect CreateStatusEffect()
    {
        return new StrengthStatus(duration, symbol);
    }
}