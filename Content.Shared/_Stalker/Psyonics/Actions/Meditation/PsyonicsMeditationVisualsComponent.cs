using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Content.Shared._Stalker.Psyonics.Actions.Meditation;

/// <summary>
/// Sets which sprite RSI is used for displaying the psy shield visuals
/// </summary>
[RegisterComponent]
public sealed partial class PsyonicsMeditationVisualsComponent : Component
{
    [DataField]
    public string? State;

    [DataField]
    public float LightRadius = 2f;

    [DataField]
    public float LightEnergy = 2f;

    [DataField("lightColor")]
    public Color LightColor = Color.PaleGoldenrod;

    [DataField]
    public string? Sprite;

    /// <summary>
    ///     Client side point-light entity. We use this instead of directly adding a light to
    ///     the burning entity as entities don't support having multiple point-lights.
    /// </summary>
    public EntityUid? LightEntity;
}
