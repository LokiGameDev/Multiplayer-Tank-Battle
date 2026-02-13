using UnityEngine;
using UnityEngine.UI;

public class SoundSettingsSaver : MonoBehaviour
{
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;

    private const string MusicVolumeKey = "MusicVolume";
    private const string SFXVolumeKey = "SFXVolume";

    private void Start()
    {
        // Load saved settings
        musicVolumeSlider.value = PlayerPrefs.GetFloat(MusicVolumeKey, 1f);
        sfxVolumeSlider.value = PlayerPrefs.GetFloat(SFXVolumeKey, 1f);
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetFloat(MusicVolumeKey, musicVolumeSlider.value);
        PlayerPrefs.SetFloat(SFXVolumeKey, sfxVolumeSlider.value);

        AudioManager.Instance.VolumeChanged(musicVolumeSlider.value, sfxVolumeSlider.value);

        PlayerPrefs.Save();
    }

    public void ResetToDefault()
    {
        musicVolumeSlider.value = 1f;
        sfxVolumeSlider.value = 1f;
        SaveSettings();
    }
}
