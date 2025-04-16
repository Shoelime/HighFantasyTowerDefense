public interface IHealth
{
    void TakeDamage(DamageData damageData);
    void ApplyEffect(IStatusEffect effect);
}
