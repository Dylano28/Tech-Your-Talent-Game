using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RandomAudio : MonoBehaviour
{
    [SerializeField] private AudioClip[] clips; // Audio clips
    public AudioClip[] Clips => clips;

    [SerializeField] [Range(0.01f, 3)] private float maxPitch = 1f; // Maximum or minimum pitch of the sound (Leave at 1 for no randomized pitch)
    [SerializeField] [Range(0, 1)] private float minVolume = 1f; // Minimum volume of the sound (Leave at 1 for no randomized volume)
    [SerializeField] private bool playOnAwake = false; // Play when object is created

    private AudioSource _audio;


    // Set up
    private void Start()
    {
        _audio = gameObject.GetComponent<AudioSource>(); // Create new audiosource
        _audio.playOnAwake = false;
        if (playOnAwake) PlaySound();
    }


    // Play a random sound
    public void PlaySound()
    {
        var play_clip = clips[Random.Range(0, clips.Length)];
        _audio.clip = play_clip;

        var pitch = maxPitch != 1f ? Random.Range(1, maxPitch) : _audio.pitch; // Pick random number of pitch
        var volume = minVolume != 1f ? Random.Range(minVolume, 1) : _audio.volume; // Pick random number of volume

        _audio.pitch = pitch;
        _audio.volume = volume;

        _audio.Play();
    }
}
