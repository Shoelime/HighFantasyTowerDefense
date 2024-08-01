using System;
using UnityEngine;

public class EconomicManager : IEconomicsManager, IDisposable
{
    public int CurrentGold { get; private set; }

    public event Action<int> GoldAmountChanged;

    public void Initialize()
    {
        AddGold(Services.Get<IGameManager>().GetLevelData.StartingGold);
        EnemyCharacter.EnemyDied += (enemyData) => AddGold(enemyData.GoldCarryCount);       
    }

    public bool AttemptToPurchase(int cost)
    {
        if (CurrentGold < cost)
        {
            Debug.Log("Can't afford a tower");
            return false;
        }
        else
        {
            CurrentGold -= cost;
            GoldAmountChanged?.Invoke(CurrentGold);
            return true;
        }
    }

    public int GetCurrentGoldAmount()
    {
        return CurrentGold;
    }

    public void AddGold(int gold)
    {
        CurrentGold += gold;
        GoldAmountChanged?.Invoke(CurrentGold);
    }

    public void Dispose()
    {
        EnemyCharacter.EnemyDied -= (enemyData) => AddGold(enemyData.GoldCarryCount);
    }
}