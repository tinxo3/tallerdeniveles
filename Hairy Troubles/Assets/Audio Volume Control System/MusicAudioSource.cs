using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicAudioSource : MonoBehaviour
{
    AudioSource audioSource;
    AudioSettings audioSettings;

    void Start()
    {
        audioSettings = AudioSettings.audioSettings;
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = audioSettings.GetMusicVolume();
        audioSettings.AddMeToMusicAudioSources(audioSource);
    }

    void OnDestroy()
    {
        audioSettings.RemoveMeFromMusicAudioSources(audioSource);
    }
}
