using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpiritGainStatusData.asset", menuName = "Status Effects/Spirit Gain")]
public class SpiritGainStatusData : StatusEffectData
{
    public override StatusEffect CreateStatusEffect()
    {
        return new SpiritGainStatus(duration, symbol, magnitude);
    }
}
