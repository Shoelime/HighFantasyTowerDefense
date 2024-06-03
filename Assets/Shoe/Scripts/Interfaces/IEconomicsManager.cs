using System;

public interface IEconomicsManager : IService
{
    public int GetCurrentGoldAmount();
    public bool AttemptToPurchase(int cost);
    public void AddGold(int gold);

    event Action<int> GoldAmountChanged;
}
