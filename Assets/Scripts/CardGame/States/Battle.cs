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
        BattleSystem.player.UpdateStatusEffects();
        BattleSystem.enemy.UpdateStatusEffects();
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
            if (playerCard.friendlyStatusEffect != null)
            {
                BattleSystem.player.ApplyStatusEffect(playerCard.friendlyStatusEffect.CreateStatusEffect());
            }
            if (playerCard.targetingStatusEffect != null)
            {
                BattleSystem.enemy.ApplyStatusEffect(playerCard.targetingStatusEffect.CreateStatusEffect());
            }
            yield return new WaitForSeconds(1);
        }
        if (BattleSystem.enemyCombo.intents.Count > 0)
        {
            Enemy.Intents enemyIntent = BattleSystem.enemyCombo.intents[0];
            switch (enemyIntent)
            {
                case Enemy.Intents.Attack:
                    BattleSystem.player.Damage(BattleSystem.enemy.damage);
                    break;
                case Enemy.Intents.Block:
                    BattleSystem.enemy.block += 2;
                    break;
                case Enemy.Intents.Buff:
                    BattleSystem.enemy.damage++;
                    break;
                case Enemy.Intents.Debuff:
                    BattleSystem.player.energy -= 1;
                    break;
            }
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