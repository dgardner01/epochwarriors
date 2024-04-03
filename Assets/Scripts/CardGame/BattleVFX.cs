using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class BattleVFX : MonoBehaviour
{
    public Volume volume;
    public Vignette vignette;
    public float targetVignetteIntensity, lerpedVignetteIntensity, vignetteSpeed;
    // Start is called before the first frame update
    void Start()
    {
        volume.profile.TryGet(out vignette);
    }
    private void FixedUpdate()
    {
        lerpedVignetteIntensity = Mathf.Lerp(lerpedVignetteIntensity, targetVignetteIntensity, vignetteSpeed);
        vignette.intensity.value = lerpedVignetteIntensity;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetVignetteIntensity(float intensity)
    {
        lerpedVignetteIntensity = intensity;
    }
}
