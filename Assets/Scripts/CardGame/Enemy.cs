using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public BattleSystem battleSystem;
    public int health;
    public int maxHealth;
    public int block;
    public int damage;
    public enum Intents
    {
        Attack,
        Block,
        Buff,
        Debuff
    }
    public void AssignIntent()
    {
        battleSystem.enemyPlayArea.intents.Add(Enemy.Intents.Attack);
    }
    public void Damage(int damage)
    {
        block -= damage;
        if (block < 0)
        {
            health += block;
            block = 0;
        }
    }
}
