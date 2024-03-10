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
        List<StatusEffect> statusToRemove = new List<StatusEffect>();
            foreach(StatusEffect status in BattleSystem.player.activeStatusEffects)
        {
            status.OnTurnUpdate(BattleSystem.player);
            if (status.duration < 0)
            {
                statusToRemove.Add(status);
            }
        }
        int statusCount = statusToRemove.Count;
            for (int i = 0; i < statusCount; i++)
        {
            BattleSystem.player.RemoveStatusEffect(statusToRemove[0]);
            statusToRemove.RemoveAt(0);
        }
        BattleSystem.ui.middleThird.SetActive(true);
        BattleSystem.ui.lowerThird.SetActive(true);
        BattleSystem.AssignEnemyIntent();
            BattleSystem.StartCoroutine(BattleSystem.hand.DrawCard(BattleSystem.player.cardsDrawnPerTurn));
            BattleSystem.player.spirit += BattleSystem.player.spiritPerTurn;
            yield break;
        }
}