using System.Collections.Generic;
using UnityEngine;

public class MusicGameSpeedController : MonoBehaviour
{
    [SerializeField] private MusicPlayer musicPlayer;
    [SerializeField] private List<SpeedBoostInterval> speedBoostIntervals;

    private int currentSpeedBoostIndex = 0;

    private void Update()
    {
        if (musicPlayer == null || speedBoostIntervals == null || speedBoostIntervals.Count == 0)
            return;

        var currentTrackTime = musicPlayer.GetTrackTime();
        var currentTrackIndex = musicPlayer.GetCurrentTrackIndex();

        if (currentTrackIndex != currentSpeedBoostIndex)
        {
            currentSpeedBoostIndex = currentTrackIndex;
        }

        foreach (var interval in speedBoostIntervals)
        {
            if (currentTrackTime >= interval.startTime && currentTrackTime <= interval.endTime)
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
    public float startTime;
    public float endTime;
    public float speedBoost = 1f;

}