using UnityEngine;

public class TimeManager : ITimeManager
{
    public float GameDuration;

    public void SetTimeScale(float timeScale)
    {
        Time.timeScale = timeScale;
    }

    void IUpdateableService.Update()
    {
        GameDuration += Time.deltaTime * Time.timeScale;
    }

    public void Initialize() { }
}
