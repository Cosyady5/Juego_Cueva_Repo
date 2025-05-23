using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public static AudioManager Instance
    {
        get
        {
            if (instance != null) Debug.Log("You already have an AudioManager");
            return instance;
        }
    }
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;
    public AudioClip[] musicLibrary;
    public AudioClip[] sfxLibrary;
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this.gameObject);
    }

    public void PlayMusic(int musicToPlay)
    {
        musicSource.clip = musicLibrary[musicToPlay];
    }
    public void StopMusic()
    {
        musicSource.Stop();
    }
    public void PlaySFX(int sfxToPlay)
    {
        sfxSource.PlayOneShot(sfxLibrary[sfxToPlay]);
    }
}
