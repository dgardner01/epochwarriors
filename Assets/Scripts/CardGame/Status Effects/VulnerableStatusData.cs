using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VulnerableStatusData.asset", menuName = "Status Effects/Vulnerable")]
public class VulnerableStatusData : StatusEffectData
{
    public int duration;
    public float damageModifier;
    public override StatusEffect CreateStatusEffect()
    {
        return new VulnerableStatus(duration, damageModifier);
    }
}
