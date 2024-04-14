using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Lose : State
{
    public Lose(BattleSystem battleSystem) : base(battleSystem)
    {

    }
    public override IEnumerator Start()
    {
        BattleSystem.StartCoroutine(BattleSystem.ui.ClearCombo(BattleSystem.playerCombo.transform, BattleSystem.discard.transform));
        BattleSystem.StartCoroutine(BattleSystem.ui.ClearCombo(BattleSystem.enemyCombo.transform, BattleSystem.ui.enemyPlayedZone.transform));
        yield return new WaitForSeconds(0.1f);
        BattleSystem.player.health = 0;
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("PlayerLose");
    }
}