using System.Collections;

public interface IStatusEffect
{
    public StatusEffectData EffectData { get; set; }
    IEnumerator Tick(Health target);
}