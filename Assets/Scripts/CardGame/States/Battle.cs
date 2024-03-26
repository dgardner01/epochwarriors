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
        BattleSystem.InitializeCombos();
        ApplyBlocks();
        yield return new WaitForSeconds(1);
        BattleSystem.StartCoroutine(ResolveCard());
    }
    public void ApplyBlocks()
    {
        List<Card> playerCardsToRemove = new List<Card>();
        foreach(Card card in BattleSystem.playerCombo.cards)
        {
            if (card.cardType == CardType.Block)
            {
                BattleSystem.player.block += card.block;
                if (card.statusEffect == null && card.damage <= 0)
                {
                    playerCardsToRemove.Add(card);
                }
            }
        }
        for (int i = 0; i < playerCardsToRemove.Count; i++)
        {
            BattleSystem.playerCombo.cards.Remove(playerCardsToRemove[0]);
            playerCardsToRemove.RemoveAt(0);
        }
        List<Card> enemyCardsToRemove = new List<Card>();
        foreach(Card card in BattleSystem.enemy.currentTurn)
        { 
            if (card.cardType == CardType.Block)
            {
                BattleSystem.enemy.block += card.block;
            }
        }
        for (int i = 0; i < enemyCardsToRemove.Count; i++)
        {
            BattleSystem.enemy.currentTurn.Remove(enemyCardsToRemove[0]);
            enemyCardsToRemove.RemoveAt(0);
        }
    }
    public IEnumerator ResolveCard()
    {
        Player player = BattleSystem.player;
        Enemy enemy = BattleSystem.enemy;
        PlayerCombo playerCombo = BattleSystem.playerCombo;
        if (BattleSystem.playerCombo.cards.Count > 0)
        {
            Card playerCard = playerCombo.cards[0];
            if (playerCard.animation != null)
            {
                player.animator.PlayAnimationClip(playerCard.animation);
            }
            if (playerCard.damage > 0)
            {
                int damage = playerCard.damage + player.strength;
                enemy.Damage(damage, player);
                yield return new WaitForSeconds(1f);
            }
            playerCombo.cards.Remove(playerCard);
            if (playerCard.statusEffect != null)
            {
                player.ApplyStatusEffect(playerCard.statusEffect.CreateStatusEffect());
                if (playerCard.statusEffect.duration < 0)
                {
                    BattleSystem.ui.StatusPopUp(playerCard.statusEffect.id + " active", BattleSystem.ui.PuppetPos(BattleSystem.player, "head", Vector3.up));
                }
                else
                {
                    BattleSystem.ui.StatusPopUp(playerCard.statusEffect.id + " up", BattleSystem.ui.PuppetPos(BattleSystem.player, "head", Vector3.up));
                }
                yield return new WaitForSeconds(1.5f);
            }
        }
        if (BattleSystem.enemy.currentTurn.Count > 0)
        {
            Card enemyCard = BattleSystem.enemy.currentTurn[0];
            if (enemyCard.animation != null)
            {
                enemy.animator.PlayAnimationClip(enemyCard.animation);
            }
            if (enemyCard.damage > 0)
            {
                int damage = enemyCard.damage + enemy.strength;
                player.Damage(damage, enemy);
                yield return new WaitForSeconds(1);
            }
            BattleSystem.enemy.currentTurn.Remove(enemyCard);
            if (enemyCard.statusEffect != null)
            {
                BattleSystem.ui.StatusPopUp(enemyCard.statusEffect.id + " active", BattleSystem.ui.PuppetPos(BattleSystem.enemy, "head", Vector3.up));
                enemy.ApplyStatusEffect(enemyCard.statusEffect.CreateStatusEffect());
                yield return new WaitForSeconds(1f);
            }
        }
        if (playerCombo.cards.Count == 0 && BattleSystem.enemy.currentTurn.Count == 0)
        {
            BattleSystem.SetState(new PlayerTurn(BattleSystem));
        }
        else
        {
            BattleSystem.StartCoroutine(ResolveCard());
        }

    }
}