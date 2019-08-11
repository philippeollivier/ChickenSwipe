using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource MusicSource;
    public AudioSource MusicSource2;
    private int musicVolume;
    private int sfxVolume;

    void Start()
    {
        musicVolume = PlayerPrefs.GetInt("MVolume", 2);
        sfxVolume = PlayerPrefs.GetInt("SFXVolume", 7);

        MusicSource.volume = (float)musicVolume * 0.1f;
        MusicSource2.volume = (float)sfxVolume * 0.1f;

        MusicSource.Play();
        MusicSource.loop = true;
    }

    public void OnCoinCollect()
    {
        if (MusicSource2.isPlaying)
        {
            MusicSource2.pitch += 0.080901f;
        }
        else
        {
            MusicSource2.pitch = 1.0f; 
        }
        MusicSource2.Play();
    }

    public void musicVolumeUp()
    {
        musicVolume++;
        musicVolume = Mathf.Clamp(musicVolume, 0, 10);
        MusicSource.volume = (float)musicVolume * 0.1f;
        PlayerPrefs.SetInt("MVolume", musicVolume);
    }

    public void musicVolumeDown()
    {
        musicVolume--;
        musicVolume = Mathf.Clamp(musicVolume, 0, 10);
        MusicSource.volume = (float)musicVolume * 0.1f;
        PlayerPrefs.SetInt("MVolume", musicVolume);
    }

    public void SFXVolumeUP()
    {
        sfxVolume++;
        sfxVolume = Mathf.Clamp(sfxVolume, 0, 10);
        MusicSource2.volume = (float)sfxVolume * 0.1f;
        PlayerPrefs.SetInt("SFXVolume", sfxVolume);
    }

    public void SFXVolumeDown()
    {
        sfxVolume--;
        sfxVolume = Mathf.Clamp(sfxVolume, 0, 10);
        MusicSource2.volume = (float)sfxVolume * 0.1f;
        PlayerPrefs.SetInt("SFXVolume", sfxVolume);
    }

}
