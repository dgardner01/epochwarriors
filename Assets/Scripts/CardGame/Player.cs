using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int health;
    public int maxHealth;
    public int block;
    public int energy;
    public int energyPerTurn;
    public List<Card> drawPile;
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
