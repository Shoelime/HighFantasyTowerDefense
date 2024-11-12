using System;

public interface IWaveManager : IUpdateableService
{
    float TimeUntilNextWave { get; set; }
    public (int, int) WaveCounter();
    public bool IsLastWave();
    public event Action NewWaveStarted;
}
