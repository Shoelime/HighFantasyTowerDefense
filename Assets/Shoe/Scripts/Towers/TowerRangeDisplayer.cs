using UnityEngine;

public class TowerRangeDisplayer : MonoBehaviour
{
    [SerializeField] private GameObject towerRangePrefab;
    [SerializeField] private GameObject towerUpgradedRangePrefab;
    [SerializeField] private GameObject towerPreviousRangeDisplayPrefab;

    private GameObject towerRangeDisplayer;
    private GameObject towerUpgradedRangeDisplayer;
    private GameObject towerPreviousRangeDisplayer;
    private TowerMenuUI towerMenuUI;

    public void Initialize(TowerMenuUI towerMenuUI)
    {
        this.towerMenuUI = towerMenuUI;
        towerRangeDisplayer = Instantiate(towerRangePrefab, null);
        towerRangeDisplayer.SetActive(false);

        towerUpgradedRangeDisplayer = Instantiate(towerUpgradedRangePrefab, null);
        towerUpgradedRangeDisplayer.SetActive(false);

        towerPreviousRangeDisplayer = Instantiate(towerPreviousRangeDisplayPrefab, null);
        towerPreviousRangeDisplayer.SetActive(false);
    }

    public void DisplayTowerRange(TowerData towerData)
    {
        if (towerMenuUI.Upgrading)
        {
            towerUpgradedRangeDisplayer.SetActive(true);
            towerPreviousRangeDisplayer.SetActive(true);

            int towerLevel = towerMenuUI.SelectedTowerPlate.PlacedTower.TowerLevel;

            towerUpgradedRangeDisplayer.transform.position = towerMenuUI.SelectedTowerPlate.transform.position + towerMenuUI.SelectedTowerPlate.GetBasePosition;
            towerPreviousRangeDisplayer.transform.position = towerMenuUI.SelectedTowerPlate.transform.position + towerMenuUI.SelectedTowerPlate.GetBasePosition;

            towerUpgradedRangeDisplayer.transform.localScale = RangeConversion(towerData, towerLevel + 1);
            towerPreviousRangeDisplayer.transform.localScale = RangeConversion(towerData, towerLevel);

        }
        else
        {
            towerRangeDisplayer.SetActive(true);

            towerRangeDisplayer.transform.position = towerMenuUI.SelectedTowerPlate.transform.position + towerMenuUI.SelectedTowerPlate.GetBasePosition;
            towerRangeDisplayer.transform.localScale = RangeConversion(towerData, 0);
        }
    }

    public void HideTowerRange()
    {
        towerRangeDisplayer.SetActive(false);
        towerUpgradedRangeDisplayer.SetActive(false);
        towerPreviousRangeDisplayer.SetActive(false);
    }

    Vector3 RangeConversion(TowerData towerData, int towerLevel)
    {
        return new Vector3(
    towerData.GetTowerSpecs(towerLevel).SightRadius * 2,
    towerData.GetTowerSpecs(towerLevel).SightRadius * 2,
    towerData.GetTowerSpecs(towerLevel).SightRadius * 2);
    }
}
