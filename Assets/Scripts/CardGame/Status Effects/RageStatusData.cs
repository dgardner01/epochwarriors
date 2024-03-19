using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RageStatusData.asset", menuName = "Status Effects/Rage")]
public class RageStatusData : StatusEffectData
{
    public override StatusEffect CreateStatusEffect()
    {
        return new RageStatus(duration, symbol);
    }
}
