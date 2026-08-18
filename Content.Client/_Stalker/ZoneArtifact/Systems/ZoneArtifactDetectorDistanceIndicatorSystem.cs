using Content.Client._Stalker.ZoneArtifact.UI;
using Content.Client.Items;
using Content.Shared._Stalker.ZoneArtifact.Components;

namespace Content.Client._Stalker.ZoneArtifact.Systems;

public sealed class ZoneArtifactDetectorDistanceIndicatorSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();

        Subs.ItemStatus<ZoneArtifactDetectorDistanceIndicatorComponent>(ent => new ZoneArtifactDetectorDistanceIndicatorControl(ent));
    }
}
