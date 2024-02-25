using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCardTrigger : TooltipTrigger
{
    public EnemyCardDisplay display => GetComponent<EnemyCardDisplay>();
    private void Update()
    {
        header = "Enemy Card";
        switch (display.intent)
        {
            case Enemy.Intents.Attack:
                content = "Bruttia is going to attack for " + display.enemy.damage + " damage.";
                break;
            case Enemy.Intents.Block:
                content = "Bruttia is going to gain 2 block.";
                break;
            case Enemy.Intents.Buff:
                content = "Bruttia is going to gain 1 damage.";
                break;
            case Enemy.Intents.Debuff:
                content = "Bruttia is going to make you lose 1 energy next turn.";
                break;
        }

    }
}
