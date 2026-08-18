using Content.Shared._RMC14.Weapons.Ranged.Prediction;
using Content.Shared._Stalker.Random;
using Content.Shared._Stalker.Weapon.Evasion;
using Content.Shared.FixedPoint;
using Content.Shared.Projectiles;
using Robust.Shared.Physics.Events;
using Robust.Shared.Timing;
using System.Linq;
using EntityCoordinates = Robust.Shared.Map.EntityCoordinates;

namespace Content.Shared._Stalker.Weapon.Projectile;

public sealed class STProjectileSystem : EntitySystem
{
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly SharedTransformSystem _transform = default!;
    [Dependency] private readonly STEvasionSystem _evasion = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<STProjectileDamageFalloffComponent, MapInitEvent>(OnFalloffProjectileMapInit);
        SubscribeLocalEvent<STProjectileDamageFalloffComponent, ComponentStartup>(OnFalloffProjectileStartup);
        SubscribeLocalEvent<STProjectileDamageFalloffComponent, ProjectileHitEvent>(OnFalloffProjectileHit);

        SubscribeLocalEvent<STProjectileAccuracyComponent, MapInitEvent>(OnProjectileAccuracyMapInit);
        SubscribeLocalEvent<STProjectileAccuracyComponent, ComponentStartup>(OnProjectileAccuracyStartup);
        SubscribeLocalEvent<STProjectileAccuracyComponent, PreventCollideEvent>(OnProjectileAccuracyPreventCollide);
    }

    public void SetProjectileFalloffWeaponModifier(Entity<STProjectileDamageFalloffComponent> projectile, float modifier)
    {
        projectile.Comp.WeaponModifier = modifier;
        Dirty(projectile);
    }

    public void SetFalloffStartCoordinates(EntityUid projectile, EntityCoordinates startCoords)
    {
        if (TryComp<STProjectileDamageFalloffComponent>(projectile, out var falloffComp))
        {
            falloffComp.StartCoordinates = startCoords;
            Dirty(projectile, falloffComp);
        }
    }

    public void SetAccuracyStartCoordinates(EntityUid projectile, EntityCoordinates startCoords)
    {
        if (TryComp<STProjectileAccuracyComponent>(projectile, out var accuracyComp))
        {
            accuracyComp.StartCoordinates = startCoords;
            Dirty(projectile, accuracyComp);
        }
    }

    private void OnFalloffProjectileStartup(Entity<STProjectileDamageFalloffComponent> ent, ref ComponentStartup args)
    {
        if (ent.Comp.StartCoordinates is null)
        {
            ent.Comp.StartCoordinates = _transform.GetMoverCoordinates(ent.Owner);
            Dirty(ent);
        }
    }

    private void OnProjectileAccuracyStartup(Entity<STProjectileAccuracyComponent> ent, ref ComponentStartup args)
    {
        if (ent.Comp.StartCoordinates is null)
        {
            ent.Comp.StartCoordinates = _transform.GetMoverCoordinates(ent.Owner);
            Dirty(ent);
        }
    }

    private void OnFalloffProjectileMapInit(Entity<STProjectileDamageFalloffComponent> ent, ref MapInitEvent args)
    {
        if (ent.Comp.StartCoordinates is null)
        {
            ent.Comp.StartCoordinates = _transform.GetMoverCoordinates(ent.Owner);
            Dirty(ent);
        }
    }

    private void OnProjectileAccuracyMapInit(Entity<STProjectileAccuracyComponent> ent, ref MapInitEvent args)
    {
        if (ent.Comp.StartCoordinates is null)
        {
            ent.Comp.StartCoordinates = _transform.GetMoverCoordinates(ent.Owner);
            Dirty(ent);
        }
        ent.Comp.Tick = _timing.CurTick.Value;
        Dirty(ent);
    }

    private void OnFalloffProjectileHit(Entity<STProjectileDamageFalloffComponent> ent, ref ProjectileHitEvent args)
    {
        EntityCoordinates? startCoords = null;

        if (TryComp<PredictedProjectileHitComponent>(ent, out var predictedHit))
        {
            startCoords = predictedHit.Origin;
        }
        else
        {
            startCoords = ent.Comp.StartCoordinates;
        }

        if (startCoords is null || ent.Comp.MinRemainingDamageModifier < 0)
            return;

        var startMapPos = _transform.ToMapCoordinates(startCoords.Value);
        var targetMapPos = _transform.GetMapCoordinates(args.Target);

        if (startMapPos.MapId != targetMapPos.MapId)
            return;

        var distance = (targetMapPos.Position - startMapPos.Position).Length();
        var minDamage = args.Damage.GetTotal() * ent.Comp.MinRemainingDamageModifier;

        var totalFalloff = 0f;
        var sortedThresholds = ent.Comp.Thresholds.OrderBy(t => t.Range).ToList();
        float previousRange = 0;
        int processedCount = 0;

        foreach (var threshold in sortedThresholds)
        {
            if (distance <= threshold.Range)
            {
                var rangeInSegment = distance - previousRange;
                if (rangeInSegment > 0)
                {
                    var extraModifier = threshold.IgnoreModifiers ? 1 : ent.Comp.WeaponModifier;
                    totalFalloff += rangeInSegment * threshold.Falloff * extraModifier;
                }
                processedCount++;
                break;
            }

            var fullRangeInSegment = threshold.Range - previousRange;
            if (fullRangeInSegment > 0)
            {
                var extraModifier = threshold.IgnoreModifiers ? 1 : ent.Comp.WeaponModifier;
                totalFalloff += fullRangeInSegment * threshold.Falloff * extraModifier;
            }

            previousRange = threshold.Range;
            processedCount++;
        }

        if (processedCount >= sortedThresholds.Count && distance > previousRange)
        {
            var lastThreshold = sortedThresholds.Last();
            var rangeInSegment = distance - previousRange;
            if (rangeInSegment > 0)
            {
                var extraModifier = lastThreshold.IgnoreModifiers ? 1 : ent.Comp.WeaponModifier;
                totalFalloff += rangeInSegment * lastThreshold.Falloff * extraModifier;
            }
        }

        var totalDamage = args.Damage.GetTotal();
        if (totalDamage <= minDamage)
            return;

        var minModifier = FixedPoint2.Min(minDamage / totalDamage, 1);
        var damageMultiplier = FixedPoint2.Clamp((totalDamage - totalFalloff) / totalDamage, minModifier, 1);
        args.Damage *= damageMultiplier;
    }

    private void OnProjectileAccuracyPreventCollide(Entity<STProjectileAccuracyComponent> ent, ref PreventCollideEvent args)
    {
        if (args.Cancelled || ent.Comp.ForceHit || ent.Comp.StartCoordinates is null)
            return;

        if (!HasComp<STEvasionComponent>(args.OtherEntity))
            return;

        var accuracy = ent.Comp.Accuracy;
        var startMapPos = _transform.ToMapCoordinates(ent.Comp.StartCoordinates.Value);
        var targetMapPos = _transform.GetMapCoordinates(args.OtherEntity);

        if (startMapPos.MapId != targetMapPos.MapId)
            return;

        var distance = (targetMapPos.Position - startMapPos.Position).Length();

        foreach (var threshold in ent.Comp.Thresholds)
        {
            accuracy += CalculateFalloff(distance - threshold.Range, threshold.Falloff, threshold.AccuracyGrowth);
        }

        accuracy -= _evasion.GetEvasion(args.OtherEntity);
        accuracy = accuracy > ent.Comp.MinAccuracy ? accuracy : ent.Comp.MinAccuracy;

        var random = new STXoshiro128P(ent.Comp.GunSeed, ((long) ent.Comp.Tick << 32) | (uint) GetNetEntity(args.OtherEntity).Id).NextFloat(0f, 1f);

        if (accuracy >= random)
            return;

        args.Cancelled = true;
    }

    private static float CalculateFalloff(float range, float falloff, bool accuracyGrowth)
    {
        if (accuracyGrowth)
            return range >= 0 ? 0 : falloff * range;

        return range <= 0 ? 0 : -falloff * range;
    }
}
