using System;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{

    public Slider master, music, sfx;
    public AudioMixer audioMixer;
    public TMP_InputField name;
    public GameObject main, nameMenu;
    
    public TextMeshProUGUI[] leaderboardNameFields, leaderboardScoreFields;
    public TextMeshProUGUI personalNameField, personalScoreField, personalRankField;
    public GameObject personalItemParent;

    private void Start()
    {
        master.value = PlayerPrefs.GetFloat("MasterVolume", 0);
        music.value = PlayerPrefs.GetFloat("MusicVolume", -9);
        sfx.value = PlayerPrefs.GetFloat("SFXVolume", 0);

        if (PlayerPrefs.GetString("Name") != "")
        {
            nameMenu.SetActive(false);
            main.SetActive(true);
            GameManager.instance.name = PlayerPrefs.GetString("Name");
        }
        else
        {
            nameMenu.SetActive(true);
            main.SetActive(false);
        }
    }

    private void Update()
    {
        audioMixer.SetFloat("MasterVolume", master.value);
        audioMixer.SetFloat("MusicVolume", music.value);
        audioMixer.SetFloat("SFXVolume", sfx.value);
    }

    public void ApplyVolume()
    {
        PlayerPrefs.SetFloat("MasterVolume", master.value);
        PlayerPrefs.SetFloat("MusicVolume", music.value);
        PlayerPrefs.SetFloat("SFXVolume", sfx.value);
    }

    public void StartGame()
    {
        GameManager.instance.LoadGame();
    }

    public void UISFX()
    {
        SoundManager.instance.UIClick();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SetName()
    {
        if (name.text != "")
        {
            PlayerPrefs.SetString("Name", name.text);
            GameManager.instance.name = PlayerPrefs.GetString("Name");
            nameMenu.SetActive(false);
            main.SetActive(true);
        }
    }

    public void LoadLeaderboard()
    {
        
        GameManager.instance.LoadEntries();

        foreach (var t in leaderboardNameFields)
        {
            t.text = "";
        }
        foreach (var t in leaderboardScoreFields)
        {
            t.text = "";
        }
        
        for (int i = 0; i < GameManager.instance.leaderboardItems.Count; i++)
        {
            leaderboardNameFields[i].text = GameManager.instance.leaderboardItems[i].name;

            int minutes = (int)(GameManager.instance.leaderboardItems[i].seconds / 60);
            int seconds = GameManager.instance.leaderboardItems[i].seconds - (minutes * 60);
            
            leaderboardScoreFields[i].text = minutes.ToString("00") + ":" + seconds.ToString("00");
        }

        var personalItem = GameManager.instance.playerEntry;

        if (personalItem.seconds != 0 && personalItem.rank != 999)
        {
            personalItemParent.SetActive(true);
            
            personalNameField.text = personalItem.name;
            personalRankField.text = personalItem.rank.ToString();
        
            int m = (int)(personalItem.seconds / 60);
            int s = personalItem.seconds - (m * 60);
        
            personalScoreField.text = m.ToString("00") + ":" + s.ToString("00");
        }
        else
        {
            personalItemParent.SetActive(false);
        }
    }
}
