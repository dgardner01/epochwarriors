using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EnemyCardDisplay : MonoBehaviour
{
    public EnemyPlayArea enemyPlayArea => FindAnyObjectByType<EnemyPlayArea>().GetComponent<EnemyPlayArea>();
    public Enemy enemy;
    public Enemy.Intents intent;
    public TextMeshProUGUI enemyIntent;
    public Sprite[] sprites;
    public Image image;
    private void Update()
    {
        switch (intent)
        {
            case Enemy.Intents.Attack:
                image.sprite = sprites[0];
                enemyIntent.text = "" + enemy.damage;
                break;
            case Enemy.Intents.Block:
                image.sprite = sprites[1];
                enemyIntent.text = "2";
                break;
            case Enemy.Intents.Buff:
                image.sprite = sprites[2];
                enemyIntent.text = "1";
                break;
            case Enemy.Intents.Debuff:
                image.sprite = sprites[2];
                enemyIntent.text = "1";
                break;
        }
    }
}
