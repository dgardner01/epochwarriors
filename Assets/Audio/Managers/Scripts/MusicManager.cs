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

    private Dictionary<string, AudioSource> namedAudioSources = new Dictionary<string, AudioSource>(); // named!

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

        if (!namedAudioSources.TryGetValue(trackName, out AudioSource existingSource))
        {
            AudioSource newSource = CreateAudioSourceForTrack(musicToPlay);
            newSource.Play();  // Ensure the source is played
            StartCoroutine(FadeIn(newSource, musicToPlay.volume, musicToPlay.fadeInTime));  // Ensure the fade-in starts
            namedAudioSources[trackName] = newSource;
            activeSources.Add(newSource);
        }
        else
        {
            Debug.Log("Track already playing: " + trackName);
        }
    }



    public void PlayMusicOver(string baseTrackName, string layerTrackName)
    {
        Music baseTrack = musicTracks.FirstOrDefault(m => m.trackName == baseTrackName);
        Music layerTrack = musicTracks.FirstOrDefault(m => m.trackName == layerTrackName);

        if (baseTrack == null || layerTrack == null)
        {
            Debug.LogWarning($"Base track or layer track not found: {baseTrackName}, {layerTrackName}");
            return;
        }

        if (!namedAudioSources.TryGetValue(baseTrackName, out AudioSource baseSource))
        {
            Debug.LogWarning($"Base track not currently playing: {baseTrackName}");
            return;
        }

        AudioSource layerSource = CreateAudioSourceForTrack(layerTrack);
        if (layerSource == null)
        {
            Debug.LogError("Failed to create audio source for layer track.");
            return;
        }

        layerSource.time = baseSource.time;  // Synchronize starting time with base track
        layerSource.Play();
        StartCoroutine(FadeIn(layerSource, layerTrack.volume, layerTrack.fadeInTime));  // Start the fade-in process
        namedAudioSources[layerTrackName] = layerSource;  // Store reference to this new overlay source
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
        source.volume = track.volume; // Ensure the volume is set based on the track's setting
        return source;
    }



    private IEnumerator FadeIn(AudioSource source, float targetVolume, float duration)
    {
        if (source == null)
        {
            yield break;  // Exit if the source is null
        }

        float startVolume = 0f;  // Always start from 0 for a fade-in
        float timer = 0f;
        while (timer < duration)
        {
            if (source == null)
            {
                yield break;  // Check if the source is still valid
            }

            timer += Time.deltaTime;
            source.volume = Mathf.Lerp(startVolume, targetVolume, timer / duration);  // Linear interpolation from startVolume to targetVolume
            yield return null;
        }

        source.volume = targetVolume;  // Ensure the volume is exactly the target volume at the end
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


    public void StopMusic(string trackName)
    {
        if (namedAudioSources.TryGetValue(trackName, out AudioSource source))
        {
            StartCoroutine(FadeOutAndDestroy(source, 1f));
            namedAudioSources.Remove(trackName);
            activeSources.Remove(source);
        }
        else
        {
            Debug.LogWarning($"Attempted to stop a non-existing track: {trackName}");
        }
    }


}