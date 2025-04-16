using System;
using System.Collections.Generic;

public static class StatusEffectFactory
{
    private static readonly Dictionary<string, Func<StatusEffectData, IStatusEffect>> effectConstructors = new()
    {
        { "Burn", data => new BurningEffect(data) },
        { "Freeze", data => new FrozenEffect(data) },
        { "Stun", data => new StunnedEffect(data) }
    };

    public static IStatusEffect Create(string effectId, StatusEffectData data)
    {
        return effectConstructors.TryGetValue(effectId, out var constructor)
            ? constructor(data)
            : null;
    }
}
