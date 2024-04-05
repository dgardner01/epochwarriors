using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class BattleVFX : MonoBehaviour
{
    public BattleSystem battleSystem => FindAnyObjectByType<BattleSystem>();
    public BattleUI ui => battleSystem.ui;
    public Volume volume;
    public Vignette vignette;
    public float targetVignetteIntensity, lerpedVignetteIntensity, vignetteSpeed;
    public ParticleSystem[] particleReferences;
    public Dictionary<string, ParticleSystem> particles = new Dictionary<string, ParticleSystem>();
    // Start is called before the first frame update
    void Start()
    {
        volume.profile.TryGet(out vignette);
        InitializeParticles();
    }
    private void Update()
    {
        bool outlineCondition = ui.DragFromHand() && battleSystem.playArea.cards.Count == 0;
        EnableParticleSystem("Outline", outlineCondition);
    }
    private void FixedUpdate()
    {
        lerpedVignetteIntensity = Mathf.Lerp(lerpedVignetteIntensity, targetVignetteIntensity, vignetteSpeed);
        vignette.intensity.value = lerpedVignetteIntensity;
    }
    void InitializeParticles()
    {
        for (int i = 0; i < particleReferences.Length; i++)
        {
            particles.Add(particleReferences[i].gameObject.name, particleReferences[i]);
        }
    }
    // Update is called once per frame
    public void EnableParticleSystem(string key, bool state)
    {
        particles[key].enableEmission = state;
    }
    public void SetVignetteIntensity(float intensity)
    {
        lerpedVignetteIntensity = intensity;
    }


}
