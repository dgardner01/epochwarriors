using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Music
{
    public string trackName;
    public AudioClip clip;
    [Range(0f, 1f)]
    public float volume = 1.0f;
    public bool loop;
    public float fadeInTime;
    public float fadeOutTime;
    [TextArea]
    public string description; // Subtitle or description for the sound
}



public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }
    public Music[] musicTracks;

    private List<AudioSource> activeSources = new List<AudioSource>();

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
    }

    public void PlayMusic(string trackName)
    {
        Music musicToPlay = musicTracks.FirstOrDefault(m => m.trackName == trackName);
        if (musicToPlay == null)
        {
            Debug.LogWarning("Music track not found: " + trackName);
            return;
        }

        StartCoroutine(ManageMusicPlayback(musicToPlay));
    }

    private IEnumerator ManageMusicPlayback(Music newTrack)
    {
        AudioSource newSource = CreateAudioSourceForTrack(newTrack);
        if (activeSources.Count > 0)
        {
            foreach (var source in activeSources.ToList())
            {
                StartCoroutine(FadeOutAndDestroy(source, newTrack.fadeOutTime));
            }
        }
        activeSources.Add(newSource);
        newSource.Play();
        yield return StartCoroutine(FadeIn(newSource, newTrack.volume, newTrack.fadeInTime));
    }

    private AudioSource CreateAudioSourceForTrack(Music track)
    {
        GameObject sourceGameObject = new GameObject($"AudioSource_{track.trackName}");
        sourceGameObject.transform.parent = transform;
        AudioSource source = sourceGameObject.AddComponent<AudioSource>();
        source.clip = track.clip;
        source.loop = track.loop;
        source.volume = 0; // Start muted to fade in
        return source;
    }

    private IEnumerator FadeIn(AudioSource source, float targetVolume, float duration)
    {
        float timer = 0;
        while (timer < duration)
        {
            source.volume = Mathf.Lerp(0, targetVolume, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }
        source.volume = targetVolume;
    }

    private IEnumerator FadeOutAndDestroy(AudioSource source, float duration)
    {
        float startVolume = source.volume;
        float timer = 0;
        while (timer < duration)
        {
            // Check if the AudioSource or GameObject has been destroyed
            if (source == null || source.gameObject == null)
                yield break; // Exit the coroutine if the source has been destroyed

            source.volume = Mathf.Lerp(startVolume, 0, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }

        if (source != null && source.gameObject != null)
        {
            source.Stop();
            activeSources.Remove(source);
            Destroy(source.gameObject);
        }
    }


    public void StopMusic()
    {
        foreach (var source in activeSources.ToList())
        {
            StartCoroutine(FadeOutAndDestroy(source, 1f));  // Assuming a generic fade out time of 1 second
        }
        activeSources.Clear();
    }
}