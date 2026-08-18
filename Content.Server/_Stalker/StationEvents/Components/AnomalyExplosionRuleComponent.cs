using Content.Server._Stalker.StationEvents.Events;
using Content.Shared.Damage;
using Robust.Shared.Audio;

namespace Content.Server._Stalker.StationEvents.Components;

[RegisterComponent, Access(typeof(AnomalyExplosionRule))]
public sealed partial class AnomalyExplosionRuleComponent : Component
{
    [DataField]
    public SoundSpecifier? SoundStart = new SoundPathSpecifier("/Audio/_Stalker/blowout.ogg");

    [DataField]
    public SoundSpecifier? SoundEnd = new SoundPathSpecifier("/Audio/_Stalker/blowout_end.ogg");

    [DataField]
    public DamageSpecifier? Damage;

    [DataField]
    public TimeSpan DamageNext;

    [DataField]
    public TimeSpan DamageNextDelay = TimeSpan.FromSeconds(3f);

    [DataField]
    public TimeSpan DamageStarts;

    [DataField]
    public TimeSpan DamageStartsDelay = TimeSpan.FromMinutes(1.5f);

    [DataField]
    public float ShakeStrength = 50f;

    [DataField]
    public Color ScreenColor = Color.FromHex("#FF0000FF");
}

