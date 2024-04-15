using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class MoveAnimation
{
    public AnimationClip clip;
    public string clipID;
    public float impactTime;
    public float knockbackForce;
}
public class FighterAnimator : MonoBehaviour
{
    public Animator animator, opponentAnimator;
    public MoveAnimation[] moves;
    public float moveSpeed;
    public float distance;
    public float minDistance;
    public float maxDistance;
    public float minPosition;
    public float maxPosition;
    public bool hurt;
    Vector3 targetPos;
    Vector3 lerpedPos;
    // Start is called before the first frame update
    void Start()
    {
        targetPos = transform.parent.localPosition;
        lerpedPos = targetPos;
    }

    // Update is called once per frame
    void Update()
    {
        lerpedPos = Vector3.Lerp(lerpedPos, targetPos, moveSpeed);
        transform.parent.localPosition = lerpedPos;
        distance = Vector3.Distance(animator.transform.parent.position, opponentAnimator.transform.parent.position);
        if (!hurt)
        {
            targetPos = opponentAnimator.transform.parent.position + Vector3.right * maxDistance;
            targetPos.y = transform.parent.localPosition.y;
        }
        if (targetPos.x < minPosition)
        {
            targetPos.x = minPosition;
        }
        if (targetPos.x > maxPosition)
        {
            targetPos.x = maxPosition;
        }
    }
    public void ApplyKnockback(float knockback)
    {
        targetPos -= Vector3.right * knockback;
    }
    public float GetImpactTimeFromClip(AnimationClip clip)
    {
        for (int i = 0; i < moves.Length; i++)
        {
            if (moves[i].clipID == clip.name)
            {
                return moves[i].impactTime;
            }
        }
        return 0;
    }
    public float GetKnockbackFromClip(AnimationClip clip)
    {
        for (int i = 0; i < moves.Length; i++)
        {
            if (moves[i].clipID == clip.name)
            {
                return moves[i].knockbackForce;
            }
        }
        return 0;
    }
    public void PlayAnimationClip(AnimationClip clip)
    {
        animator.Play(clip.name);
    }
    public void PlayAnimationClipByName(string name)
    {
        animator.Play(name);
    }
}
