using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioMixer audioMixer;
    public AudioSource musicSource;
    public AudioClip clip;


    public float bgMusicVolume = 1f;
    public float sfxVolume = 1f;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        PlayMusic();
        LoadVolumeSettings();
    }

    public void PlayMusic()
    {
        musicSource.clip = clip;
        musicSource.Play();
    }

    public void LoadVolumeSettings()
    {
        bgMusicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        VolumeChanged(bgMusicVolume, sfxVolume);
    }

    public void VolumeChanged(float musicVolume, float sfxVolume)
    {
        audioMixer.SetFloat("Music", Mathf.Lerp(-80f, -20f, musicVolume));
        audioMixer.SetFloat("Combat", Mathf.Lerp(-80f, 0, sfxVolume));
        audioMixer.SetFloat("Collection", Mathf.Lerp(-80f, 0, sfxVolume));
    }
}