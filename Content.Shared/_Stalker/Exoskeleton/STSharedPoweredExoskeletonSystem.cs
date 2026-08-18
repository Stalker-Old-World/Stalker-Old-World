using Content.Shared.Actions;
using Content.Shared.Inventory.Events;

namespace Content.Shared._Stalker.Exoskeleton;

public abstract class STSharedPoweredExoskeletonSystem : EntitySystem
{
    [Dependency] private readonly SharedActionsSystem _actions = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<STPoweredExoskeletonComponent, GotEquippedEvent>(OnEquip);
        SubscribeLocalEvent<STPoweredExoskeletonComponent, GotUnequippedEvent>(OnUnequip);
    }

    private void OnEquip(EntityUid uid, STPoweredExoskeletonComponent exoskeleton, GotEquippedEvent args)
    {
        _actions.AddAction(args.Equipee, ref exoskeleton.ToggleActionUid, exoskeleton.ToggleAction, uid);
    }

    private void OnUnequip(EntityUid uid, STPoweredExoskeletonComponent exoskeleton, GotUnequippedEvent args)
    {
        _actions.RemoveAction(args.Equipee, exoskeleton.ToggleActionUid);
    }
}


public sealed partial class STTogglePoweredExoskeletonEvent : InstantActionEvent { }
