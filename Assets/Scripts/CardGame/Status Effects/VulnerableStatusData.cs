using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VulnerableStatusData.asset", menuName = "Status Effects/Vulnerable")]
public class VulnerableStatusData : StatusEffectData
{
    public float damageModifier;
    public override StatusEffect CreateStatusEffect()
    {
        return new VulnerableStatus(duration, symbol, damageModifier);
    }
}
