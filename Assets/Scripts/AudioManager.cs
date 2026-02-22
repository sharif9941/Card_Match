using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    AudioSource AudioSource;

    [Header("Audio Clips")]
    [SerializeField] AudioClip cardflip;
    [SerializeField] AudioClip match;
    [SerializeField] AudioClip mismatch;
    [SerializeField] AudioClip gameover;

    void Awake()
    {
        Instance = this;
        AudioSource = GetComponent<AudioSource>();
    }

    public void PlayCardFlip()
    {
        AudioSource.PlayOneShot(cardflip);
    }

    public void PlayMatch()
    {
        AudioSource.PlayOneShot(match);
    }

    public void PlayMismatch()
    {
        AudioSource.PlayOneShot(mismatch);
    }

    public void PlayGameOver()
    {
        AudioSource.PlayOneShot(gameover);
    }
}
