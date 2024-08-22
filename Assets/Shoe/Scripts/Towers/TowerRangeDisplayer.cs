using UnityEngine;

public class TowerRangeDisplayer : MonoBehaviour
{
    [SerializeField] private GameObject towerRangeDisplayPrefab;

    private GameObject towerRangeDisplayerObject;
    private TowerMenuUI towerMenuUI;

    public void Initialize(TowerMenuUI towerMenuUI)
    {
        this.towerMenuUI = towerMenuUI;
        towerRangeDisplayerObject = Instantiate(towerRangeDisplayPrefab);
        towerRangeDisplayerObject.SetActive(false);
    }

    public void DisplayTowerRange(TowerData towerData)
    {
        towerRangeDisplayerObject.SetActive(true);

        towerRangeDisplayerObject.transform.position = towerMenuUI.SelectedTowerPlate.transform.position + towerMenuUI.SelectedTowerPlate.GetBasePosition;
        Vector3 rangeConversion = new(towerData.SightRadius * 2, towerData.SightRadius * 2, towerData.SightRadius * 2);
        
        towerRangeDisplayerObject.transform.localScale = rangeConversion;
    }

    public void HideTowerRange()
    {
        towerRangeDisplayerObject.SetActive(false);
    }
}
