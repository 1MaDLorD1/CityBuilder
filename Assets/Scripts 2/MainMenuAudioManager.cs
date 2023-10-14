using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuAudioManager : MonoBehaviour
{
    private static MainMenuAudioManager _instance;

    public static MainMenuAudioManager Instance { get => _instance; }

    public AudioClip buttonClickedSound;
    public AudioSource effectAudioSource;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public void PlayButtonClickedSound()
    {
        effectAudioSource.Stop();
        effectAudioSource.clip = buttonClickedSound;
        effectAudioSource.Play();
    }
}
