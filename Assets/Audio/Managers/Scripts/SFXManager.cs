using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    [Range(0f, 1f)]
    public float volume = 1.0f;
    [TextArea]
    public string description; // Subtitle or description for the sound
}

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance { get; private set; }

    public Sound[] sounds;
    private Dictionary<string, Sound> soundDictionary = new Dictionary<string, Sound>();

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
            GameObject soundGameObject = new GameObject("Player: " + soundName);
            AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
            audioSource.clip = sound.clip;
            audioSource.volume = sound.volume; // Use the volume from the Sound object
            audioSource.Play();

            Destroy(soundGameObject, sound.clip.length);
        }
        else
        {
            Debug.LogWarning("Sound name not found in dictionary: " + soundName);
        }
    }
}

