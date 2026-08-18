using Content.Shared._Stalker_EN.PvpZone;
using Content.Shared.Random.Rules;

namespace Content.Shared._Stalker_EN.Rules;

/// <summary>
/// Returns false if the player is inside a green zone.
/// </summary>
public sealed partial class PreventInGreenzoneRule : RulesRule
{
    public override bool Check(EntityManager entManager, EntityUid uid)
    {
        if (!entManager.TryGetComponent<STPlayerZoneComponent>(uid, out var comp))
            return !Inverted;

        if (comp.CurrentZone != STPvpZoneType.Green && comp.CurrentZone != STPvpZoneType.Faction)
            return !Inverted;

        return Inverted;
    }
}
