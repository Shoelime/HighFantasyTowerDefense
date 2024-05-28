public class FrozenEffect : IStatusEffect
{
    public float Duration { get; private set; }

    public FrozenEffect(float duration)
    {
        Duration = duration;
    }

    public void Apply(IHealth target)
    {
        if (target is Health healthComponent)
        {
            healthComponent.ApplyFreeze(Duration);
        }
    }

    public void Remove(IHealth target)
    {
        throw new System.NotImplementedException();
    }
}
