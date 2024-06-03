using System;
using UnityEngine;

public class TimeManager : ITimeManager
{
    public float GameDuration;

    public void SetTimeScale(float timeScale)
    {
        Time.timeScale = timeScale;
    }

    internal void Initialize()
    {
    }

    void IUpdateableService.Update()
    {
        GameDuration += Time.deltaTime * Time.timeScale;
    }
}
