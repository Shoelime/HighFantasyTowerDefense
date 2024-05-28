public class StunnedEffect : IStatusEffect
{
    public float Duration { get; private set; }

    public StunnedEffect(float duration)
    {
        Duration = duration;
    }

    public void Apply(IHealth target)
    {
        if (target is Health healthComponent)
        {
            healthComponent.ApplyStun(Duration);
        }
    }

    public void Remove(IHealth target)
    {
        throw new System.NotImplementedException();
    }
}
