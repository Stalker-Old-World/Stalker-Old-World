using Content.Shared._Stalker.ClownScream;
using Content.Shared.Inventory;
using Content.Shared.Inventory.Events;
using Content.Shared.Mobs;
using Content.Shared.Mobs.Systems;

namespace Content.Server._Stalker.ClownScream;

public sealed class ClownScreamSystem : SharedClownScreamSystem
{
    [Dependency] private readonly InventorySystem _inventory = default!;
    [Dependency] private readonly MobStateSystem _mobState = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ClownScreamComponent, ComponentStartup>(OnStartup);
        SubscribeLocalEvent<ClownScreamComponent, DidUnequipEvent>(OnUnequipped);
        SubscribeLocalEvent<ClownScreamComponent, DidEquipEvent>(OnEquipped);
        SubscribeLocalEvent<ClownScreamComponent, MobStateChangedEvent>(OnMobState);
        // SubscribeLocalEvent<ClownScreamComponent, StrippableDoAfterEvent>(OnStripped);
    }
    private void OnStartup(Entity<ClownScreamComponent> entity, ref ComponentStartup args)
    {
        if (_mobState.IsIncapacitated(entity))
            return;

        if (!_inventory.TryGetSlotEntity(entity, entity.Comp.Slot, out _))
        {
            ToggleScream(entity, true);
            return;
        }
        ToggleScream(entity, false);
    }

    private void OnMobState(Entity<ClownScreamComponent> entity, ref MobStateChangedEvent args)
    {
        if (!_mobState.IsIncapacitated(entity))
            return;

        if (entity.Comp.SoundEntity == null)
            return;

        ToggleScream(entity, false);
        StopSound(entity);
    }

    private void OnUnequipped(Entity<ClownScreamComponent> entity, ref DidUnequipEvent args)
    {
        if (_mobState.IsIncapacitated(entity))
            return;

        if (args.Slot != entity.Comp.Slot)
            return;

        ToggleScream(entity, true);
        PlaySound(entity);
    }
    private void OnEquipped(Entity<ClownScreamComponent> entity, ref DidEquipEvent args)
    {
        if (_mobState.IsIncapacitated(entity))
            return;

        if (args.Slot != entity.Comp.Slot)
            return;

        ToggleScream(entity, false);
        StopSound(entity);
    }

    // private void OnStripped(Entity<ClownScreamComponent> entity, ref StrippableDoAfterEvent args)
    // {
    //     if (args.SlotOrHandName != "mask")
    //         return;
    //
    //     if (args.InsertOrRemove)
    //     {
    //         ToggleScream(entity, false);
    //         StopSound(entity);
    //     }
    //     else
    //     {
    //         ToggleScream(entity, true);
    //         PlaySound(entity);
    //     }
    // }

    private void ToggleScream(Entity<ClownScreamComponent> entity, bool enable)
    {
        var ev = new ToggleClownScreamMessage(GetNetEntity(entity), entity.Comp.Sprite, enable);
        RaiseNetworkEvent(ev);
    }
}
