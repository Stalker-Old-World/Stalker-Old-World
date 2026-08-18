using Content.Server._Stalker.Anomaly.TargetGroups.Weight;
using Content.Shared._Stalker.Anomaly.Triggers.Events;
using Content.Shared._Stalker.Weight;

namespace Content.Server._Stalker.Anomaly.Triggers.StartCollide;

public sealed class STAnomalyTriggerStartCollideWeightSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<STAnomalyTriggerStartCollideWeightComponent, STAnomalyTriggerStartCollideGetAdditionalGroupsEvent>(GetAdditionalGroups);
    }

    private void GetAdditionalGroups(Entity<STAnomalyTriggerStartCollideWeightComponent> trigger, ref STAnomalyTriggerStartCollideGetAdditionalGroupsEvent args)
    {
        if (!TryComp<STWeightComponent>(args.Target, out var weightComponent))
            return;

        var weightGroup = string.Empty;
        var maxWeight = 0f;

        foreach (var (weight, group) in trigger.Comp.WeightTriggerGroup)
        {
            if (weightComponent.Total < weight || maxWeight > weight)
                continue;

            weightGroup = group;
            maxWeight = weight;
        }

        args.Add(weightGroup);
    }
}
