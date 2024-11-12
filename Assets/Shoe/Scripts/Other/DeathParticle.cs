using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathParticle : PooledMonoBehaviour
{
    [SerializeField] private ParticleSystem myParticleSystem;
    private void OnEnable()
    {
        ReturnToPool(myParticleSystem.main.duration);

        OnReturnToPool += (PooledMonoBehaviour) => myParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }
}
