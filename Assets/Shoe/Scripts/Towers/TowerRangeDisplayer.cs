using UnityEngine;

public class TowerRangeDisplayer : MonoBehaviour
{
    [SerializeField] private GameObject towerRangeDisplayPrefab;
    [SerializeField] private GameObject towerUpgradedRangeDisplayPrefab;
    [SerializeField] private GameObject towerPreviousRangeDisplayPrefab;

    private GameObject towerRangeDisplayerObject;
    private GameObject towerUpgradedRangeDisplayerObject;
    private GameObject towerPreviousRangeDisplayerObject;
    private TowerMenuUI towerMenuUI;

    public void Initialize(TowerMenuUI towerMenuUI)
    {
        this.towerMenuUI = towerMenuUI;
        towerRangeDisplayerObject = Instantiate(towerRangeDisplayPrefab, null);
        towerRangeDisplayerObject.SetActive(false);

        towerUpgradedRangeDisplayerObject = Instantiate(towerUpgradedRangeDisplayPrefab, null);
        towerUpgradedRangeDisplayerObject.SetActive(false);

        towerPreviousRangeDisplayerObject = Instantiate(towerPreviousRangeDisplayPrefab, null);
        towerPreviousRangeDisplayerObject.SetActive(false);
    }

    public void DisplayTowerRange(TowerData towerData)
    {
        if (towerMenuUI.Upgrading)
        {
            towerUpgradedRangeDisplayerObject.SetActive(true);
            towerPreviousRangeDisplayerObject.SetActive(true);

            towerUpgradedRangeDisplayerObject.transform.position = towerMenuUI.SelectedTowerPlate.transform.position + towerMenuUI.SelectedTowerPlate.GetBasePosition;
            towerPreviousRangeDisplayerObject.transform.position = towerMenuUI.SelectedTowerPlate.transform.position + towerMenuUI.SelectedTowerPlate.GetBasePosition;

            towerUpgradedRangeDisplayerObject.transform.localScale = RangeConversion(towerData, towerMenuUI.SelectedTowerPlate.PlacedTower.TowerLevel + 1);
            towerPreviousRangeDisplayerObject.transform.localScale = RangeConversion(towerData, towerMenuUI.SelectedTowerPlate.PlacedTower.TowerLevel);
        }
        else
        {
            towerRangeDisplayerObject.SetActive(true);

            towerRangeDisplayerObject.transform.position = towerMenuUI.SelectedTowerPlate.transform.position + towerMenuUI.SelectedTowerPlate.GetBasePosition;
            towerRangeDisplayerObject.transform.localScale = RangeConversion(towerData, 0);
        }
    }

    public void HideTowerRange()
    {
        towerRangeDisplayerObject.SetActive(false);
        towerUpgradedRangeDisplayerObject.SetActive(false);
        towerPreviousRangeDisplayerObject.SetActive(false);
    }

    Vector3 RangeConversion(TowerData towerData, int towerLevel)
    {
        return new Vector3(
    towerData.GetTowerSpecs(towerLevel).SightRadius * 2,
    towerData.GetTowerSpecs(towerLevel).SightRadius * 2,
    towerData.GetTowerSpecs(towerLevel).SightRadius * 2);
    }
}
