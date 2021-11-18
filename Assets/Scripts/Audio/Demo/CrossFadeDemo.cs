using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossFadeDemo : MonoBehaviour
{
    [SerializeField] AudioSource fadeInSource;
    [SerializeField] AudioSource fadeOutSource;

    private void Start()
    {
        fadeInSource.volume = 0;
        fadeOutSource.volume = 1;

        fadeInSource.Play();
        fadeOutSource.Play();

        fadeInSource.VolumeTo(1);
        fadeOutSource.VolumeTo(0);
    }
}
