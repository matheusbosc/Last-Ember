using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Timers;
using TMPro;
using Dan.Main;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour 
{
    public static GameManager instance;
    public GameObject PauseMenu;
    public bool isPaused = false;
    public GameObject loadingScreen;
    public GameObject endScreen;

    public GameObject loadingCamera;
    
    public bool isPlaying = false;
    public Stopwatch gameTimer = new Stopwatch();
    
    public TextMeshProUGUI gameTimerText;
    
    public List<LeaderboardItem> leaderboardItems;
    public int maxLeaderboardItems;
    public LeaderboardItem playerEntry;

    [HideInInspector] public string name;
    
    private void Start()
    {
        instance = this;
        SceneManager.LoadSceneAsync("Main Menu", LoadSceneMode.Additive);
        LoadEntries();
    }

    private void Update()
    {
        if (isPlaying)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                

                if (isPaused)
                {
                    PauseMenu.SetActive(false);
                    isPaused = false;
                    SoundManager.instance.musicAudioSource.UnPause();
                    SoundManager.instance.UIClick();
                }
                else
                {
                    PauseMenu.SetActive(true);
                    isPaused = true;
                    SoundManager.instance.musicAudioSource.Pause();
                    SoundManager.instance.UIClick();
                    
                }
            }
            
        }
    }

    public void Unpause()
    {
        PauseMenu.SetActive(false);
        isPaused = false;
        SoundManager.instance.musicAudioSource.UnPause();
    }

    public void LoadGame()
    {
        StartCoroutine(TransitionRoutine("Game", "Main Menu"));
    }
    
    public void LoadMenu()
    {
        
        StartCoroutine(TransitionRoutine("Main Menu", "Game"));


    }
    
    IEnumerator TransitionRoutine(string loadScene, string unloadScene)
    {
        loadingScreen.SetActive(true);
        loadingCamera.SetActive(true);

        AsyncOperation loadOp =
            SceneManager.LoadSceneAsync(loadScene, LoadSceneMode.Additive);

        AsyncOperation unloadOp =
            SceneManager.UnloadSceneAsync(unloadScene);

        // Wait for BOTH operations to finish
        while (!loadOp.isDone || !unloadOp.isDone)
            yield return null;
        

        SoundManager.instance.ChangeScene(loadScene);

        if (loadScene == "Game")
        {
            gameTimer.Start();
            isPlaying = true;
        }
        else isPlaying = false;

        isPaused = false;
        Unpause();
        
        loadingScreen.SetActive(false);
        loadingCamera.SetActive(false);
    }

    public void EndGame()
    {
        TimeSpan ts = gameTimer.Elapsed;
        
        Leaderboards.LastEmber.UploadNewEntry(name, (int) ts.TotalSeconds, isSuccessful =>
        {
            if (isSuccessful)
                LoadEntries();
        });
        
        string elapsedTime = String.Format("{0:00}:{1:00}",
            ts.Minutes, ts.Seconds);
        
        gameTimerText.text = $"You Survived {elapsedTime}";
        gameTimer.Stop();
    }
    
    public void LoadEntries()
    {
        
        Leaderboards.LastEmber.GetEntries(entries =>
        {
            leaderboardItems.Clear();
            
            var length = Mathf.Min(maxLeaderboardItems, entries.Length);
            for (int i = 0; i < length; i++)
            {
                leaderboardItems.Add(new LeaderboardItem { name = entries[i].Username, seconds = entries[i].Score, rank = entries[i].Rank });
            }
                
        });
        
        Leaderboards.LastEmber.GetPersonalEntry(entry =>
        {
            if (entry.Score != 0)
            {
                playerEntry = new LeaderboardItem {name = entry.Username, seconds = entry.Score,  rank = entry.Rank};
            }
            else
            {
                playerEntry = new LeaderboardItem {name = entry.Username, seconds = 0, rank = 999};
            }
            
        });
    }
    
    
}

[Serializable]
public class LeaderboardItem
{
    public string name;
    public int seconds;
    public int rank;
}
