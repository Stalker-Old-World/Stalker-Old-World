using Content.Server.NPC.HTN;
using Content.Shared.Actions;
using Content.Shared.SinMobController;
using Content.Shared.NPC.Systems;
using Content.Shared.NPC.Components;
using Content.Server._Stalker.AllowTaking;
using Content.Shared.Mobs;
using Robust.Shared.Timing;

namespace Content.Server._Stalker.SinMobController;

public sealed partial class SinMobControllerSystem : EntitySystem
{
    [Dependency] private readonly EntityLookupSystem _lookup = default!;
    [Dependency] private readonly SharedActionsSystem _action = default!;
    [Dependency] private readonly HTNSystem _htn = default!;
    [Dependency] private readonly NpcFactionSystem _npc = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<SinMobControllerComponent, ComponentStartup>(OnStart);
        SubscribeLocalEvent<SinMobResistantComponent, ComponentStartup>(MakePeacefull);
        SubscribeLocalEvent<SinMobControllerComponent, StalkerSinMobControllerOffActionEvent>(ToggleOff);
        SubscribeLocalEvent<SinMobControllerComponent, StalkerSinMobControllerOnActionEvent>(ToggleOn);
        SubscribeLocalEvent<SinMobControllerComponent, MobStateChangedEvent>(OnStateChange);
    }


    public void OnStart(EntityUid uid, SinMobControllerComponent component, ComponentStartup args)
    {
        // Add actions
        _action.AddAction(uid, "StalkerSinMobControllerOffAction", uid);
        _action.AddAction(uid, "StalkerSinMobControllerOnAction", uid);
    }

    public void MakePeacefull(EntityUid uid, SinMobResistantComponent component, ComponentStartup args)
    {
        _npc.RemoveFaction(uid, component.Faction);
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);
        var query = EntityQueryEnumerator<SinMobResistantComponent>();
        while (query.MoveNext(out var uid, out var component))
        {
            component.TimeBeforeRemove -= TimeSpan.FromSeconds(frameTime);
            if (component.TimeBeforeRemove <= TimeSpan.Zero)
            {
                RemComp<SinMobResistantComponent>(uid);
                _npc.AddFaction(uid, component.Faction);
            }
        }
    }

    public void ToggleOff(EntityUid uid, SinMobControllerComponent component, StalkerSinMobControllerOffActionEvent args)
    {
        var position = Transform(uid).Coordinates;
        var humans = _lookup.GetEntitiesInRange<BlockTackingHolyItemsComponent>(position, 12f); // Selecting all alive objects which are not Sins

        foreach (var human in humans)
        {
            EnsureComp<SinMobResistantComponent>(human);
        }
    }

    public void ToggleOn(EntityUid uid, SinMobControllerComponent component, StalkerSinMobControllerOnActionEvent args)
    {
        ZeroRemovalTime();
    }
    public void OnStateChange(EntityUid uid, SinMobControllerComponent component, MobStateChangedEvent args)
    {
        ZeroRemovalTime();
    }

    public void ZeroRemovalTime()
    {
        var query = EntityQueryEnumerator<SinMobResistantComponent>();
        while (query.MoveNext(out var resistantguy, out var resistant))
        {
            resistant.TimeBeforeRemove = TimeSpan.Zero;
        }
    }

}
