using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopDemo : MonoBehaviour
{
    [SerializeField] AudioSource laser;
    [SerializeField] AudioSource explosion;

    AudioTracker tracker;
    int loopCount;
    int loopStep;

    private void Start()
    {
        loopCount = Random.Range(4, 10);

        tracker = gameObject.AddComponent<AudioTracker>();
        tracker.onLoop = OnLoop;
        tracker.Track(laser);

        laser.Play();
    }

    void OnLoop(AudioTracker sender)
    {
        laser.pitch = UnityEngine.Random.Range(0.5f, 1.5f);
        loopStep++;

        if (loopStep >= loopCount)
        {
            laser.loop = false;
            tracker.onComplete = OnComplete;
        }
    }

    void OnComplete(AudioTracker sender)
    {
        explosion.Play();
    }
}
