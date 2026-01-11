using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EventSystem : MonoBehaviour
{
    public Event currentEvent = Event.Cooldown;
    public float eventsDuration, eventCooldown;
    public Fire fire;

    public float rainMultiplier = 2, stormMultiplier = 3, windMultiplier = 2.5f;

    private bool changingEvents = false;

    public ParticleSystem lightRain, HeavyRain, Wind;
    public ParticleSystem[] groundRainDrops;

    bool waitingForEvent = false, waitingForCooldown = false, gameStarted = false;

    bool isPaused = false;

    float currentTime = 0;
    
    public Image rainOverlay;
    private float rainOverlayAlpha = 0, rainOverlaySetTime, startingRainOverlayAlpha;
    
    Event lastEvent = Event.Cooldown;

    void Start()
    {
        StartCoroutine(StartGame());
    }

    private void Update()
    {
        isPaused = GameManager.instance.isPaused;

        if (isPaused)
        {
            lightRain.Pause();
            HeavyRain.Pause();
            Wind.Pause();
            foreach (var d in groundRainDrops)
            {
                d.Pause();
            }
        }
        else
        {
            if (currentEvent == Event.Rain) lightRain.Play();
            if (currentEvent == Event.Storm) HeavyRain.Play();
            if (currentEvent == Event.Wind) Wind.Play();

            if (currentEvent == Event.Rain || currentEvent == Event.Storm)
            {
                foreach (var d in groundRainDrops)
                {
                    d.Play();
                }
            }
        }
        
        if (isPaused || !gameStarted) return;
        if (changingEvents)
        {
            currentTime = 0;
            startingRainOverlayAlpha = rainOverlay.color.a;
            switch (currentEvent)
            {
                case Event.Normal:
                    lightRain.Stop();
                    HeavyRain.Stop();
                    Wind.Stop();
                    fire.multiplier = 1;
                    rainOverlayAlpha = 0;
                    foreach (var p in groundRainDrops)
                    {
                        p.Stop();
                    }
                    ResourceManager.instance.spawningBark = false;
                    SoundManager.instance.ToggleRain(false);
                    SoundManager.instance.ToggleWind(false);
                    rainOverlaySetTime = 0;
                    break;
                case Event.Rain:
                    lightRain.Play();
                    HeavyRain.Stop();
                    Wind.Stop();
                    fire.multiplier = rainMultiplier;
                    rainOverlayAlpha = 154f / 255f;
                    foreach (var p in groundRainDrops)
                    {
                        p.Play();
                    }
                    ResourceManager.instance.spawningBark = false;
                    SoundManager.instance.ToggleRain(true);
                    SoundManager.instance.rainAudioSource.volume = 0.7f;
                    SoundManager.instance.ToggleWind(false);
                    rainOverlaySetTime = 0;
                    break;
                case Event.Storm:
                    lightRain.Stop();
                    HeavyRain.Play();
                    Wind.Stop();
                    fire.multiplier = stormMultiplier;
                    rainOverlayAlpha = 154f / 255f;
                    SoundManager.instance.rainAudioSource.volume = 1;
                    foreach (var p in groundRainDrops)
                    {
                        p.Play();
                    }
                    ResourceManager.instance.spawningBark = false;
                    SoundManager.instance.ToggleRain(true);
                    SoundManager.instance.ToggleWind(false);
                    StartCoroutine(Thunder());
                    rainOverlaySetTime = 0;
                    break;
                case Event.Wind:
                    lightRain.Stop();
                    HeavyRain.Stop();
                    Wind.Play();
                    fire.multiplier = windMultiplier;
                    rainOverlayAlpha = 0;
                    foreach (var p in groundRainDrops)
                    {
                        p.Stop();
                    }
                    rainOverlaySetTime = 0;
                    ResourceManager.instance.spawningBark = true;
                    SoundManager.instance.ToggleRain(false);
                    SoundManager.instance.ToggleWind(true);
                    break;
                default:
                    lightRain.Stop();
                    HeavyRain.Stop();
                    Wind.Stop();
                    fire.multiplier = 1;
                    rainOverlayAlpha = 0;
                    SoundManager.instance.ToggleWind(false);
                    foreach (var p in groundRainDrops)
                    {
                        p.Stop();
                    }
                    rainOverlaySetTime = 0;
                    ResourceManager.instance.spawningBark = false;
                    SoundManager.instance.ToggleRain(false);
                    break;
            }

            changingEvents = false;
        }

        if (!waitingForCooldown)
        {
            if (!waitingForEvent)
            {
                currentTime = 0;
                Random.InitState((int)(Time.time * 100));

                currentEvent = (Event)Random.Range(0, 4);

                while (currentEvent == Event.Cooldown || currentEvent == lastEvent)
                {
                    currentEvent = (Event)Random.Range(0, 4);
                }

                changingEvents = true;

                lastEvent = currentEvent;

                waitingForEvent = true;
                UIManager.instance.clock.SetActive(true);
            }
            else
            {
                currentTime += Time.deltaTime;
                
                UIManager.instance.SetClockHandRotation(Mathf.Lerp(365,5,currentTime/eventsDuration));

                if (currentTime >= eventsDuration)
                {
                    waitingForEvent = false;
                    currentEvent = Event.Cooldown;
                    changingEvents = true;
                    waitingForCooldown = true;
                    currentTime = 0;
                    UIManager.instance.clock.SetActive(false);
                }
            }
        }
        else
        {
            currentTime += Time.deltaTime;
            
            

            if (currentTime >= eventCooldown)
            {
                waitingForEvent = false;
                changingEvents = true;
                waitingForCooldown = false;
                
            }
        }

        if (rainOverlay.color.a != rainOverlayAlpha)
        {
            rainOverlaySetTime += Time.deltaTime;
            
            rainOverlay.color = new Color(rainOverlay.color.r, rainOverlay.color.g, rainOverlay.color.b, Mathf.Lerp(startingRainOverlayAlpha, rainOverlayAlpha, rainOverlaySetTime));

            if (rainOverlaySetTime >= rainOverlayAlpha)
            {
                rainOverlay.color = new Color(rainOverlay.color.r, rainOverlay.color.g, rainOverlay.color.b, rainOverlayAlpha);
            }
        }
        
    }

    public IEnumerator StartGame()
    {
        yield return new WaitForSeconds(10f);
        gameStarted = true;
    }

    public IEnumerator Thunder()
    {
        yield return new WaitForSeconds(Random.Range(7,13));

        if (currentEvent == Event.Storm)
        {
            SoundManager.instance.PlayThunder();
            StartCoroutine(Thunder());
        }
    }
}

public enum Event
{
    Normal,
    Rain,
    Storm,
    Wind,
    Cooldown
}