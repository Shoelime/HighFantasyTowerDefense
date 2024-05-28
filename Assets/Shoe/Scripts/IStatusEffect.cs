public interface IStatusEffect
{
    void Apply(IHealth target);
    void Remove(IHealth target);
}