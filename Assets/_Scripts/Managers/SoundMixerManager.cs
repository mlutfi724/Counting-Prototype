using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SoundMixerManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider musicSlider;

    private void Start()
    {
        LoadAudioSettings();
    }

    public void SetMasterVolume(float level)
    {
        audioMixer.SetFloat("masterVolume", level);
        PlayerPrefs.SetFloat("MasterVolumeLevel", level);
        //audioMixer.SetFloat("masterVolume", Mathf.Log10(level) * 20);
    }

    public void SetSFXVolume(float level)
    {
        audioMixer.SetFloat("soundFXVolume", level);
        PlayerPrefs.SetFloat("SoundFXVolumeLevel", level);
        //audioMixer.SetFloat("soundFXVolume", Mathf.Log10(level) * 20);
    }

    public void SetMusicVolume(float level)
    {
        audioMixer.SetFloat("musicVolume", level);
        PlayerPrefs.SetFloat("MusicVolumeLevel", level);
        //audioMixer.SetFloat("musicVolume", Mathf.Log10(level) * 20);
    }

    private void LoadAudioSettings()
    {
        masterSlider.value = PlayerPrefs.GetFloat("MasterVolumeLevel");
        sfxSlider.value = PlayerPrefs.GetFloat("SoundFXVolumeLevel");
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolumeLevel");

        audioMixer.SetFloat("musicVolume", masterSlider.value);
        audioMixer.SetFloat("soundFXVolume", sfxSlider.value);
        audioMixer.SetFloat("musicVolume", musicSlider.value);
    }
}