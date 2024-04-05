using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    public BattleSystem battleSystem => FindAnyObjectByType<BattleSystem>();
    BattleUI ui => battleSystem.ui;
    public string name;
    public int health;
    public int turnDamage;
    public int maxHealth;
    public int strength;
    public int block;
    public int cardsDrawnPerTurn;
    public int consecutiveHits;
    public int consecutiveDamage;
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
            if (status.id == "Reflect")
            {
                parryStatus = status;
            }
        }
        if (dodgeStatus != null)
        {
            animator.PlayAnimationClipByName("dash_backward");
            activeStatusEffects.Remove(dodgeStatus);
            ui.TextPopUp("Dodged!",ui.PuppetPos(this, "head", Vector2.up), ui.blockPopUp);
            return;
        }
        if (parryStatus != null)
        {
            StartCoroutine(DelayedDamage(damage/2, opponent));
            activeStatusEffects.Remove(parryStatus);
        }
        block -= damage;
        if (block < 0)
        {
            opponent.consecutiveHits++;
            opponent.consecutiveDamage += damage;
            health += block;
            battleSystem.ui.TextPopUp("" + Mathf.Abs(block), ui.PuppetPos(this, "head", Vector2.up / 2), ui.numberPopUp);
            cam.ScreenShake(time, magnitude * Mathf.Abs(block), decreaseFactor);
            consecutiveHits = 0;
            consecutiveDamage = 0;
            block = 0;
        }
        else
        {
            animator.PlayAnimationClipByName("guard");
            battleSystem.ui.TextPopUp("Blocked!",ui.PuppetPos(this, "head", Vector2.up), ui.blockPopUp);
            cam.ScreenShake(time, magnitude/2, decreaseFactor);
        }
    }
    public IEnumerator DelayedDamage(int damage, Fighter opponent)
    {
        animator.PlayAnimationClipByName("bite");
        yield return new WaitForSeconds(.5f);
        opponent.Damage(damage, this); 
    }
    public void ApplyStatusEffect(StatusEffect statusEffect)
    {
        statusEffect.OnApply(this);
        if (statusEffect.duration < 0)
        {
            battleSystem.ui.TextPopUp(statusEffect.id + " active", battleSystem.ui.PuppetPos(this, "head", Vector3.up), ui.statusPopUp);
        }
        else
        {
            battleSystem.ui.TextPopUp(statusEffect.id + " up", battleSystem.ui.PuppetPos(this, "head", Vector3.up), ui.statusPopUp);
        }
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
                    status.magnitude += statusEffect.magnitude;
                    status.duration += statusEffect.duration;
                    return;
                }
            }
        }
        activeStatusEffects.Add(statusEffect);
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
