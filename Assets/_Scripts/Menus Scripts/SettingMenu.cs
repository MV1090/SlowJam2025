using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingMenu : BaseMenu
{

    [SerializeField] Slider sfxVolume;
    [SerializeField] Slider musicVolume;
    [SerializeField] AudioMixer audioMixer;
    public override void InitState(MenuManager ctx)
    {
        base.InitState(ctx);
        state = MenuManager.MenuStates.SettingsMenu;

        if (sfxVolume)
        {
            sfxVolume.onValueChanged.AddListener((value) => OnVolumeChanged(value, "SFXVolume"));
            float newValue;
            audioMixer.GetFloat("SFXVolume", out newValue);
            sfxVolume.value = newValue + 80;
        }

        if (musicVolume)
        {
            musicVolume.onValueChanged.AddListener((value) => OnVolumeChanged(value, "MusicVolume"));
            float newValue;
            audioMixer.GetFloat("MusicVolume", out newValue);
            musicVolume.value = newValue + 80;
        }

    }

    public override void EnterState()
    {
        base.EnterState();
        Time.timeScale = 0.0f;
    }

    public override void ExitState()
    {
        base.ExitState();
        Time.timeScale = 1.0f;
    }   

    void OnVolumeChanged(float value, string sliderName)
    {
        audioMixer.SetFloat(sliderName, value -80);
    }
}
