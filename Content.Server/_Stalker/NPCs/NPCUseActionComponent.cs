using Content.Server.NPC.Systems;
using Content.Shared.Actions;
using Robust.Shared.Prototypes;

namespace Content.Server._Stalker.NPCs;

/// <summary>
/// This is used for an NPC that constantly tries to use an action. Different from NPCUseActionOnTargetComponent
/// Our implementation is more advanced and supports more action events
/// </summary>
[RegisterComponent, Access(typeof(NPCUseActionSystem))]
public sealed partial class NPCUseActionComponent : Component
{
    /// <summary>
    /// HTN blackboard key for the target entity
    /// </summary>
    [DataField]
    public string TargetKey = "Target";

    /// <summary>
    /// Action that's going to attempt to be used.
    /// </summary>
    [DataField(required: true)]
    public EntProtoId ActionId;

    [DataField]
    public EntityUid? ActionEnt;
}
