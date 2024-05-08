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
    public GameObject backgroundCharacterParent;
    bool bellHasSparkled;
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
        float gaugeEmissionRate = (float)battleSystem.player.spirit / battleSystem.player.spiritPerTurn;
        SetParticleSystemEmissionRate("Gauge Sparkle", Mathf.Lerp(0, 10, gaugeEmissionRate));
        bool bellSparkleCondition = battleSystem.player.spirit <= 0 && battleSystem.playArea.cards.Count > 0;
        if (bellSparkleCondition && !bellHasSparkled)
        {
            MusicManager.Instance.PlayMusicOver("9", "19");
            bellHasSparkled = true;
        }
        if (bellHasSparkled && !bellSparkleCondition)
        {
            MusicManager.Instance.StopMusic("19");
            bellHasSparkled = false;
        }
        EnableParticleSystem("Bell Sparkle", bellSparkleCondition);
        float playerComboTrackerEmissionRate = battleSystem.player.consecutiveHits > 2 ? (float)battleSystem.player.consecutiveHits / 10 : 0;
        SetParticleSystemEmissionRate("Nelly Combo Flames", Mathf.Lerp(0,250,playerComboTrackerEmissionRate));
        float enemyComboTrackerEmissionRate = battleSystem.enemy.consecutiveHits > 2 ? (float)battleSystem.enemy.consecutiveHits / 10 : 0;
        SetParticleSystemEmissionRate("Bruttia Combo Flames", Mathf.Lerp(0, 250, enemyComboTrackerEmissionRate));
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
    public void SetParticleSystemEmissionRate(string key, float emissionRate)
    {
        particles[key].emissionRate = emissionRate;
    }
    public void SetVignetteIntensity(float intensity)
    {
        lerpedVignetteIntensity = intensity;
    }

    public void StartBackgroundCharBounce(float magnitude, float frequency)
    {
        for (int i = 0; i < backgroundCharacterParent.transform.childCount; i++)
        {
            GameObject characterObject = backgroundCharacterParent.transform.GetChild(i).gameObject;
            BackgroundCharAnimation animator = characterObject.GetComponent<BackgroundCharAnimation>();
            StartCoroutine(animator.StartBounce(magnitude, frequency));
        }
    }

    public void BackgroundCharEndBounce()
    {
        for (int i = 0; i < backgroundCharacterParent.transform.childCount; i++)
        {
            GameObject characterObject = backgroundCharacterParent.transform.GetChild(i).gameObject;
            BackgroundCharAnimation animator = characterObject.GetComponent<BackgroundCharAnimation>();
            StartCoroutine(animator.StartBounce(1, 20));
            animator.win = true;
        }
    }

}
