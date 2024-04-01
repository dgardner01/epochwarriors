using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardDrawStatusData.asset", menuName = "Status Effects/Card Draw")]
public class CardDrawStatusData : StatusEffectData
{
    public int cardsDrawn;
    public StatusEffectData rageData;
    public override StatusEffect CreateStatusEffect()
    {
        return new CardDrawStatus(duration, symbol, cardsDrawn, rageData);
    }
}
