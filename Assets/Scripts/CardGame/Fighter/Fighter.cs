using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    BattleSystem battleSystem => FindAnyObjectByType<BattleSystem>();
    BattleUI ui => battleSystem.ui;
    public string name;
    public int health;
    public int maxHealth;
    public int strength;
    public int block;
    public int cardsDrawnPerTurn;
    public List<StatusEffect> activeStatusEffects = new List<StatusEffect>();
    public FighterAnimator animator;
    public void Damage(int damage, Fighter opponent)
    {
        CameraManager cam = Camera.main.GetComponent<CameraManager>();
        float time = 0.1f;
        float magnitude = 0.1f;
        float decreaseFactor = 2;
        StatusEffect dodgeStatus = null;
        StatusEffect parryStatus = null;
        foreach (StatusEffect status in activeStatusEffects)
        {
            if (status.id == "Dodge")
            {
                dodgeStatus = status;
            }
            if (status.id == "Parry")
            {
                parryStatus = status;
            }
        }
        if (dodgeStatus != null)
        {
            activeStatusEffects.Remove(dodgeStatus);
            battleSystem.ui.TextPopUp("Dodged!",ui.PuppetPos(this, "head", Vector2.up));
            return;
        }
        if (parryStatus != null)
        {
            damage /= 2;
            opponent.Damage(damage, this);
            activeStatusEffects.Remove(parryStatus);
        }
        block -= damage;
        if (block < 0)
        {
            health += block;
            battleSystem.ui.NumberPopUp("" + Mathf.Abs(block), ui.PuppetPos(this, "head", Vector2.up / 2));
            cam.ScreenShake(time, magnitude * Mathf.Abs(block), decreaseFactor);
            block = 0;
        }
        else
        {
            battleSystem.ui.TextPopUp("Blocked!",ui.PuppetPos(this, "head", Vector2.up));
            cam.ScreenShake(time, magnitude/2, decreaseFactor);
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
                    return;
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
            status.OnTurnUpdate(this);
            status.DecreaseDuration();
            if (status.duration <= -1)
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
