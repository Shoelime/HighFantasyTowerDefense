using System.Collections;
using UnityEngine;

public class BurningEffect : IStatusEffect
{
    public StatusEffectData EffectData { get; set; }

    public BurningEffect(StatusEffectData data)
    {
        EffectData = data;
    }

    public IEnumerator Tick(Health target)
    {
        float timer = EffectData.duration;

        while (timer > 0)
        {
            yield return new WaitForSeconds(1f);
            timer -= 1;
            target.ReduceHealth(EffectData.damagePerSecond);
        }

        target.RemoveEffect(this);
    }
}