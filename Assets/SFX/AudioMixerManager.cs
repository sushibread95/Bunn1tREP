using UnityEngine;
using UnityEngine.Audio;


public class AudioMixerManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    public void SetMasterVolume(float level)
    {
        audioMixer.SetFloat("masterVolume", Mathf.Log10(level) * 20f);

    }
    public void SetSoundFXVolume(float level)
    {
        audioMixer.SetFloat("effectsVolume", Mathf.Log10(level) * 20f);
    }
    public void SetMusicVolume(float level)
    {
        audioMixer.SetFloat("musicVolume", Mathf.Log10(level) * 20f);

    }
    public void SetAmbianceVolume(float level)
    {
        audioMixer.SetFloat("ambianceVolume", Mathf.Log10(level) * 20f);

    }
}
