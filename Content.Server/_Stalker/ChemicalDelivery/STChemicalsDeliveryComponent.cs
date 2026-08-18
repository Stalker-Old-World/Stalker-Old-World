using Robust.Shared.Audio;
using Content.Shared.StepTrigger.Components;
using Content.Shared.Storage;
using Robust.Shared.Prototypes;
using Content.Shared.Chemistry.Reagent;

namespace Content.Server._Stalker.ChemicalDelivery;

/// <summary>
/// Injects selected chemical to the intity on touch
/// !!! Prototypes required <seealso cref="StepTriggerComponent"/> to work !!!
/// <seealso cref="STChemicalsDeliverySystem"/>
/// </summary>
[RegisterComponent, Access(typeof(STChemicalsDeliverySystem))]
public sealed partial class STChemicalsDeliveryComponent : Component
{
    /// <summary>
    /// Sound played if someone steps into entity
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public SoundSpecifier? DeliverSound = default;

    /// <summary>
    /// Reagent which will be injected from the toxic waste. Fluorine gives Caustic and poison damage. Be creative!
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public ProtoId<ReagentPrototype> Reagent = default;

    /// <summary>
    /// Reagent amount that will be injected after stepping in.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float Amount = 0.5f;

    /// <summary>
    /// Effect that you want to spawn on entry
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public EntitySpawnEntry? Entry = default!;
}
