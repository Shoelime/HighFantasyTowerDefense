using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Shoe/TowerData")]
public class TowerData : ScriptableObject
{
    [SerializeField] private TowerType towerType = default;
    [SerializeField] private int goldCostToBuild = 0;
    [SerializeField] private int goldRefundedOnDisassembly = 0;
    [SerializeField] private float constructDuration = 0.0f;
    [SerializeField] private GameObject worldPrefab;
    [SerializeField] private LayerMask lineOfSightLayers;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private Projectile projectileToShoot;
    [SerializeField] private TowerUpgradableVariables[] upgradeableVariables;
    public TowerType TowerType { get { return towerType; } }
    public int GoldCostToBuild { get { return goldCostToBuild; } }
    public int GoldRefundedOnDisassembly { get { return goldRefundedOnDisassembly; } }
    public float ConstructDuration { get { return constructDuration; } }
    public GameObject WorldPrefab { get { return worldPrefab; } }
    public LayerMask LineOfSightLayers { get { return lineOfSightLayers; } }
    public LayerMask EnemyLayer { get { return enemyLayer; } }
    public Projectile ProjectileToShoot { get { return projectileToShoot; } }
    public TowerUpgradableVariables GetTowerSpecs(int index) { return upgradeableVariables[index]; }
}

public enum TowerType
{
    Earth,
    Fire,
    Water,
    Air
}

[Serializable]
public struct DamageData
{
    public int DamagePerHit;
    public int DamagePerSecond;
    public int DamagePerSecondDuration;
    public float DamageRadius;
    public List<StatusEffectData> StatusEffects;
}

[Serializable]
public struct TowerUpgradableVariables
{
    public float SightRadius;
    public float FireRate;
    public float ProjectileSpeed;
    public int GoldCostToUpgrade;
    public DamageData DamageData;

}