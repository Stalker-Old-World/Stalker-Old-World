using Content.Shared.Random.Rules;

namespace Content.Shared._Stalker_EN.Rules;

/// <summary>
/// Checks if the grid being stood on is at daytime or night time.
/// </summary>
public sealed partial class IsOnSpecificMapRule : RulesRule
{
    [DataField]
    public List<string>? Maps;
    public override bool Check(EntityManager entManager, EntityUid uid)
    {
        if (Maps is null ||
            !entManager.TryGetComponent<TransformComponent>(uid, out var transform) ||
            !entManager.TryGetComponent<MetaDataComponent>(transform.MapUid, out var meta) ||
            !Maps.Contains(meta.EntityName))
            return false;
        return true;
    }
}
