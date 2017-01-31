using UnityEngine;
using System;

[Serializable]
public class Settings
{
    [SerializeField]
    public string Name = "Settings_1";

    [SerializeField]
    string buildVersion = "";

    public bool UpdateVersion() { if (buildVersion == Application.version) return false;
        buildVersion = Application.version;
        return true;
    }
    //    [SerializeField]
    //    public float soundVolume = 0.7f;
    public void iniDefaults()
    {

        ls_musicVolume = musicVolume;
        ls_music_on = music_on;
        ls_soundVolume = soundVolume;
        ls_sound_on = sound_on;
        ls_infinity_fire_on = infinity_fire_on;
        ls_show_coins_hud = show_coins_hud;
    }

    #region playerPrefs
    [SerializeField]
    [Range(0f, 1f)]
    float musicVolume=.4f;
    [SerializeField]
    bool music_on = true;
    [SerializeField]
    [Range(0f, 1f)]
    float soundVolume = .4f;
    [SerializeField]
    bool sound_on=true;
    [SerializeField]
    bool infinity_fire_on;
    [SerializeField]
    bool show_coins_hud;
    [SerializeField]
    int current_level_index;

    [NonSerialized]
    float ls_musicVolume;
    [NonSerialized]
    float ls_soundVolume;
    [NonSerialized]
    bool ls_infinity_fire_on = false;
    [NonSerialized]
    bool ls_show_coins_hud = false;
    [NonSerialized]
    bool ls_music_on = false;
    [NonSerialized]
    bool ls_sound_on= false;

    public int Current_level_index { get { return current_level_index; } set { current_level_index=value; } }
    public float MusicVolume { get { return musicVolume; } set { musicVolume = Mathf.Clamp(value, 0, 1); SafeCallback(); } }
    public float SoundVolume { get { return soundVolume; } set { soundVolume = Mathf.Clamp(value, 0, 1); SafeCallback(); } }
    public bool Infinity_fire_on { get { return infinity_fire_on; } set { infinity_fire_on=value; SafeCallback(); } }
    public bool Music_on { get { return music_on; } set { music_on= value; SafeCallback(); } }
    public bool Sound_on { get { return sound_on; } set { sound_on= value; SafeCallback(); } }
    public bool Show_coins_hud { get { return show_coins_hud; } set { show_coins_hud = value; SafeCallback(); } }
    public void SaveSettingValues()
    {
        ls_musicVolume = musicVolume;
        ls_music_on = music_on;
        ls_soundVolume = soundVolume;
        ls_sound_on = sound_on;
        ls_infinity_fire_on = infinity_fire_on;
        ls_show_coins_hud = show_coins_hud;
    }
    [NonSerialized]
    public Action onSettingsChanged;
    void SafeCallback()
    {
        if (onSettingsChanged != null)
            onSettingsChanged.Invoke();
    }

    public void RevertSettings()
    {
        musicVolume = ls_musicVolume;
        music_on = ls_music_on;
        soundVolume = ls_soundVolume;
        sound_on = ls_sound_on;
        infinity_fire_on = ls_infinity_fire_on;
        show_coins_hud = ls_show_coins_hud;
        SafeCallback();
    }

    #endregion playerPrefs
    
}

