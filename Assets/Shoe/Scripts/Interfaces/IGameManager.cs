using System;

public interface IGameManager : IService
{
    LevelData GetLevelData { get; }

    Action VictoryEvent { get; set; }
    Action DefeatEvent { get; set; }
}
