using System.Diagnostics.CodeAnalysis;
using Content.Shared.Clothing.Components;
using Content.Shared.DoAfter;
using Content.Shared.Hands;
using Content.Shared.Hands.Components;
using Content.Shared.Hands.EntitySystems;
using Content.Shared.Interaction;
using Content.Shared.Inventory.Events;
using Content.Shared.Item;
using Content.Shared.Movement.Systems;
using Content.Shared.Popups;
using Content.Shared.Strip.Components;
using Content.Shared.Tag;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Containers;
using Robust.Shared.Serialization;
using Robust.Shared.Timing;
using Robust.Shared.Utility;

namespace Content.Shared.Inventory;

// Stalker-Changes-Start
public abstract partial class InventorySystem
{
    private readonly Dictionary<string, string> _slotsByConsealers = new Dictionary<string, string>()
    {
        { "cloak", "outerClothing" },
        { "head", "ears" }
    };

    private void HideSlotsOnConcealerEquip(EntityUid uid, InventoryComponent component, string consealerId)
    {
        if (!_slotsByConsealers.TryGetValue(consealerId, out string? slotToHide))
            return;

        if (!TryGetSlot(uid, slotToHide, out var slotDef, inventory: component))
            return;
        slotDef.StripHidden = true;
    }

    private void HideSlotsOnConcealerUnequip(EntityUid uid, InventoryComponent component, string consealerId)
    {
        if (!_slotsByConsealers.TryGetValue(consealerId, out string? slotToHide))
            return;


        if (!TryGetSlot(uid, slotToHide, out var slotDef, inventory: component))
            return;
        slotDef.StripHidden = false;
    }
}

// Stalker-Changes-Ends
