using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Fighter
{
    public int spirit;
    public int spiritPerTurn;
    public List<Card> drawPile;
    public List<StatusEffectData> rewards;
    private void Update()
    {
    }
}
