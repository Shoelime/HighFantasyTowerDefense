using System;

public interface IEconomicsManager : IService
{
    public int GetCurrentGoldAmount();
    public bool AttemptToPurchase(int cost);

    event Action<int> GoldAmountChanged;
}
