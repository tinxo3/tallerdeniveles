using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    public static AudioSettings audioSettings;

    [Header("Information - Read Only from inspector")]
    [SerializeField]
    private float musicVolume;
    [SerializeField]
    private float sfxVolume;

    float musicDefaultVolume=0.7f;
    float sfxDefaultVolume = 0.9f;

    int musicDefaultMute = 0;
    int sfxDefaultMute = 0;

    string musicAudioSourcesTag ="Music-AudioSource";
    string sfxAudioSourcesTag="SFX-AudioSource";

    string musicVolumeDataName = "music-volume";
    string sfxVolumeDataName = "sfx-volume";

    string musicMuteDataName = "music-mute";
    string sfxMuteDataName = "sfx-mute";

    List<AudioSource> musicAudioSources;
    List<AudioSource> sfxAudioSources;

    [SerializeField]
    private int musicAudioSourcesCount=0;
    [SerializeField]
    private int sfxAudioSourcesCount = 0;
    [SerializeField]
    private int musicMute = 1; //actually bool
    [SerializeField]
    private int sfxMute = 1; //actually bool

    private void Awake()
    {
        audioSettings = this;
        musicAudioSources = new List<AudioSource>();
        sfxAudioSources = new List<AudioSource>();
        LoadSavedSettings();
    }

    void LoadSavedSettings()
    {
        musicMute = PlayerPrefs.GetInt(musicMuteDataName, musicDefaultMute);
        
        //if(musicMute == 1)
        //{
        //    Debug.Log("true "+ musicMute.ToString());
        //    musicVolume = 0;
        //
        //}
        //else
            musicVolume = PlayerPrefs.GetFloat(musicVolumeDataName,musicDefaultVolume);

        //sfxMute = PlayerPrefs.GetInt(sfxMuteDataName, sfxDefaultMute);
        //if (sfxMute == 1)
        //{
        //    sfxVolume = 0;
        //}
        //else
            sfxVolume = PlayerPrefs.GetFloat(sfxVolumeDataName, sfxDefaultVolume);
    }

    public void ChangeMusicVolume(float newVolume)
    {
        musicVolume = newVolume;
        PlayerPrefs.SetFloat(musicVolumeDataName, musicVolume);
        SetVolumeToAudioSources(musicAudioSources, musicVolume);
    }


    public void ChangSFXVolume(float newVolume)
    {
        sfxVolume = newVolume;
        PlayerPrefs.SetFloat(sfxVolumeDataName, sfxVolume);
        SetVolumeToAudioSources(sfxAudioSources, sfxVolume);
    }
    public void ChangeMuteMusic(Toggle tg)
    {
        if (tg.isOn)
        {
            foreach (AudioSource a in musicAudioSources)
            {
                a.mute = true;
            }
            musicMute = 0;
            PlayerPrefs.SetInt(musicMuteDataName, musicMute);
        }
        else
        {
            foreach (AudioSource a in musicAudioSources)
            {
                a.mute = false;
            }
            PlayerPrefs.SetInt(musicMuteDataName, musicMute);
        }
    }
    public void ChangeMuteSFX(Toggle tg)
    {
        if (tg.isOn == true)
        {
            foreach (AudioSource a in sfxAudioSources)
            {
                a.mute = true;
            }
            sfxMute = 0;
            PlayerPrefs.SetInt(sfxMuteDataName, sfxMute);
        }
        else
        {
            foreach (AudioSource a in sfxAudioSources)
            {
                a.mute = false;
            }
            sfxMute = 1;
            PlayerPrefs.SetInt(sfxMuteDataName, sfxMute);
        }
    }
    void SetVolumeToAudioSources(List<AudioSource> audioSources, float volume)
    {
        foreach (AudioSource a in audioSources)
        {
            a.mute = false;
            a.volume = volume;
        }
    }


    public float GetMusicVolume()
    {
        return musicVolume;
    }
    public float GetSFXVolume()
    {
        return sfxVolume;
    }

    public void AddMeToMusicAudioSources(AudioSource a)
    {
        musicAudioSources.Add(a);
        musicAudioSourcesCount = musicAudioSources.Count;
    }

    public void RemoveMeFromMusicAudioSources(AudioSource a)
    {
        musicAudioSources.Remove(a);
        musicAudioSourcesCount = musicAudioSources.Count;
    }
    public void AddMeToSFXAudioSources(AudioSource a)
    {
        sfxAudioSources.Add(a);
        sfxAudioSourcesCount = sfxAudioSources.Count;
    }

    public void RemoveMeFromSFXAudioSources(AudioSource a)
    {
        sfxAudioSources.Remove(a);
        sfxAudioSourcesCount = sfxAudioSources.Count;
    }


}
