using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardDrawStatusData.asset", menuName = "Status Effects/Card Draw")]
public class CardDrawStatusData : StatusEffectData
{
    public RageStatusData rageData;
    public override StatusEffect CreateStatusEffect()
    {
        return new CardDrawStatus(duration, magnitude, symbol, rageData);
    }
}
