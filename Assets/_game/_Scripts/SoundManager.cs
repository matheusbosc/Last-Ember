using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundManager : MonoBehaviour
{
    public AudioClip[] stepSounds, pickupSounds, thunderSounds;
    public AudioSource stepAudioSource;
    [HideInInspector] public bool isWalking;
    public AudioSource musicAudioSource, uiClick;
    public AudioClip menuMusic, gameMusic;
    
    public AudioSource rainAudioSource, thunderAudioSource, fireAudioSource, pickupAudioSource, windAudioSource, woodDepositAudioSource, extinguishAudioSource;
    
    public static SoundManager instance;

    private void Start()
    {
        instance = this;
    }

    private void Update()
    {
        if (isWalking & !stepAudioSource.isPlaying)
        {
            stepAudioSource.clip = stepSounds[Random.Range(0, stepSounds.Length)];
            stepAudioSource.Play();
        }
    }

    public void ToggleMusic(bool t)
    {
        if (t) musicAudioSource.Play();
        else musicAudioSource.Stop();
    }
    
    public void ToggleWind(bool t)
    {
        if (t) windAudioSource.Play();
        else windAudioSource.Stop();
    }
    
    public void ToggleFire(bool t)
    {
        if (t) fireAudioSource.Play();
        else fireAudioSource.Stop();
    }
    
    public void ToggleRain(bool t)
    {
        if (t) rainAudioSource.Play();
        else rainAudioSource.Stop();
    }
    
    public void PlayThunder()
    {
        thunderAudioSource.clip = thunderSounds[Random.Range(0, thunderSounds.Length)];
        thunderAudioSource.Play();
    }

    public void PickupItem()
    {
        pickupAudioSource.clip = pickupSounds[Random.Range(0, pickupSounds.Length)];
        pickupAudioSource.Play();
    }
    
    public void DepositWood()
    {
        woodDepositAudioSource.Play();
    }
    
    public void Extinguish()
    {
        extinguishAudioSource.Play();
    }

    public void ChangeScene(string scene)
    {
        musicAudioSource.Stop();
        if (scene == "Main Menu")
        {
            musicAudioSource.volume = 0.8f;
            musicAudioSource.clip = menuMusic;
            rainAudioSource.Stop();
            windAudioSource.Stop();
        }
        else
        {
            musicAudioSource.volume = 0.416f;
            musicAudioSource.clip = gameMusic;
        }
        musicAudioSource.Play();
    }

    public void UIClick()
    {
        uiClick.Play();
    }

    public void PauseGame(bool p)
    {
        if (p)
        {
            musicAudioSource.Pause();
            rainAudioSource.Pause();
            thunderAudioSource.Pause();
            windAudioSource.Pause();
            fireAudioSource.Pause();
        }
        else
        {
            musicAudioSource.UnPause();
            rainAudioSource.UnPause();
            thunderAudioSource.UnPause();
            windAudioSource.UnPause();
            fireAudioSource.UnPause();
        }
    }
}
