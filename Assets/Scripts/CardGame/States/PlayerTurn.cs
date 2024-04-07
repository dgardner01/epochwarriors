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
        BattleSystem.player.UpdateStatusEffects();
        BattleSystem.enemy.UpdateStatusEffects();
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
        }
        BattleSystem.AssignEnemyIntent();
        BattleSystem.StartCoroutine(BattleSystem.hand.DrawCard(BattleSystem.player.cardsDrawnPerTurn));
        BattleSystem.player.spirit += BattleSystem.player.spiritPerTurn;
        BattleSystem.player.block = 0;
        BattleSystem.enemy.block = 0;
        yield break;
        }
}