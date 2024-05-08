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
    public int highCombo;
    public int consecutiveDamage;
    public int chain;
    public int highChain;
    public int charge;
    public int highCharge;
    public List<StatusEffect> activeStatusEffects = new List<StatusEffect>();
    public FighterAnimator animator;
    public void Damage(int damage, float knockback, Fighter opponent)
    {
        CameraManager cam = Camera.main.GetComponent<CameraManager>();
        float time = 0.1f;
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
            animator.PlayAnimationClipByName("afterimage");
            activeStatusEffects.Remove(dodgeStatus);
            ui.TextPopUp("Dodged!",ui.PuppetPos(this, "head", Vector2.up), ui.blockPopUp);
            return;
        }
        block -= damage;
        if (block < 0)
        {
            if (health + block <= 0)
            {
                for (int i = 0; i < 22; i++)
                {
                    MusicManager.Instance.StopMusic(i.ToString());
                }
                if (name == "Bruttia")
                {
                    SFXManager.Instance.PlaySound("42");
                }
                else
                {
                    SFXManager.Instance.PlaySound("43");
                }
                animator.PlayAnimationClipByName("defeat");
                StartCoroutine(cam.ScreenShake(2 * (block/10), cam.magnitude * Mathf.Min(15, Mathf.Abs(block)) * lostComboModifier, cam.frequency));
                if (opponent == battleSystem.player)
                {
                    battleSystem.SetState(new Win(battleSystem));
                }
                else
                {
                    battleSystem.SetState(new Lose(battleSystem));
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
                    //add next fight phase track layer loop when hit
                    int trackLayer = MusicManager.Instance.gameObject.transform.childCount;
                    print(trackLayer);
                    if (trackLayer < 6)
                    {
                        MusicManager.Instance.PlayMusicOver("0", trackLayer.ToString());
                    }
                    //play hit stinger
                    if (name == "Bruttia")
                    {
                        SFXManager.Instance.PlaySound("40");
                    }
                    else
                    {
                        SFXManager.Instance.PlaySound("41");
                    }
                    SFXManager.Instance.PlaySound("32");
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
                if (opponent.consecutiveHits > opponent.highCombo)
                {
                    opponent.highCombo = opponent.consecutiveHits;
                    PlayerPrefs.SetInt("combo", opponent.highCombo);
                }
                opponent.consecutiveDamage += damage;
                float magModifier = damage;
                float freqModifier = 1;
                health += block;
                battleSystem.ui.TextPopUp("" + Mathf.Abs(block), ui.PuppetPos(this, "head", Vector2.up / 2), ui.numberPopUp);
                StartCoroutine(cam.ScreenShake((block/10) * lostComboModifier, cam.magnitude * Mathf.Min(15, Mathf.Abs(block)) * lostComboModifier, cam.frequency));
                battleSystem.vfx.StartBackgroundCharBounce(bounceMag * magModifier, 0);
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
            StartCoroutine(cam.ScreenShake(time, (cam.magnitude/2), cam.frequency));
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
            SFXManager.Instance.PlaySound("17");
        }
        opponent.Damage(damage, -20, this); 
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
