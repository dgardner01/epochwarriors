using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurn : State
    {
        public PlayerTurn(BattleSystem battleSystem) : base(battleSystem)
        {
        }
        public override IEnumerator Start()
        {
            BattleSystem.ui.PrintLog("player turn begin");
        BattleSystem.enemy.UpdateTurnIndex();
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
            BattleSystem.ui.StatusPopUp(status.id + " wears off", BattleSystem.ui.PuppetPos(BattleSystem.player, "head", Vector2.up));
            statusToRemove.RemoveAt(0);
        }
        BattleSystem.ui.middleThird.SetActive(true);
        BattleSystem.ui.lowerThird.SetActive(true);
        BattleSystem.AssignEnemyIntent();
        BattleSystem.StartCoroutine(BattleSystem.hand.DrawCard(BattleSystem.player.cardsDrawnPerTurn));
        BattleSystem.player.spirit += BattleSystem.player.spiritPerTurn;
        BattleSystem.player.block = 0;
        BattleSystem.enemy.block = 0;
        yield break;
        }
}