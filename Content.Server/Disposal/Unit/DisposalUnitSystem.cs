using Content.Server.Atmos.EntitySystems;
using Content.Shared.Atmos;
using Content.Shared.Destructible;
using Content.Shared.Disposal.Components;
using Content.Shared.Disposal.Unit;
using Content.Shared.Explosion;
using System.Linq; // ST:OW

namespace Content.Server.Disposal.Unit;

public sealed class DisposalUnitSystem : SharedDisposalUnitSystem
{
    [Dependency] private readonly AtmosphereSystem _atmosSystem = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<DisposalUnitComponent, DestructionEventArgs>(OnDestruction);
        SubscribeLocalEvent<DisposalUnitComponent, BeforeExplodeEvent>(OnExploded);
    }

    protected override void HandleAir(EntityUid uid, DisposalUnitComponent component, TransformComponent xform)
    {
        var air = component.Air;
        var indices = TransformSystem.GetGridTilePositionOrDefault((uid, xform));

        if (_atmosSystem.GetTileMixture(xform.GridUid, xform.MapUid, indices, true) is { Temperature: > 0f } environment)
        {
            var transferMoles = 0.1f * (0.25f * Atmospherics.OneAtmosphere * 1.01f - air.Pressure) * air.Volume / (environment.Temperature * Atmospherics.R);

            component.Air = environment.Remove(transferMoles);
        }
    }
    // ST:OW begin
    public override void ManualEngage(EntityUid uid, DisposalUnitComponent component, MetaDataComponent? metadata = null)
    {
        if (HasComp<DisposalDeleteContentsComponent>(uid))
        {
            VoidContents(uid, component);
            return;
        }

        // Normal disposal units keep their normal behavior
        base.ManualEngage(uid, component, metadata);
    }

    public override bool TryFlush(EntityUid uid, DisposalUnitComponent component)
    {
        if (HasComp<DisposalDeleteContentsComponent>(uid))
            return VoidContents(uid, component);

        return base.TryFlush(uid, component);
    }

    private bool VoidContents(EntityUid uid, DisposalUnitComponent component)
    {
        if (!Transform(uid).Anchored)
            return false;

        var hasContents = component.Container.ContainedEntities.Count > 0;

        if (hasContents)
        {
            foreach (var entity in component.Container.ContainedEntities.ToArray())
            {
                QueueDel(entity);
            }

            component.NextPressurized = TimeSpan.Zero;
        }

        component.Engaged = false;
        component.NextFlush = null;

        Dirty(uid, component);
        UpdateVisualState(uid, component);
        UpdateUI((uid, component));

        return hasContents;
    }
    // ST:OW end
    private void OnDestruction(EntityUid uid, DisposalUnitComponent component, DestructionEventArgs args)
    {
        TryEjectContents(uid, component);
    }

    private void OnExploded(Entity<DisposalUnitComponent> ent, ref BeforeExplodeEvent args)
    {
        args.Contents.AddRange(ent.Comp.Container.ContainedEntities);
    }
}
