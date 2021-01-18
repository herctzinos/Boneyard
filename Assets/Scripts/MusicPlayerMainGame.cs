using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicPlayerMainGame : MonoBehaviour
{
    AudioSource audioSource;

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
        if ((SceneManager.GetActiveScene().name != "GameScene") && (SceneManager.GetActiveScene().name != "GameOverScene")) { 
            Destroy(gameObject);
           // Destroy(audioSource);
        }

    }
}
