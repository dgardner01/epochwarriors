using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ParryStatusData.asset", menuName = "Status Effects/Parry")]
public class ParryStatusData : StatusEffectData
{
    public override StatusEffect CreateStatusEffect()
    {
        return new ParryStatus(duration, symbol, id);
    }
}
