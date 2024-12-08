using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] private List<AudioClip> _tracks;
    [SerializeField, Range(0f, 1f)] private float volume = 0.5f;
    private AudioSource _audioSource;
    private int _currentTrackIndex = 0;
    private bool _isTransitioning = false;
    private AudioClip _bossMusic;

    public bool IsActive { get; private set; } = true;

    private void Start()
    {
        if (_tracks == null || _tracks.Count == 0)
        {
            Debug.LogWarning("Треки не указаны.");
            return;
        }

        Initialize();
        PlayCurrentTrack();
    }

    private void Update()
    {
        if (!_audioSource.isPlaying && _tracks.Count > 0 && IsActive && !_isTransitioning)
        {
            NextTrack();
        }
    }

    public void PauseMusic()
    {
        if (_audioSource.isPlaying)
        {
            _audioSource.Pause();
            IsActive = false;
        }
    }

    public void ResumeMusic()
    {
        if (!_audioSource.isPlaying && !IsActive)
        {
            _audioSource.UnPause();
            IsActive = true;
        }
    }

    private void PlayCurrentTrack()
    {
        if (_currentTrackIndex >= 0 && _currentTrackIndex < _tracks.Count)
        {
            StartCoroutine(PlayTrackWithFade(_tracks[_currentTrackIndex]));
        }
    }

    public float GetTrackTime()
    {
        if (_audioSource != null)
        {
            return _audioSource.time;
        }
        return 0f;
    }

    public int GetCurrentTrackIndex()
    {
        return _currentTrackIndex;
    }

    public void Initialize()
    {
        if (_audioSource == null)
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
            _audioSource.loop = false;
            _audioSource.playOnAwake = false;
            _audioSource.volume = volume;
        }

        if (_tracks.Count > 0 && !_audioSource.isPlaying)
        {
            PlayCurrentTrack();
        }
    }

    private IEnumerator PlayTrackWithFade(AudioClip clip)
    {
        _isTransitioning = true;
        _audioSource.Stop();
        _audioSource.clip = clip;
        _audioSource.Play();

        float _fadeTime = 1f;
        float _startVolume = 0f;
        float _currentTime = 0f;

        while (_currentTime < _fadeTime && _audioSource.isPlaying)
        {
            _audioSource.volume = Mathf.Lerp(_startVolume, volume, _currentTime / _fadeTime);
            _currentTime += Time.deltaTime;
            yield return null;
        }
        _audioSource.volume = volume;
        _isTransitioning = false;
    }

    public void PlayBossMusic(float volume)
    {
        if (_bossMusic != null)
        {
            StartCoroutine(PlayBossMusicWithFade(_bossMusic, volume));
        }
        else
        {
            Debug.LogError("Boss music not loaded!");
        }
    }

    private IEnumerator PlayBossMusicWithFade(AudioClip bossMusic, float volume)
    {
        _isTransitioning = true;
        _audioSource.Stop();
        _audioSource.clip = bossMusic;
        _audioSource.volume = volume;
        _audioSource.Play();
        _isTransitioning = false;
        yield return null;
    }

    private void NextTrack()
    {
        _currentTrackIndex = (_currentTrackIndex + 1) % _tracks.Count;
        PlayCurrentTrack();
    }

    public void LoadBossMusic(AudioClip bossMusicClip)
    {
        _bossMusic = bossMusicClip;
        Debug.Log("Boss music loaded.");
    }

    IEnumerator LoadAudioClipAsync(AudioClip clipToLoad)
    {
        if (clipToLoad == null) yield break;
        var op = Resources.LoadAsync<AudioClip>(clipToLoad.name);
        yield return op;
        _bossMusic = op.asset as AudioClip;
        Debug.Log("Boss music loaded asynchronously.");
    }

    public int CurrentTrackIndex => _currentTrackIndex;

    public float CurrentTrackTime
    {
        get
        {
            if (_audioSource != null)
            {
                return _audioSource.time;
            }
            return 0f;
        }
    }
}