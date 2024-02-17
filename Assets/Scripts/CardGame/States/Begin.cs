using System.Collections;
using UnityEngine;

public class Begin : State
    {
        public Begin(BattleSystem battleSystem) : base(battleSystem)
        {

        }
        public override IEnumerator Start()
        {
            BattleSystem.ui.PrintLog("match begin");
            yield return new WaitForSeconds(2);
            BattleSystem.SetState(new PlayerTurn(BattleSystem));
        }
    }