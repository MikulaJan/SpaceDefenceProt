using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    public AudioClip menuMusic; // Music for the menu
    public AudioClip gameMusic; // Music for the gameplay

    public float fadeDuration = 2.0f; // Duration of fade in/out in seconds

    private AudioSource audioSource;
    private bool fading;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // Play menu music when the game starts
        PlayMenuMusic();
    }

    void Update()
    {
        // Check current scene and play appropriate music
        Scene currentScene = SceneManager.GetActiveScene();

        if (currentScene.name == "MenuScene")
        {
            if (!audioSource.isPlaying || audioSource.clip != menuMusic)
            {
                FadeOutAndPlayNewMusic(menuMusic);
            }
        }
        else // Assuming other scenes are for gameplay
        {
            if (!audioSource.isPlaying || audioSource.clip != gameMusic)
            {
                FadeOutAndPlayNewMusic(gameMusic);
            }
        }
    }

    void PlayMenuMusic()
    {
        audioSource.clip = menuMusic;
        audioSource.Play();
        fading = false;
    }

    void FadeOutAndPlayNewMusic(AudioClip newClip)
    {
        if (!fading)
        {
            fading = true;
            StartCoroutine(FadeOutMusicAndPlayNew(newClip));
        }
    }

    IEnumerator FadeOutMusicAndPlayNew(AudioClip newClip)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
        fading = false;

        // Play new music with fade in
        PlayMusicWithFadeIn(newClip);
    }

    void PlayMusicWithFadeIn(AudioClip newClip)
    {
        audioSource.clip = newClip;
        audioSource.Play();

        float targetVolume = audioSource.volume;
        audioSource.volume = 0;

        StartCoroutine(FadeInMusic(targetVolume));
    }

    IEnumerator FadeInMusic(float targetVolume)
    {
        while (audioSource.volume < targetVolume)
        {
            audioSource.volume += targetVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        audioSource.volume = targetVolume;
        fading = false;
    }
}
