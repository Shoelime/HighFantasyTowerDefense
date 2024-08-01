using System;
using System.Collections.Generic;
using UnityEngine;

public interface IGemManager : IService
{
    public List<GameObject> AvailableGems { get; }
    public int GemCountAtBase { get; }
    public void EnemySnatchesGem(Vector3 fromPos, EnemyCharacter enemyCharacter);

    public event Action AllGemsLost;
}