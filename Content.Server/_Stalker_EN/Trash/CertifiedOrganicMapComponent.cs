namespace Content.Server._Stalker_EN.Trash;

/// <summary>
///     Added to maps loaded by the Stalker maploader to signify they should be
///         paused/unpaused by TrashDeletingSystem.
///
///     Maps that dont have this wont be affected by it
/// </summary>
[RegisterComponent]
[UnsavedComponent] // dont map this ffs
public sealed partial class CertifiedOrganicMapComponent : Component;
