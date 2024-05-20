using System.Collections.Generic;
using UnityEngine;

public interface IGemManager : IService
{
    public List<GameObject> AvailableGems { get; }
    public void EnemySnatchesGem(Vector3 fromPos, EnemyCharacter enemyCharacter);
}