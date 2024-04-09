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
    public IEnumerator ApplyRewards()
    {
        if (consecutiveHits > 2)
        {
            ApplyStatusEffect(rewards[1].CreateStatusEffect());
            yield return new WaitForSeconds(0.5f);
        }
        if (chain > 0)
        {
            ApplyStatusEffect(rewards[0].CreateStatusEffect());
            yield return new WaitForSeconds(0.5f);
        }
    }
}
