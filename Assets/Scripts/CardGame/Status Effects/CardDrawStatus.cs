using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDrawStatus : StatusEffect
{
    int cardsDrawn;
    StatusEffectData rageData;
    public CardDrawStatus(int duration, Sprite symbol, int cardsDrawn, StatusEffectData rageData) : base(duration, symbol)
    {
        this.duration = duration;
        this.symbol = symbol;
        this.cardsDrawn = cardsDrawn;
        this.rageData = rageData;
        id = "CardDraw";
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
