using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public AudioClip introClip;
    public AudioClip loopClip;
    public AudioSequence sequence
    {
        get;
        private set;
    }

    private void Start()
    {
        sequence = gameObject.AddComponent<AudioSequence>();
        sequence.Play(introClip, loopClip);
        AudioSequenceData data = sequence.GetData(loopClip);
        data.source.loop = true;
    }
}
