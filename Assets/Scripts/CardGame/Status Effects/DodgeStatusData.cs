using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DodgeStatusData.asset", menuName = "Status Effects/Dodge")]
public class DodgeStatusData : StatusEffectData
{
    public override StatusEffect CreateStatusEffect()
    {
        return new DodgeStatus(duration, symbol);
    }
}