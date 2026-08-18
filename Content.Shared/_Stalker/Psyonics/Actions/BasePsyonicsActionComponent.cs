using Robust.Shared.Prototypes;

namespace Content.Shared._Stalker.Psyonics.Actions;

public abstract partial class BasePsyonicsActionComponent : Component
{
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public virtual EntProtoId ActionId { get; set; } = string.Empty;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public virtual int Cost { get; set; }

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public virtual bool Consumable { get; set; } = true;
}
