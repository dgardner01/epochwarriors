using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDrawStatus : StatusEffect
{
    int cardsDrawn;
    StatusEffectData rageData;
    public CardDrawStatus(int duration, int cardsDrawn, Sprite symbol, StatusEffectData rageData) : base(duration, symbol)
    {
        this.duration = duration;
        this.symbol = symbol;
        this.cardsDrawn = cardsDrawn;
        this.rageData = rageData;
        id = "Card draw";
    }
    public override void OnApply(Fighter fighter)
    {
        fighter.battleSystem.hand.StartCoroutine(fighter.battleSystem.hand.DrawCard(cardsDrawn));
        fighter.battleSystem.enemy.ApplyStatusEffect(rageData.CreateStatusEffect());
    }
    public override void OnRemove(Fighter fighter)
    {
        base.OnRemove(fighter);
    }
}
