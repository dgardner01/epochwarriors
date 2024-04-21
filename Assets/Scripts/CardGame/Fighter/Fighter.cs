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
    public int chain;
    public int charge;
    public List<StatusEffect> activeStatusEffects = new List<StatusEffect>();
    public FighterAnimator animator;
    public void Damage(int damage, float knockback, Fighter opponent)
    {
        CameraManager cam = Camera.main.GetComponent<CameraManager>();
        float time = 0.1f;
        float magnitude = 0.1f;
        float decreaseFactor = 2;
        float bounceMag = 0.05f;
        float bounceFreq = 10;
        float lostComboModifier = consecutiveHits > 2 ? 2 : 1;
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
        block -= damage;
        if (block < 0)
        {
            if (health + block <= 0)
            {
                animator.PlayAnimationClipByName("defeat");
                cam.ScreenShake(.5f, magnitude * Mathf.Min(15, Mathf.Abs(block)) * lostComboModifier, decreaseFactor);
                battleSystem.SetState(new Win(battleSystem));
            }
            else
            {
                if (parryStatus != null)
                {
                    Parry(damage, opponent, parryStatus);
                }
                else
                {
                    animator.hurt = true;
                    StartCoroutine(ResetAnimatorHurt(knockback));
                    animator.ApplyKnockback(knockback);
                    animator.PlayAnimationClipByName("hurt" + Random.Range(1, 3));
                }
                opponent.consecutiveHits++;
                if (opponent.consecutiveHits > 2)
                {
                    opponent.charge++;
                }
                opponent.consecutiveDamage += damage;
                float magModifier = damage;
                float freqModifier = 1;
                health += block;
                battleSystem.ui.TextPopUp("" + Mathf.Abs(block), ui.PuppetPos(this, "head", Vector2.up / 2), ui.numberPopUp);
                cam.ScreenShake(time * lostComboModifier, magnitude * Mathf.Min(15, Mathf.Abs(block)) * lostComboModifier, decreaseFactor);
                battleSystem.vfx.StartBackgroundCharBounce(bounceMag * magModifier, bounceFreq * freqModifier);
                charge = 0;
                consecutiveHits = 0;
                consecutiveDamage = 0;
                block = 0;
            }
        }
        else
        {
            if (parryStatus != null)
            {
                Parry(damage, opponent, parryStatus);
            }
            else
            {
                animator.PlayAnimationClipByName("guard");
                animator.PlayAnimationClipByName("block");
            }
            battleSystem.OnAttackBlocked.Invoke();
            battleSystem.vfx.StartBackgroundCharBounce(bounceMag, bounceFreq);
            battleSystem.ui.TextPopUp("Blocked!",ui.PuppetPos(this, "head", Vector2.up), ui.blockPopUp);
            cam.ScreenShake(time, (magnitude/2), decreaseFactor);
        }
    }
    public void Parry(int damage, Fighter opponent, StatusEffect parryStatus)
    {
        StartCoroutine(DelayedDamage(damage / 2, opponent));
        activeStatusEffects.Remove(parryStatus);
    }
    public IEnumerator ResetAnimatorHurt(float knockback)
    {
        yield return new WaitForSeconds(knockback * animator.moveSpeed);
        animator.hurt = false;
    }
    public IEnumerator DelayedDamage(int damage, Fighter opponent)
    {
        animator.PlayAnimationClipByName("bite");
        yield return new WaitForSeconds(0.3f);
        if (damage > opponent.block)
        {
            SFXManager.Instance.PlaySound("14");
        }
        opponent.Damage(damage, -3, this); 
    }
    public void ApplyStatusEffect(StatusEffect statusEffect)
    {
        battleSystem.OnStatusEffectUp.Invoke();
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
        battleSystem.OnStatusEffectDown.Invoke();
        statusEffect.OnRemove(this);
        activeStatusEffects.Remove(statusEffect);
    }
    
    public IEnumerator UpdateStatusEffects()
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
            yield return new WaitForSeconds(0.5f);
            RemoveStatusEffect(statusToRemove);
        }
    }
}
