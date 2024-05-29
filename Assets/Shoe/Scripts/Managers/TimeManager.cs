using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : IUpdateableService
{
    public float GameDuration;

    public void Initialize()
    {

    }

    void SetTimeScale(float timeScale)
    {
        Time.timeScale = timeScale;
    }

    void IUpdateableService.Update()
    {
        GameDuration += Time.deltaTime * Time.timeScale;
    }
}
