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
        MusicManager.Instance.StopMusic("9");
        MusicManager.Instance.StopMusic("10");
        MusicManager.Instance.PlayMusic("0"); 
        if (BattleSystem.player.chain > 1)
        {
            MusicManager.Instance.PlayMusic("6");
        }
        if (BattleSystem.player.chain > 4)
        {
            MusicManager.Instance.PlayMusic("7");
        }
        if (BattleSystem.player.chain > 7)
        {
            MusicManager.Instance.PlayMusic("8");
        }
        BattleSystem.enemy.ClearIntents();
        BattleSystem.ui.PrintLog("battle begin");
        BattleSystem.player.turnDamage = BattleSystem.player.health;
        BattleSystem.enemy.turnDamage = BattleSystem.enemy.health;
        if (BattleSystem.playArea.chain <= 0)
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
                float impactTime = player.animator.GetImpactTimeFromClip(playerCard.animation); 
                player.animator.PlayAnimationClip(playerCard.animation);
                float strengthAffectedDamage = playerCard.damage + player.strength;
                if (strengthAffectedDamage > enemy.block && strengthAffectedDamage > BattleSystem.anticipationMinDamage)
                {
                    BattleSystem.OnAnticipation.Invoke();
                    Time.timeScale = 1 / (2.3f / impactTime);
                }
                yield return new WaitForSeconds(impactTime);
                Time.timeScale = 1;
            }
            if (playerCard.SFXClip != null && playerCard.damage > enemy.block)
            {
                var nameChars = playerCard.SFXClip.name.ToCharArray();
                string name = "";
                if (nameChars[1] != '_')
                {
                    name = nameChars[0].ToString() + nameChars[1].ToString();
                }
                else
                {
                    name = nameChars[0].ToString();
                }
                SFXManager.Instance.PlaySound(name);
            }
            if (playerCard.damage > 0)
            {
                int damage = playerCard.damage + player.strength;
                enemy.Damage(damage, player.animator.GetKnockbackFromClip(playerCard.animation), player);
                yield return new WaitForSeconds(waitTime);
            }
            if (playerCard.block > 0)
            {
                player.animator.PlayAnimationClipByName("guard");
                BattleSystem.OnGainBlock.Invoke();
                player.block += playerCard.block;
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
                float impactTime = enemy.animator.GetImpactTimeFromClip(enemyCard.animation);
                yield return new WaitForSeconds(impactTime);
            }
            if (enemyCard.SFXClip != null && enemyCard.damage > player.block)
            {
                var nameChars = enemyCard.SFXClip.name.ToCharArray();
                string name = "";
                if (nameChars[1] != '_')
                {
                    name = nameChars[0].ToString() + nameChars[1].ToString();
                }
                else
                {
                    name = nameChars[0].ToString();
                }
                SFXManager.Instance.PlaySound(name);
            }
            if (enemyCard.damage > 0)
            {
                int damage = enemyCard.damage + enemy.strength;
                player.Damage(damage, enemy.animator.GetKnockbackFromClip(enemyCard.animation),enemy);
                yield return new WaitForSeconds(waitTime);
            }
            if (enemyCard.block > 0)
            {
                BattleSystem.OnGainBlock.Invoke();
                enemy.block += enemyCard.block;
                yield return new WaitForSeconds(waitTime);
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