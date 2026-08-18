using System.Numerics;
using Robust.Shared.Map.Components;

namespace Content.Server._Stalker_EN.Devices.Radar.EntitySystems;

/// <summary>
/// Shared helper for computing radar-space angles from user to target,
/// using grid-local coordinates when available.
/// </summary>
public static class RadarAngleHelper
{
    /// <summary>
    /// Calculates a radar-space angle from user to target.
    /// Uses grid-local coordinates when on a grid for consistency across grid rotations.
    /// </summary>
    /// <returns>Angle in radians, normalized to -PI..PI. 0 = north, positive = clockwise.</returns>
    public static float CalculateRadarAngle(
        SharedMapSystem map,
        EntityUid? gridUid,
        MapGridComponent? grid,
        Vector2 userWorldPos,
        Vector2 targetWorldPos)
    {
        float radarAngle;
        var diff = targetWorldPos - userWorldPos;

        if (grid != null && gridUid != null)
        {
            var userLocal = map.WorldToLocal(gridUid.Value, grid, userWorldPos);
            var targetLocal = map.WorldToLocal(gridUid.Value, grid, targetWorldPos);
            var localAngle = new Angle(targetLocal - userLocal);
            radarAngle = (float)(Math.PI / 2 - localAngle.Theta);
        }
        else
        {
            radarAngle = (float)(Math.PI / 2 - new Angle(diff).Theta);
        }

        // Normalize to -PI..PI
        while (radarAngle > MathF.PI)
            radarAngle -= MathF.PI * 2;
        while (radarAngle < -MathF.PI)
            radarAngle += MathF.PI * 2;

        return radarAngle;
    }
}
