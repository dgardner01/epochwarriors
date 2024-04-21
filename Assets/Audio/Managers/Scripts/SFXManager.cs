using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip[] clips; // Array to hold multiple clips for round-robin selection
    [Range(0f, 1f)]
    public float volume = 1.0f;
    [Range(0.1f, 3f)]
    public float minPitch = 1.0f; // Default minimum pitch range set to 1
    [Range(0.1f, 3f)]
    public float maxPitch = 1.0f; // Default maximum pitch range set to 1
    [TextArea]
    public string description; // Description for the sound
    public bool loopIndefinitely = false;
}

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance { get; private set; }

    public Sound[] sounds;
    private Dictionary<string, Sound> soundDictionary = new Dictionary<string, Sound>();
    private Dictionary<string, AudioSource> loopingSounds = new Dictionary<string, AudioSource>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        foreach (Sound sound in sounds)
        {
            soundDictionary[sound.name] = sound;
        }
    }

    public void PlaySound(string soundName)
    {
        if (soundDictionary.TryGetValue(soundName, out Sound sound))
        {
            AudioSource audioSource = CreateAudioSource(sound);
            if (audioSource == null)
            {
                return; // AudioSource creation failed, exit the function
            }

            audioSource.Play();

            if (sound.loopIndefinitely)
            {
                if (!loopingSounds.ContainsKey(soundName))
                {
                    loopingSounds[soundName] = audioSource;
                    StartCoroutine(AdjustPitchAtLoop(audioSource, sound));
                }
            }
            else
            {
                Destroy(audioSource.gameObject, audioSource.clip.length);
            }
        }
        else
        {
            Debug.LogWarning("Sound name not found in dictionary: " + soundName);
        }
    }

    private IEnumerator AdjustPitchAtLoop(AudioSource source, Sound sound)
    {
        while (source.isPlaying)
        {
            yield return new WaitWhile(() => source.isPlaying && source.time < source.clip.length - 0.01f);
            source.pitch = Random.Range(sound.minPitch, sound.maxPitch);
        }
    }

    public void StopLoopingSound(string soundName)
    {
        if (loopingSounds.TryGetValue(soundName, out AudioSource audioSource))
        {
            StopCoroutine("AdjustPitchAtLoop");  // Stop the coroutine if it's running
            audioSource.Stop();
            Destroy(audioSource.gameObject);
            loopingSounds.Remove(soundName);
        }
    }

    private AudioSource CreateAudioSource(Sound sound)
    {
        if (sound.clips == null || sound.clips.Length == 0)
        {
            Debug.LogError($"No clips assigned for the sound '{sound.name}'.");
            return null; // Early return to prevent further execution and potential crash
        }

        // Safely access an AudioClip using Random.Range
        AudioClip clipToPlay = sound.clips[Random.Range(0, sound.clips.Length)];
        GameObject soundGameObject = new GameObject("SFX_" + sound.name);
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.clip = clipToPlay;
        audioSource.volume = sound.volume;
        audioSource.pitch = Random.Range(sound.minPitch, sound.maxPitch); // Randomize pitch
        audioSource.loop = sound.loopIndefinitely; // Set looping based on the sound settings

        return audioSource;
    }

}
