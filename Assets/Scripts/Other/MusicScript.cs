using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] private List<AudioClip> tracks;
    [SerializeField, Range(0f, 1f)] private float volume = 0.5f;
    private AudioSource audioSource;
    private int currentTrackIndex = 0;

    public bool IsActive { get; private set; } = true; 

    private void Start()
    {
        if (tracks == null || tracks.Count == 0)
        {
            Debug.LogWarning("Не добавлены треки в список.");
            return;
        }

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = false;
        audioSource.playOnAwake = false;
        audioSource.volume = volume;

        PlayCurrentTrack();
    }

    private void Update()
    {
        if (!audioSource.isPlaying && tracks.Count > 0 && IsActive)
        {
            NextTrack();
        }
    }

    public void PauseMusic()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Pause();
            IsActive = false;
        }
    }

    public void ResumeMusic()
    {
        if (!audioSource.isPlaying && !IsActive)
        {
            audioSource.UnPause();
            IsActive = true;
        }
    }

    private void PlayCurrentTrack()
    {
        if (currentTrackIndex >= 0 && currentTrackIndex < tracks.Count)
        {
            audioSource.clip = tracks[currentTrackIndex];
            audioSource.Play();
        }
    }

    private void NextTrack()
    {
        currentTrackIndex = (currentTrackIndex + 1) % tracks.Count;
        PlayCurrentTrack();
    }
}