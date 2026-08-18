using Content.Shared.DoAfter;
using Robust.Shared.Serialization;

namespace Content.Shared._Stalker.Lopata;

[Serializable, NetSerializable]
public sealed partial class PlowGroundDoAfterEvent : SimpleDoAfterEvent
{
    public string NameProrotype = "";

    public PlowGroundDoAfterEvent(string nameProrotype)
    {
        NameProrotype = nameProrotype;
    }
}
