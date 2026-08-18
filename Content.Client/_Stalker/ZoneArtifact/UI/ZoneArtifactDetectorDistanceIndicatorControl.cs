using Content.Client.Message;
using Content.Client.Stylesheets;
using Content.Shared._Stalker.ZoneArtifact.Components;
using Content.Shared.Radiation.Components;
using Content.Shared.Radiation.Systems;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Shared.Timing;

namespace Content.Client._Stalker.ZoneArtifact.UI;

public sealed class ZoneArtifactDetectorDistanceIndicatorControl : Control
{
    private readonly ZoneArtifactDetectorDistanceIndicatorComponent _component;
    private readonly RichTextLabel _label;

    public ZoneArtifactDetectorDistanceIndicatorControl(ZoneArtifactDetectorDistanceIndicatorComponent component)
    {
        _component = component;
        _label = new RichTextLabel { StyleClasses = { StyleNano.StyleClassItemStatus } };

        AddChild(_label);
        Update();
    }

    protected override void FrameUpdate(FrameEventArgs args)
    {
        base.FrameUpdate(args);
        Update();
    }

    private void Update()
    {
        if (!_component.Enabled)
        {
            _label.SetMarkup(Loc.GetString("distance-indicator-component-disabled"));
            return;
        }

        if (_component.Distance is { } distance)
        {
            _label.SetMarkup(Loc.GetString("distance-indicator-component-examine", ("distance", distance.ToString("N1"))));
            return;
        }

        _label.SetMarkup(Loc.GetString("distance-indicator-component-search"));
    }
}
