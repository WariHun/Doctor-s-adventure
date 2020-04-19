using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;   //Audiomixer
    public Slider volumeSlider; //Hang csuszka
    public TMP_Dropdown resDrop;    //Felbontás
    public TMP_Dropdown qDrop;  //Minőség
    public Toggle fsToggle; //Teljes képernyő

    //Változók a kezeléshez
    private bool fulls;
    private float vol;
    private int qIndex;
    private int rIndex;

    static private Resolution[] resolutions;

    //Betöltéskor frissítse a menüt
    private void OnEnable()
    {
        List<string> options = new List<string>();

        resolutions = Screen.resolutions;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = $"{resolutions[i].width} x {resolutions[i].height} @ {resolutions[i].refreshRate}Hz";
            options.Add(option);
        }
        
        resDrop.ClearOptions();
        resDrop.AddOptions(options);
        resDrop.RefreshShownValue();

        ResetToPrev();
        Apply();
    }

    /// <summary>
    /// Hangerő állítása
    /// </summary>
    public void SetVolume(float volume)
    {
        vol = volume;
    }

    /// <summary>
    /// Minőség állítása
    /// </summary>
    public void SetQuality(int index)
    {
        qIndex = index;
    }

    /// <summary>
    /// Felbontás állítása
    /// </summary>
    public void SetResolution(int index)
    {
        rIndex = index;
    }

    /// <summary>
    /// Teljes képernyő állítása
    /// </summary>
    public void SetFullscreen(bool fs)
    {
        fulls = fs;
    }

    /// <summary>
    /// Beállítások alkalmazása
    /// </summary>
    public void Apply()
    {
        //Felbontás és fs
        PlayerPrefs.SetInt("resIndex", rIndex);
        PlayerPrefs.SetInt("fs", BoolConverter(fulls));

        Screen.SetResolution(resolutions[rIndex].width, resolutions[rIndex].height, fulls);

        //Quality
        PlayerPrefs.SetInt("qIndex", qIndex);
        QualitySettings.SetQualityLevel(qIndex);

        //Hangerő
        PlayerPrefs.SetFloat("Volume", vol);
        audioMixer.SetFloat("volume", vol);

        PlayerPrefs.Save();
    }

    /// <summary>
    /// Visszaállítás változtatás elöttre
    /// </summary>
    public void ResetToPrev()
    { 
        //Felbontás
        resDrop.value = PlayerPrefs.GetInt("resIndex");
        rIndex = PlayerPrefs.GetInt("resIndex");

        //Teljes képernyő
        fsToggle.isOn = BoolConverter(PlayerPrefs.GetInt("fs"));
        fulls = BoolConverter(PlayerPrefs.GetInt("fs"));

        //Quality
        qDrop.value = PlayerPrefs.GetInt("qIndex");
        qIndex = PlayerPrefs.GetInt("qIndex");

        //Hangerő
        volumeSlider.value = PlayerPrefs.GetFloat("Volume");
        vol = PlayerPrefs.GetFloat("Volume");
    }

    /// <summary>
    /// Int átalakítása boolra
    /// </summary>
    public bool BoolConverter(int fs)
    {
        if (fs == 1) return true;
        else return false;
    }

    /// <summary>
    /// Bool átalakítása intre
    /// </summary>
    public int BoolConverter(bool fs)
    {
        if (fs) return 1;
        else return 0;
    }
}
