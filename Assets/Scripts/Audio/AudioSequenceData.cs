using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSequenceData
{
    public double startTime
    {
        get;
        private set;
    }
    public readonly AudioSource source;

    public bool isScheduled
    {
        get { return startTime > 0; }
    }
    public double endTime
    {
        get { return startTime + source.clip.length; }
    }

    public AudioSequenceData(AudioSource source)
    {
        this.source = source;
        startTime = -1;
    }

    public void Schedule(double time)
    {
        if (isScheduled)
            source.SetScheduledStartTime(time);
        else
            source.PlayScheduled(time);
        startTime = time;
    }

    public void Stop()
    {
        startTime = -1;
        source.Stop();
    }
}
