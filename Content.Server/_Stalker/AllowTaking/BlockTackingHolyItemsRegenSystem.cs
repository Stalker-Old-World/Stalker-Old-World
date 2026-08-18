using Content.Shared.Interaction.Components;
using Content.Shared.Interaction.Events;
using Content.Shared.Item;
using Content.Shared.Tag;

namespace Content.Server._Stalker.AllowTaking;

public sealed class BlockTackingHolyItemsSystem : EntitySystem
{
    [Dependency] private readonly TagSystem _tagSystem = default!;
    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<BlockTackingHolyItemsComponent, InteractionAttemptEvent>(OnInteractionAttempt);
    }
    private void OnInteractionAttempt(EntityUid uid, BlockTackingHolyItemsComponent component, ref InteractionAttemptEvent args)
    {
        if (!HasComp<ItemComponent>(args.Target) || HasComp<UnremoveableComponent>(args.Target) || args.Target == null)
            return;

        if (_tagSystem.HasAnyTag(args.Target.Value, "GrehCanTakeOnly"))
            args.Cancelled = true;
    }
}
