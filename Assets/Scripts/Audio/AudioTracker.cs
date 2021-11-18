using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioTracker : MonoBehaviour
{
    // Triggers when an audio source isPlaying changes to true (play or unpause)
    public Action<AudioTracker> onPlay;
    // Triggers when an audio source isPlaying changes to false without completing (pause)
    public Action<AudioTracker> onPause;
    // Triggers when an audio source isPlaying changes to false (stop or played to end)
    public Action<AudioTracker> onComplete;
    // Triggers when an audio source repeats
    public Action<AudioTracker> onLoop;

    public bool autoStop = true;
    public AudioSource source
    {
        get;
        private set;
    }
    private float lastTime;
    private bool lastIsPlaying;

    const string trackingCoroutine = "TrackSequence";

    public void Track(AudioSource source)
    {
        Cancel();
        this.source = source;
        if (source != null)
        {
            lastTime = source.time;
            lastIsPlaying = source.isPlaying;
            StartCoroutine(trackingCoroutine);
        }
    }

    public void Cancel()
    {
        StopCoroutine(trackingCoroutine);
    }

    IEnumerator TrackSequence()
    {
        while (true)
        {
            yield return null;
            SetTime(source.time);
            SetIsPlaying(source.isPlaying);
        }
    }

    void AudioSourceBegan()
    {
        if (onPlay != null)
            onPlay(this);
    }

    void AudioSourceLooped()
    {
        if (onLoop != null)
            onLoop(this);
    }

    void AudioSourceCompleted()
    {
        if (onComplete != null)
            onComplete(this);
    }

    void AudioSourcePaused()
    {
        if (onPause != null)
            onPause(this);
    }

    void SetIsPlaying(bool isPlaying)
    {
        if (lastIsPlaying == isPlaying)
            return;

        lastIsPlaying = isPlaying;

        if (isPlaying)
            AudioSourceBegan();
        else if (Mathf.Approximately(source.time, 0))
            AudioSourceCompleted();
        else
            AudioSourcePaused();

        if (isPlaying == false && autoStop == true)
            StopCoroutine(trackingCoroutine);
    }

    void SetTime(float time)
    {
        if (lastTime > time)
            AudioSourceLooped();

        lastTime = time;
    }
}
