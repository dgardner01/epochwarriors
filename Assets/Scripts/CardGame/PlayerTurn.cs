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
            BattleSystem.ui.fightPanel.SetActive(false);
            BattleSystem.ui.cardGamePanel.SetActive(true);
            BattleSystem.AssignEnemyIntent();
            BattleSystem.StartCoroutine(BattleSystem.hand.DrawCard(5));
            BattleSystem.player.energy += BattleSystem.player.energyPerTurn;
            yield break;
        }
}