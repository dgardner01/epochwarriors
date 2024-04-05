using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterAnimator : MonoBehaviour
{
    public Animator animator;
    public AnimationClip[] clips;
    public float[] impactTime;
    public Dictionary<string, float> impactTimes = new Dictionary<string, float>();
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < clips.Length; i++)
        {
            impactTimes.Add(clips[i].name, impactTime[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public float GetImpactTimeFromClipName(string name)
    {
        return impactTimes[name];
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
