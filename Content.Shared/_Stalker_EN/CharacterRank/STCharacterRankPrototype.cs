using Content.Shared.StatusIcon;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.Manager.Attributes;

namespace Content.Shared._Stalker_EN.CharacterRank;

/// <summary>
/// Defines the rank tiers and time thresholds for character progression.
/// </summary>
[Prototype("stCharacterRank")]
public sealed partial class STCharacterRankPrototype : IPrototype
{
    [IdDataField]
    public string ID { get; private set; } = default!;

    /// <summary>
    /// Ordered list of rank definitions. Must be sorted by TimeRequired ascending.
    /// </summary>
    [DataField(required: true)]
    public List<STRankDefinition> Ranks { get; private set; } = new();
}

/// <summary>
/// A single rank level with its time threshold, icon, and display name.
/// </summary>
[DataDefinition]
public sealed partial class STRankDefinition
{
    /// <summary>
    /// Zero-based index of this rank (0-7).
    /// </summary>
    [DataField(required: true)]
    public int Index { get; set; }

    /// <summary>
    /// Cumulative playtime required to achieve this rank.
    /// </summary>
    [DataField(required: true)]
    public TimeSpan TimeRequired { get; set; }

    /// <summary>
    /// The status icon prototype to display for this rank.
    /// </summary>
    [DataField(required: true)]
    public ProtoId<JobIconPrototype> IconId { get; set; }

    /// <summary>
    /// Localization key for the rank's display name.
    /// </summary>
    [DataField(required: true)]
    public LocId Name { get; set; }
}
