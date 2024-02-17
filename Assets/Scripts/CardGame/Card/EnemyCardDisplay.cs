using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyCardDisplay : MonoBehaviour
{
    public EnemyPlayArea enemyPlayArea => FindAnyObjectByType<EnemyPlayArea>().GetComponent<EnemyPlayArea>();
    public Enemy enemy;
    public Enemy.Intents intent;
    public TextMeshProUGUI enemyIntent;
    private void Update()
    {
        enemyIntent.text = IntentText();
    }

    public string IntentText()
    {
        switch (intent)
        {
            case Enemy.Intents.Attack:
                return enemy.damage + " damage";
            case Enemy.Intents.Block:
                return 2 + " block";
            case Enemy.Intents.Buff:
                return "Buff";
            case Enemy.Intents.Debuff:
                return "Debuff";
        }
        return null;
    }
}
