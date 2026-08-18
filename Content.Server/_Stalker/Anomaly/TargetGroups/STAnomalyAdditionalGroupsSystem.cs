using Content.Shared._Stalker.Anomaly.Triggers.Events;

namespace Content.Server._Stalker.Anomaly.TargetGroups;

public sealed class STAnomalyAdditionalGroupsSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<STAnomalyAdditionalGroupsComponent, STAnomalyTriggerStartCollideGetAdditionalGroupsEvent>(GetAdditionalGroups);
    }

    private void GetAdditionalGroups(Entity<STAnomalyAdditionalGroupsComponent> additionalGroups, ref STAnomalyTriggerStartCollideGetAdditionalGroupsEvent args)
    {
        args.Add(additionalGroups.Comp.Groups);
    }
}
