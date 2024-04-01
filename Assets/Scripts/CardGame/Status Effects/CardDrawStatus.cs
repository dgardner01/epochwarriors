using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDrawStatus : StatusEffect
{
    string id = "CardDraw";
    int cardsDrawn;
    int cardsDrawnAtStart;
    public CardDrawStatus(int duration, Sprite symbol, int cardsDrawn) : base(duration, symbol)
    {
        this.duration = duration;
        this.symbol = symbol;
        this.cardsDrawn = cardsDrawn;
    }
    public override void OnApply(Fighter fighter)
    {
        base.OnApply(fighter);
        cardsDrawnAtStart = fighter.cardsDrawnPerTurn;
        fighter.cardsDrawnPerTurn = cardsDrawnAtStart + cardsDrawn;
    }
    public override void OnRemove(Fighter fighter)
    {
        fighter.cardsDrawnPerTurn = cardsDrawnAtStart;
        base.OnRemove(fighter);
    }
}
