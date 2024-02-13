using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle : State
{
    public Battle(BattleSystem battleSystem) : base(battleSystem)
    {
    }
    public override IEnumerator Start()
    {
        BattleSystem.ui.PrintLog("battle begin");
        BattleSystem.ui.cardGamePanel.SetActive(false);
        BattleSystem.ui.fightPanel.SetActive(true);
        BattleSystem.InitializeCombos();
        yield return new WaitForSeconds(1);
        BattleSystem.StartCoroutine(ResolveCard());
    }
    public IEnumerator ResolveCard()
    {
        if (BattleSystem.playerCombo.cards.Count > 0)
        {
            Card playerCard = BattleSystem.playerCombo.cards[0];
            BattleSystem.enemy.Damage(playerCard.damage);
            BattleSystem.player.block += playerCard.block;
            BattleSystem.playerCombo.cards.Remove(playerCard);
            yield return new WaitForSeconds(1);
        }
        if (BattleSystem.enemyCombo.intents.Count > 0)
        {
            Enemy.Intents enemyIntent = BattleSystem.enemyCombo.intents[0];
            BattleSystem.player.Damage(BattleSystem.enemy.damage); 
            BattleSystem.enemyCombo.intents.Remove(enemyIntent);
            yield return new WaitForSeconds(1);
        }
        if (BattleSystem.playerCombo.cards.Count == 0 && BattleSystem.enemyCombo.intents.Count == 0)
        {
            BattleSystem.SetState(new PlayerTurn(BattleSystem));
        }
        else
        {
            BattleSystem.StartCoroutine(ResolveCard());
        }

    }
}