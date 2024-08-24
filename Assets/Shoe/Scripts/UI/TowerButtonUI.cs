using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerButtonUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] TextMeshProUGUI towerNameText;
    [SerializeField] TextMeshProUGUI towerCostText;
    [SerializeField] float originalFontSize;
    [SerializeField] float fontSizeWhenUpgrading;

    public TowerData TowerData { get; private set; }
    public event Action<TowerData> ButtonSelected;
    public event Action ButtonUnselected;

    public void SetTowerData(TowerData towerData)
    {
        TowerData = towerData;
        SetTitleText(towerData.TowerType.ToString(), false);
        SetGoldText(towerData.GoldCostToBuild.ToString());
    }

    public void SetTitleText(string v, bool upgrading)
    {
        if (upgrading)
            towerNameText.fontSize = fontSizeWhenUpgrading;
        else towerNameText.fontSize = originalFontSize;

        towerNameText.text = v;
    }

    public void SetGoldText(string v)
    {
        towerCostText.text = v;
    }

    public void SetCostTextColor(bool tryingToUpgrade, TowerPlate towerPlate)
    {
        bool canAfford = tryingToUpgrade ?
            Services.Get<IEconomicsManager>().GetCurrentGoldAmount() >= towerPlate.PlacedTower.GetTowerSpecs().GoldCostToUpgrade :
            Services.Get<IEconomicsManager>().GetCurrentGoldAmount() >= TowerData.GoldCostToBuild;

        if (tryingToUpgrade)
        {
            SetTitleText("Upgrade", true);
        }
        else
        {
            SetTitleText(TowerData.TowerType.ToString(), false);
        }

        if (canAfford)
            towerCostText.color = Color.black;
        else towerCostText.color = Color.red;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ButtonSelected?.Invoke(TowerData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ButtonUnselected?.Invoke();
    }
}
