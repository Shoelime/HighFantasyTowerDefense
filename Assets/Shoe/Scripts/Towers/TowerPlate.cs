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

    private TowerAI placedTower;
    private bool plateSelected;
    private AudioSource audioSource;

    public Vector3 GetBasePosition => towerBasePosition;

    public static event Action<TowerPlate> PlateSelected;
    public static event Action<TowerPlate> PlateUnSelected;

    public bool ContainsTower()
    {
        return placedTower != null;
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
            placedTower = Instantiate(towerToBuild.WorldPrefab, transform.position + towerBasePosition, transform.rotation, transform).GetComponent<TowerAI>();
            placedTower.BuildTower();
            success = true;

            SoundManager.Instance.PlaySound(audioSource, buildTowerAudio, 1);
        }
        else
        {
            success = false;
            SoundManager.Instance.PlaySound(audioSource, buildErrorAudio, 1);
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

    private void OnDisable()
    {
        Services.Get<IInputManager>().OnLeftMouseButton -= ButtonPressed;
    }
}