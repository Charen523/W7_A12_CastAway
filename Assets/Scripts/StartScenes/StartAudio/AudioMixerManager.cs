using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioMixerManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider sfmSlider;
    [SerializeField] private Slider bgmSlider;

    private void Awake()
    {
        masterSlider.onValueChanged.AddListener(SetmasterVolume);
        sfmSlider.onValueChanged.AddListener(SetsfxVolume);
        bgmSlider.onValueChanged.AddListener(SetbgmVolume);

    }

    public void SetbgmVolume(float volume)
    {
        audioMixer.SetFloat("BGM", Mathf.Log10(volume) * 20);
    }

    public void SetsfxVolume(float volume)
    {
        audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
    }

    public void SetmasterVolume(float volume)
    {
        audioMixer.SetFloat("Master", Mathf.Log10(volume) * 20);
    }
}
