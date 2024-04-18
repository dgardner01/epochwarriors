using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle : State
{
    public Battle(BattleSystem battleSystem) : base(battleSystem)
    {
    }
    BattleUI ui => BattleSystem.ui;
    public override IEnumerator Start()
    {
        BattleSystem.enemy.ClearIntents();
        BattleSystem.ui.PrintLog("battle begin");
        BattleSystem.player.turnDamage = BattleSystem.player.health;
        BattleSystem.enemy.turnDamage = BattleSystem.enemy.health;
        if (BattleSystem.playArea.chain)
        {
            BattleSystem.ui.PrintLog("chain bonus!");
            BattleSystem.player.chain++;
        }
        else
        {
            BattleSystem.player.chain = 0;
        }
        BattleSystem.StartCoroutine(BattleSystem.InitializeCombos());
        yield return new WaitForSeconds(1);
        BattleSystem.ui.lowerThird.GetComponent<Animator>().Play("down");
        yield return new WaitForSeconds(.25f);
        BattleSystem.ui.InitializeComboDisplay();
        BattleSystem.StartCoroutine(ResolveCard());
    }
    public IEnumerator ResolveCard()
    {
        Player player = BattleSystem.player;
        Enemy enemy = BattleSystem.enemy;
        PlayerCombo playerCombo = BattleSystem.playerCombo;
        float waitTime = 1;
        if (BattleSystem.playerCombo.cards.Count > 0 && BattleSystem.player.health > 0)
        {
            BattleSystem.OnCardPlay.Invoke();
            ui.PlayComboCard(BattleSystem.playerCombo.transform, BattleSystem.playerCombo.cards);
            Card playerCard = playerCombo.cards[0];
            yield return new WaitForSeconds(waitTime);
            if (playerCard.animation != null)
            {
                player.animator.PlayAnimationClip(playerCard.animation);
                yield return new WaitForSeconds(player.animator.GetImpactTimeFromClip(playerCard.animation));
            }
            if (playerCard.block > 0)
            {
                player.block += playerCard.block;
            }
            if (playerCard.damage > 0)
            {
                int damage = playerCard.damage + player.strength;
                enemy.Damage(damage, player.animator.GetKnockbackFromClip(playerCard.animation), player);
                yield return new WaitForSeconds(waitTime);
            }
            playerCombo.cards.Remove(playerCard);
            if (playerCard.statusEffect != null)
            {
                player.ApplyStatusEffect(playerCard.statusEffect.CreateStatusEffect());
                yield return new WaitForSeconds(waitTime);
            }
        }
        if (BattleSystem.enemy.currentTurn.Count > 0 && BattleSystem.enemy.health > 0)
        {
            BattleSystem.OnCardPlay.Invoke();
            ui.PlayComboCard(BattleSystem.enemyCombo.transform, BattleSystem.enemy.currentTurn);
            yield return new WaitForSeconds(waitTime);
            Card enemyCard = BattleSystem.enemy.currentTurn[0];
            if (enemyCard.animation != null)
            {
                enemy.animator.PlayAnimationClip(enemyCard.animation);
            }
            if (enemyCard.damage > 0)
            {
                int damage = enemyCard.damage + enemy.strength;
                player.Damage(damage, enemy.animator.GetKnockbackFromClip(enemyCard.animation),enemy);
                yield return new WaitForSeconds(waitTime);
            }
            if (enemyCard.block > 0)
            {
                enemy.block += enemyCard.block;
            }
            BattleSystem.enemy.currentTurn.Remove(enemyCard);
            if (enemyCard.statusEffect != null)
            {
                enemy.ApplyStatusEffect(enemyCard.statusEffect.CreateStatusEffect());
                yield return new WaitForSeconds(waitTime);
            }
        }
        if (player.health <= 0 || enemy.health <= 0)
        {

        }
        else if (playerCombo.cards.Count == 0 && BattleSystem.enemy.currentTurn.Count == 0)
        {
            BattleSystem.SetState(new PlayerTurn(BattleSystem));
        }
        else
        {
            BattleSystem.StartCoroutine(ResolveCard());
        }

    }
}