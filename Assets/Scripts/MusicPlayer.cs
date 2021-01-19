using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicPlayer : MonoBehaviour
{

    AudioSource audioSource;
    public static MusicPlayer musicPlayer = null;
    private void Awake()
    {
        if (musicPlayer == null)
        {
            musicPlayer = this;
        }
        else if (musicPlayer != this)
        {
            Destroy(gameObject);
        }
    }



    void Start()
    {
        DontDestroyOnLoad(this);
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = PlayerPrefsController.GetMasterVolume();
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name != "StartScreen" && SceneManager.GetActiveScene().name != "SplashScreen" &&
            SceneManager.GetActiveScene().name != "OptionsScreen" && SceneManager.GetActiveScene().name != "StatsScreen")
        {
            Destroy(gameObject);
        }
    }
}
