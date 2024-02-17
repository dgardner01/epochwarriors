using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Fighter
{ 
    public BattleSystem battleSystem;
    public int damage;
    public enum Intents
    {
        Attack,
        Block,
        Buff,
        Debuff
    }
    public IEnumerator AssignIntent()
    {
        List<Intents> intents = new List<Intents> { Intents.Attack, Intents.Block, Intents.Buff, Intents.Debuff };
        int cardsPlayed = Random.Range(1, 4);
        for (int i = 0; i < cardsPlayed; i++)
        {
            Intents intent = intents[Random.Range(0, intents.Count)];
            battleSystem.enemyPlayArea.intents.Add(intent);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
