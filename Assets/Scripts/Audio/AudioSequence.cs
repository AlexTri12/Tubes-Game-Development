using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSequence : MonoBehaviour
{
    private enum PlayMode
    {
        Stopped,
        Playing,
        Paused
    }

    Dictionary<AudioClip, AudioSequenceData> playMap = new Dictionary<AudioClip, AudioSequenceData>();
    PlayMode playMode = PlayMode.Stopped;
    double pauseTime;

    public void Play(params AudioClip[] clips)
    {
        if (playMode == PlayMode.Stopped)
            playMode = PlayMode.Playing;
        else if (playMode == PlayMode.Paused)
            UnPause();

        double startTime = GetNextStartTime();
        float defaultVolume = (float)VolumeData.INSTANCE.bgmVolume / 100;
        for (int i = 0; i < clips.Length; ++i)
        {
            AudioClip clip = clips[i];
            AudioSequenceData data = GetData(clip);
            data.Schedule(startTime);
            data.source.volume = defaultVolume;
            startTime += clip.length;
        }
    }

    public void Pause()
    {
        if (playMode != PlayMode.Playing)
            return;
        playMode = PlayMode.Paused;

        pauseTime = AudioSettings.dspTime;
        foreach (AudioSequenceData data in playMap.Values)
            data.source.Pause();
    }

    public void UnPause()
    {
        if (playMode != PlayMode.Paused)
            return;
        playMode = PlayMode.Playing;

        double elapsedTime = AudioSettings.dspTime - pauseTime;
        foreach (AudioSequenceData data in playMap.Values)
        {
            if (data.isScheduled)
                data.Schedule(data.startTime + elapsedTime);
            data.source.UnPause();
        }
    }

    public void Stop()
    {
        playMode = PlayMode.Stopped;
        foreach (AudioSequenceData data in playMap.Values)
        {
            data.Stop();
        }
    }

    public AudioSequenceData GetData(AudioClip clip)
    {
        if (!playMap.ContainsKey(clip))
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = clip;
            playMap[clip] = new AudioSequenceData(source);
        }
        return playMap[clip];
    }

    AudioSequenceData GetLast()
    {
        double highestEndTime = double.MinValue;
        AudioSequenceData lastData = null;
        foreach (AudioSequenceData data in playMap.Values)
        {
            if (data.isScheduled && data.endTime > highestEndTime)
            {
                highestEndTime = data.endTime;
                lastData = data;
            }
        }
        return lastData;
    }

    double GetNextStartTime()
    {
        AudioSequenceData lastToPlay = GetLast();
        if (lastToPlay != null && lastToPlay.endTime > AudioSettings.dspTime)
            return lastToPlay.endTime;
        else
            return AudioSettings.dspTime;
    }

    // Change volume from the volume settings
    private void Awake()
    {
        this.AddObserver(OnBGMChanged, VolumeData.BGMVolumeChanged);
    }

    private void OnDestroy()
    {
        this.RemoveObserver(OnBGMChanged, VolumeData.BGMVolumeChanged);
    }

    void OnBGMChanged(object sender, object args)
    {
        float value = (float)((int)args) / 100;
        foreach (AudioSequenceData data in playMap.Values)
        {
            data.source.VolumeTo(value);
        }
    }
}
