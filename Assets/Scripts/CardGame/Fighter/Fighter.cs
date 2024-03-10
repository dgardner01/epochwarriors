using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    public string name;
    public int health;
    public int maxHealth;
    public int strength;
    public int block;
    public int cardsDrawnPerTurn;
    public List<StatusEffect> activeStatusEffects = new List<StatusEffect>();
    public FighterAnimator animator;
    public void Damage(int damage)
    {
        block -= damage;
        if (block < 0)
        {
            health += block;
            block = 0;
        }
    }

    public void ApplyStatusEffect(StatusEffect statusEffect)
    {
        if (activeStatusEffects == null)
        {
            activeStatusEffects = new List<StatusEffect>();
        }
        else
        {
            foreach(StatusEffect status in activeStatusEffects)
            {
                if (statusEffect.id == status.id)
                {
                    status.duration += statusEffect.duration;
                    break;
                }
            }
        }
        activeStatusEffects.Add(statusEffect);
        statusEffect.OnApply(this);
    }

    public void RemoveStatusEffect(StatusEffect statusEffect)
    {
        statusEffect.OnRemove(this);
        activeStatusEffects.Remove(statusEffect);
    }
    
    public void UpdateStatusEffects()
    {
        if (activeStatusEffects == null)
        {
            activeStatusEffects = new List<StatusEffect>();
        }
        List<StatusEffect> effectsToRemove = new List<StatusEffect>();
        foreach (StatusEffect status in activeStatusEffects)
        {
            status.DecreaseDuration();
            if (status.duration <= 0)
            {
                effectsToRemove.Add(status);
            }
        }
        foreach(StatusEffect statusToRemove in effectsToRemove)
        {
            RemoveStatusEffect(statusToRemove);
        }
    }
}
