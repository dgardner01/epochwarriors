using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurn : State
    {
        public PlayerTurn(BattleSystem battleSystem) : base(battleSystem)
        {
        }
    BattleUI ui => BattleSystem.ui;
        public override IEnumerator Start()
        {
            BattleSystem.ui.PrintLog("player turn begin");
        BattleSystem.player.turnDamage = BattleSystem.player.health;
        BattleSystem.enemy.turnDamage = BattleSystem.enemy.health;
        BattleSystem.ui.lowerThird.GetComponent<Animator>().Play("up");
        BattleSystem.StartCoroutine(BattleSystem.ui.ClearCombo(BattleSystem.playerCombo.transform, BattleSystem.discard.transform));
        yield return new WaitForSeconds(0.5f);
        BattleSystem.StartCoroutine(BattleSystem.ui.ClearCombo(BattleSystem.enemyCombo.transform, BattleSystem.ui.enemyPlayedZone.transform));
        yield return new WaitForSeconds(0.5f);
        BattleSystem.enemy.UpdateTurnIndex();
        BattleSystem.enemy.StartCoroutine(BattleSystem.enemy.DisplayIntents());
        BattleSystem.StartCoroutine(BattleSystem.player.UpdateStatusEffects());
        BattleSystem.StartCoroutine(BattleSystem.enemy.UpdateStatusEffects());
        if (BattleSystem.enemy.activeStatusEffects.Count > 0)
        {
            StatusEffect status = BattleSystem.enemy.activeStatusEffects[0];
        }
        List<StatusEffect> statusToRemove = new List<StatusEffect>();
            foreach(StatusEffect status in BattleSystem.player.activeStatusEffects)
        {
            status.OnTurnUpdate(BattleSystem.player);
            if (status.duration < 1)
            {
                statusToRemove.Add(status);
            }
        }
        int statusCount = statusToRemove.Count;
        for (int i = 0; i < statusCount; i++)
        {
            StatusEffect status = statusToRemove[0];
            BattleSystem.player.RemoveStatusEffect(status);
            BattleSystem.ui.TextPopUp(status.id + " wears off", BattleSystem.ui.PuppetPos(BattleSystem.player, "head", Vector2.up), ui.statusPopUp);
            statusToRemove.RemoveAt(0);
            yield return new WaitForSeconds(1);
        }
        if (BattleSystem.player.charge > 0)
        {
            if (BattleSystem.player.charge > BattleSystem.player.highCharge)
            {
                BattleSystem.player.highCharge = BattleSystem.player.charge;
                PlayerPrefs.SetInt("charge", BattleSystem.player.charge);
            }
            if (BattleSystem.player.charge > 1)
            {
                BattleSystem.OnChargeExtended.Invoke();
            }
            else
            {
                BattleSystem.OnChargeBegin.Invoke();
            }
            BattleSystem.ui.TextPopUp("Charge bonus!", BattleSystem.ui.PuppetPos(BattleSystem.player, "head", Vector2.up), ui.chargePopUp);
            yield return new WaitForSeconds(1);
            for (int i = 0; i < BattleSystem.player.charge; i++)
            {
                BattleSystem.player.ApplyStatusEffect(BattleSystem.player.rewards[1].CreateStatusEffect());
            }
            yield return new WaitForSeconds(1);
        }
        if (BattleSystem.player.chain > 1)
        {
            if (BattleSystem.player.chain > BattleSystem.player.highChain)
            {
                BattleSystem.player.highChain = BattleSystem.player.chain;
                PlayerPrefs.SetInt("chain", BattleSystem.player.highChain);
            }
            if (BattleSystem.player.chain > 3)
            {
                BattleSystem.OnChainExtended.Invoke();
            }
            else
            {
                BattleSystem.OnChainBegin.Invoke();
            }
            BattleSystem.ui.TextPopUp("Chain bonus!", BattleSystem.ui.PuppetPos(BattleSystem.player, "head", Vector2.up), ui.chainPopUp);
            yield return new WaitForSeconds(1);
            for (int i = 0; i < 1+Mathf.FloorToInt(BattleSystem.player.chain/5); i++)
            {
                BattleSystem.player.ApplyStatusEffect(BattleSystem.player.rewards[0].CreateStatusEffect());
            }
            yield return new WaitForSeconds(1);
        }
        BattleSystem.AssignEnemyIntent();
        BattleSystem.StartCoroutine(BattleSystem.hand.DrawCard(BattleSystem.player.cardsDrawnPerTurn));
        BattleSystem.player.spirit += BattleSystem.player.spiritPerTurn;
        BattleSystem.player.block = 0;
        BattleSystem.enemy.block = 0;
        yield break;
        }
}