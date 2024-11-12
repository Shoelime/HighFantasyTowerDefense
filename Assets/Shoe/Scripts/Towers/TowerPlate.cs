using System;
using UnityEngine;

public class TowerPlate : MonoBehaviour
{
    [Header("Where to instantiate tower relative to plate")]
    [SerializeField] private Vector3 towerBasePosition;

    [Header("Autobuild a tower, for testing purposes")]
    [SerializeField] private TowerData autoPlaceTower;
    [Header("Audio")]
    [SerializeField] private SoundData buildTowerAudio;
    [SerializeField] private SoundData buildErrorAudio;

    public TowerAI PlacedTower { get; private set; }
    public bool plateSelected { get; private set; }
    private AudioSource audioSource;
    public Vector3 GetBasePosition => towerBasePosition;

    public static event Action<TowerPlate> PlateSelected;
    public static event Action<TowerPlate> PlateUnSelected;

    public bool ContainsTower()
    {
        return PlacedTower != null;
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        Services.Get<IInputManager>().OnLeftMouseButton += ButtonPressed;

        // For testing purposes if we want to start with a tower, spawn one here
        if (autoPlaceTower != null)
        {
            Services.Get<IEconomicsManager>().AddGold(autoPlaceTower.GoldCostToBuild);
            BuildTower(autoPlaceTower, out bool success);
        }
    }

    public void BuildTower(TowerData towerToBuild, out bool success)
    {
        if (Services.Get<IEconomicsManager>().AttemptToPurchase(towerToBuild.GoldCostToBuild))
        {
            PlacedTower = Instantiate(towerToBuild.WorldPrefab, transform.position + towerBasePosition, transform.rotation, transform).GetComponent<TowerAI>();
            PlacedTower.BuildTower(this);
            success = true;

            Services.Get<ISoundManager>().PlaySound(audioSource, buildTowerAudio, 1);
        }
        else
        {
            success = false;
            Services.Get<ISoundManager>().PlaySound(audioSource, buildErrorAudio, 1);
        }
    }

    public void UpgradeTower(out bool success)
    {
        if (Services.Get<IEconomicsManager>().AttemptToPurchase(PlacedTower.GetTowerSpecs().GoldCostToUpgrade))
        {
            success = true;

            PlacedTower.UpgradeTower();
            Services.Get<ISoundManager>().PlaySound(audioSource, buildTowerAudio, 1);
        }
        else
        {
            success = false;
            Services.Get<ISoundManager>().PlaySound(audioSource, buildErrorAudio, 1);
        }
    }

    private void ButtonPressed(Vector3 clickPosition, GameObject clickedObject)
    {
        if (clickedObject == this.gameObject)
        {
            SelectPlate(true);

        }
        else if (plateSelected)
        {
            SelectPlate(false);
        }
    }

    public void SelectPlate(bool value)
    {
        if (value)
        {
            PlateSelected?.Invoke(this);
            plateSelected = true;
        }
        else
        {
            plateSelected = false;
            PlateUnSelected?.Invoke(this);
        }
    }

    private void OnDisable()
    {
        Services.Get<IInputManager>().OnLeftMouseButton -= ButtonPressed;
    }
}