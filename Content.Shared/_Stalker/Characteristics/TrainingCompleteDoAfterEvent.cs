using Content.Shared.DoAfter;
using Robust.Shared.Serialization;
namespace Content.Shared._Stalker.Characteristics;

[Serializable, NetSerializable]
public sealed partial class TrainingCompleteDoAfterEvent : SimpleDoAfterEvent
{
}
