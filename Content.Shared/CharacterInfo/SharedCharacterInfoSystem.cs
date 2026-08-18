using Content.Shared._Stalker.Characteristics;
using Content.Shared.Objectives;
using Robust.Shared.Serialization;

namespace Content.Shared.CharacterInfo;

[Serializable, NetSerializable]
public sealed class RequestCharacterInfoEvent : EntityEventArgs
{
    public readonly NetEntity NetEntity;

    public RequestCharacterInfoEvent(NetEntity netEntity)
    {
        NetEntity = netEntity;
    }
}

[Serializable, NetSerializable]
public sealed class CharacterInfoEvent : EntityEventArgs
{
    public readonly NetEntity NetEntity;
    public readonly string JobTitle;
    public readonly Dictionary<string, List<ObjectiveInfo>> Objectives;
    public readonly string? Briefing;
    public readonly Dictionary<CharacteristicType, Characteristic>? Characteristics; // stalker-changes

    public CharacterInfoEvent(
        NetEntity netEntity,
        string jobTitle,
        Dictionary<string, List<ObjectiveInfo>> objectives,
        Dictionary<CharacteristicType, Characteristic>? characteristicsByType, // stalker-changes
        string? briefing)
    {
        NetEntity = netEntity;
        JobTitle = jobTitle;
        Objectives = objectives;
        Briefing = briefing;
        Characteristics = characteristicsByType; // stalker-changes
    }
}
