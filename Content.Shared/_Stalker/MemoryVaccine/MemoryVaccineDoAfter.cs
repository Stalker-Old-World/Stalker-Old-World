using Content.Shared.DoAfter;
using Robust.Shared.Serialization;

namespace Content.Shared.MemoryVaccine
{

    [Serializable, NetSerializable]
    public sealed partial class MemoryVaccineDoAfterEvent : SimpleDoAfterEvent { }

}
