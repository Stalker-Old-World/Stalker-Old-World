using Content.Shared.Examine;
using Content.Shared.Radiation.Components;

namespace Content.Shared.Radiation.Systems;

public abstract class SharedGeigerSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<GeigerComponent, ExaminedEvent>(OnExamine);
    }

    private void OnExamine(EntityUid uid, GeigerComponent component, ExaminedEvent args)
    {
        if (!component.ShowExamine || !component.IsEnabled || !args.IsInDetailsRange)
            return;
        float currentRads = component.CurrentRadiation; // stalker en change
        if (component.damageTypes.Count == 1 && component.damageTypes[0] == "Radiation"){ // stalker en change
            var rads = currentRads.ToString("N1");
            var color = LevelToColor(component.DangerLevel);
            var msg = Loc.GetString("geiger-component-examine",
                ("rads", rads), ("color", color));
            args.PushMarkup(msg);
        }
        else // start stalker en changes
        {
            var rads = currentRads.ToString("N"+component.accuracy);
            
            var color = Loc.GetString("geiger-get-color", ("color",LevelToColor(component.DangerLevel)));
            string message = "";
            if (component.damageTypes.Count == 1)
            {
                message = $"Current {component.damageTypes[0].ToString()} radiation: [color={color}]{rads} rads[/color]";
                args.PushMarkup(message);
                return;
            }
            message += $"Current total radiation: [color={color}]{rads} rads[/color]";
            foreach (string damageType in component.damageTypes)
            {
                if (component.CurrentRadiationLevels.TryGetValue(damageType, out var t))
                {
                    float level = t.Item1;
                    var danger = t.Item2;
                    rads = level.ToString("N"+component.accuracy);
                    color = Loc.GetString("geiger-get-color", ("color",LevelToColor(danger)));
                    if (level == 0)
                    {
                        continue;
                    }
                }
                else
                {
                    continue;
                }
                message += $"\n{damageType}: [color={color}]{rads} rads[/color]";
            }
            args.PushMarkup(message); // end stalker en changes
        }
    }

    public static Color LevelToColor(GeigerDangerLevel level)
    {
        switch (level)
        {
            case GeigerDangerLevel.None:
                return Color.Green;
            case GeigerDangerLevel.Low:
                return Color.Yellow;
            case GeigerDangerLevel.Med:
                return Color.DarkOrange;
            case GeigerDangerLevel.High:
            case GeigerDangerLevel.Extreme:
                return Color.Red;
            default:
                return Color.White;
        }
    }
}
