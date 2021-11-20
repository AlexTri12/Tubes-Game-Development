using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class VolumeData : MonoBehaviour
{
    public static VolumeData INSTANCE;
    public int bgmVolume
    {
        get { return _bgmVolume; }
        set
        {
            _bgmVolume = value;
            this.PostNotification(BGMVolumeChanged, _bgmVolume);
        }
    }
    public int sfxVolume
    {
        get { return _sfxVolume; }
        set
        {
            _sfxVolume = value;
            this.PostNotification(SFXVolumeChanged, _sfxVolume);
        }
    }
    int _bgmVolume;
    int _sfxVolume;

    public static string BGMVolumeChanged = "VolumeData.bgmVolumeChanged";
    public static string SFXVolumeChanged = "VolumeData.sfxVolumeChanged";

    const string saveName = "/volume_data.json";

    private void Awake()
    {
        if (INSTANCE != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            INSTANCE = this;
            DontDestroyOnLoad(gameObject);
            LoadVolumeSettings();
        }
    }

    [System.Serializable]
    class SaveVolumeData
    {
        public int bgmVolume;
        public int sfxVolume;
    }

    public void LoadVolumeSettings()
    {
        string path = Application.persistentDataPath + saveName;
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveVolumeData data = JsonUtility.FromJson<SaveVolumeData>(json);

            bgmVolume = data.bgmVolume;
            sfxVolume = data.sfxVolume;
        }
        else
        {
            bgmVolume = 100;
            sfxVolume = 100;
        }
    }

    public void SaveVolumeSettings()
    {
        SaveVolumeData data = new SaveVolumeData();
        data.bgmVolume = bgmVolume;
        data.sfxVolume = sfxVolume;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + saveName, json);
    }
}
