using System;
using System.Collections;
using UnityEngine;

public class TowerPlate : MonoBehaviour
{
    [SerializeField] private Vector3 towerBasePosition;

    private TowerAI placedTower;
    private bool plateSelected;

    public static event Action<TowerPlate> PlateSelected;
    public static event Action<TowerPlate> PlateUnSelected;

    public bool ContainsTower()
    {
        return placedTower != null;
    }

    void Start()
    {
        Services.Get<IInputManager>().OnLeftMouseButton += ButtonPressed;
    }

    public void BuildTower(TowerData towerToBuild, out bool success)
    {
        if (Services.Get<IEconomicsManager>().AttemptToPurchase(towerToBuild.GoldCostToBuild))
        {
            placedTower = Instantiate(towerToBuild.WorldPrefab, transform.position + towerBasePosition, transform.rotation, transform).GetComponent<TowerAI>();
            placedTower.BuildTower();
            success = true;
        }
        else
        {
            success = false;
            Debug.Log("can't afford to build tower!");
        }
    }

    private void ButtonPressed(Vector3 clickPosition, GameObject clickedObject)
    {
        if (clickedObject == this.gameObject)
        {
            PlateSelected?.Invoke(this);
            plateSelected = true;
        }
        else if (plateSelected)
        {
            plateSelected = false;
            PlateUnSelected?.Invoke(this);
        }
    }
}