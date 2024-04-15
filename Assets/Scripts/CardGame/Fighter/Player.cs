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
            SFXManager.Instance.PlaySound("comboActive");
            ApplyStatusEffect(rewards[1].CreateStatusEffect());
            yield return new WaitForSeconds(0.5f);
        }
        if (chain > 0)
        {
            SFXManager.Instance.PlaySound("chainActive");
            ApplyStatusEffect(rewards[0].CreateStatusEffect());
            yield return new WaitForSeconds(0.5f);
        }
    }
}
