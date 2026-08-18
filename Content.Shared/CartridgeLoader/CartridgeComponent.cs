using Robust.Shared.GameStates;
using Robust.Shared.Serialization;
using Robust.Shared.Utility;

namespace Content.Shared.CartridgeLoader;

/// <summary>
/// This is used for defining values used for displaying in the program ui in yaml
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class CartridgeComponent : Component
{
    [DataField]
    public EntityUid? LoaderUid;

    [DataField(required: true)]
    public LocId ProgramName = "default-program-name";

    [DataField]
    public SpriteSpecifier? Icon;

    [AutoNetworkedField]
    public InstallationStatus InstallationStatus = InstallationStatus.Cartridge;

    /// <summary>Whether to show a notification indicator on the program list.</summary>
    [DataField, AutoNetworkedField]
    public bool HasNotification; // stalker-changes
}

[Serializable, NetSerializable]
public enum InstallationStatus
{
    Cartridge,
    Installed,
    Readonly
}
