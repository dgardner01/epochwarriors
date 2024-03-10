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
        BattleSystem.ui.lowerThird.SetActive(false);
        BattleSystem.ui.middleThird.SetActive(false);
        BattleSystem.player.UpdateStatusEffects();
        BattleSystem.enemy.UpdateStatusEffects();
        BattleSystem.InitializeCombos();
        yield return new WaitForSeconds(1);
        BattleSystem.StartCoroutine(ResolveCard());
    }
    public IEnumerator ResolveCard()
    {
        Player player = BattleSystem.player;
        Enemy enemy = BattleSystem.enemy;
        PlayerCombo playerCombo = BattleSystem.playerCombo;
        if (BattleSystem.playerCombo.cards.Count > 0)
        {
            Card playerCard = playerCombo.cards[0];
            enemy.Damage(playerCard.damage+player.strength);
            player.block += playerCard.block;
            playerCombo.cards.Remove(playerCard);
            if (playerCard.statusEffect != null)
            {
                player.ApplyStatusEffect(playerCard.statusEffect.CreateStatusEffect());
            }
            if (playerCard.animation != null)
            {
                player.animator.PlayAnimationClip(playerCard.animation);
            }
            yield return new WaitForSeconds(1);
        }
        if (playerCombo.cards.Count == 0)
        {
            BattleSystem.SetState(new PlayerTurn(BattleSystem));
        }
        else
        {
            BattleSystem.StartCoroutine(ResolveCard());
        }

    }
}