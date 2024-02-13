using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyCardDisplay : MonoBehaviour
{
    public EnemyPlayArea enemyPlayArea => FindAnyObjectByType<EnemyPlayArea>().GetComponent<EnemyPlayArea>();
    public Enemy.Intents intent;
    public TextMeshProUGUI enemyIntent;
    private void Update()
    {
        enemyIntent.text = intent.ToString();
    }
}
