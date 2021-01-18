using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicPlayer : MonoBehaviour
{

    AudioSource audioSource;
   /// public MusicPlayer musicPlayer;
    // Start is called before the first frame update
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
        if (SceneManager.GetActiveScene().name != "Start Screen" && SceneManager.GetActiveScene().name != "Splash Screen" && SceneManager.GetActiveScene().name != "Options Screen")
        {
            //Destroy(audioSource);
            Destroy(gameObject);
        }

/*        if ((SceneManager.GetActiveScene().name == "Start Screen") && (!audioSource.isPlaying)) {

            Destroy(gameObject);
            MusicPlayer newMusicPlayer = Instantiate<MusicPlayer>(musicPlayer);

            newMusicPlayer.GetComponent<AudioSource>().Play();
            Debug.Log("Played");

        }*/
  
    }

    private object FindSceneObjectOfType<T>()
    {
        throw new NotImplementedException();
    }
}
