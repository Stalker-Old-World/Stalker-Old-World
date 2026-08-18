using Robust.Shared.Prototypes;

namespace Content.Shared._Stalker.Physique;

public sealed class PhysiqueSystem : EntitySystem
{
    [Dependency] private readonly IPrototypeManager _prototype = default!;

    public override void Initialize()
    {
        base.Initialize();

        foreach (var prototype in _prototype.EnumeratePrototypes<PhysiquePrototype>())
        {
            Log.Debug(prototype.ID);
        }
    }

    public HashSet<PhysiquePrototype> GetSelectablePrototypes()
    {
        var prototypes = new HashSet<PhysiquePrototype>();
        foreach (var prototype in _prototype.EnumeratePrototypes<PhysiquePrototype>())
        {
            if (!prototype.Selectable)
                continue;

            prototypes.Add(prototype);
        }

        return prototypes;
    }
}
