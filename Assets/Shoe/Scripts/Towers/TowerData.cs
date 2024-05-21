using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Shoe/TowerData")]
public class TowerData : ScriptableObject
{
    [SerializeField] private TowerType towerType = default;
    [SerializeField] private float sightRadius = 0.0f;
    [SerializeField] private float fireRate = 0.0f;
    [SerializeField] private float projectileSpeed = 0.0f;
    [SerializeField] private int goldCostToBuild = 0;
    [SerializeField] private int goldRefundedOnDisassembly = 0;
    [SerializeField] private float constructDuration = 0.0f;
    [SerializeField] private GameObject worldPrefab;
    [SerializeField] private LayerMask lineOfSightLayers;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private Projectile projectileToShoot;
    [SerializeField] private DamageData damageData;
    public TowerType TowerType { get { return towerType; } }
    public float SightRadius { get { return sightRadius; } }
    public float FireRate { get { return fireRate; } }
    public float ProjectileSpeed { get { return projectileSpeed; } }
    public int GoldCostToBuild { get { return goldCostToBuild; } }
    public int GoldRefundedOnDisassembly { get { return goldRefundedOnDisassembly; } }
    public float ConstructDuration { get { return constructDuration; } }
    public GameObject WorldPrefab { get { return worldPrefab; } }
    public LayerMask LineOfSightLayers { get { return lineOfSightLayers; } }
    public LayerMask EnemyLayer { get { return enemyLayer; } }
    public Projectile ProjectileToShoot { get { return projectileToShoot; } }
    public DamageData DamageData { get { return damageData; } }
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