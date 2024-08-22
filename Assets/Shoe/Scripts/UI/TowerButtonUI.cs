using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerButtonUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] TextMeshProUGUI towerNameText;
    [SerializeField] TextMeshProUGUI towerCostText;

    public TowerData TowerData { get; private set; }
    public event Action<TowerData> ButtonSelected;
    public event Action ButtonUnselected;

    public void SetTowerData(TowerData towerData)
    {
        TowerData = towerData;
        SetTitleText(towerData.TowerType.ToString());
        SetGoldText(towerData.GoldCostToBuild.ToString());
    }

    public void SetTitleText(string v)
    {
        towerNameText.text = v;
    }

    public void SetGoldText(string v)
    {
        towerCostText.text = v;
    }

    public void SetCostTextColor()
    {
        if (Services.Get<IEconomicsManager>().GetCurrentGoldAmount() >= TowerData.GoldCostToBuild)
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
