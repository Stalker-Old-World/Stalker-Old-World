using Robust.Shared.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Content.Shared._Stalker.Psyonics;

[Serializable, NetSerializable]
public sealed class PsyEnergyChangedMessage : EntityEventArgs
{
    public readonly int MaxPsy = 0;
    public readonly int OldPsy = 0;
    public readonly int NewPsy = 0;

    public PsyEnergyChangedMessage(int maxPsy, int oldPsy, int newPsy)
    {
        MaxPsy = maxPsy;
        OldPsy = oldPsy;
        NewPsy = newPsy;
    }
}
