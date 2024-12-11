using System.Collections.Generic;
using UnityEngine;

public class MusicGameSpeedController : MonoBehaviour
{
    [SerializeField] private MusicPlayer musicPlayer;
    [SerializeField] private List<SpeedBoostInterval> speedBoostIntervals;

    private int currentTrackIndex = -1;

    private void Update()
    {
        if (musicPlayer == null || speedBoostIntervals == null || speedBoostIntervals.Count == 0)
            return;

        var trackTime = musicPlayer.GetTrackTime();
        var trackIndex = musicPlayer.GetCurrentTrackIndex();

        if (trackIndex != currentTrackIndex)
        {
            currentTrackIndex = trackIndex;
        }

        foreach (var interval in speedBoostIntervals)
        {
            if (interval.trackIndex == currentTrackIndex &&
                trackTime >= interval.startTime && trackTime <= interval.endTime)
            {
                Time.timeScale = interval.speedBoost;
                return;
            }
        }

        Time.timeScale = 1f;
    }
}

[System.Serializable]
public class SpeedBoostInterval
{
    public int trackIndex;
    public float startTime;
    public float endTime;
    public float speedBoost = 1f;
}