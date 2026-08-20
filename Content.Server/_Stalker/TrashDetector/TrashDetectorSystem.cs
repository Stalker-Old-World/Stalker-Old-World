using Content.Server.TrashDetector.Components;
using Content.Server.Popups;
using Robust.Shared.Random;
using Content.Shared.DoAfter;
using Robust.Shared.Map;
using Robust.Server.Audio;
using Content.Shared.Popups;
using Content.Shared.Interaction;
using Content.Server._Stalker.TrashSerchable;
using Content.Shared.TrashDetector;
using Content.Shared.Hands.EntitySystems; // ST:OW

namespace Content.Server.TrashDetector
{
    public sealed partial class TrashDetectorSystem : EntitySystem
    {
        [Dependency] private readonly PopupSystem _popupSystem = default!;
        [Dependency] private readonly IRobustRandom _random = default!;
        [Dependency] private readonly SharedDoAfterSystem _doAfterSystem = default!;
        [Dependency] internal readonly IEntityManager _entityManager = default!;
        [Dependency] internal readonly IMapManager _mapManager = default!;
        [Dependency] protected readonly AudioSystem Audio = default!;
        [Dependency] private readonly SharedHandsSystem _hands = default!;

        public override void Initialize()
        {
            base.Initialize();
            SubscribeLocalEvent<TrashDetectorComponent, BeforeRangedInteractEvent>(OnUseInHand);
            SubscribeLocalEvent<TrashDetectorComponent, GetTrashDoAfterEvent>(OnDoAfter);
        }

        public override void Update(float frameTime)
        {
            base.Update(frameTime);

        }

        public void OnUseInHand(EntityUid uid, TrashDetectorComponent comp, BeforeRangedInteractEvent args)
        {
            if (!args.CanReach)
                return;
            OnUse(uid, comp, args.Target, args.User);
        }

        public void OnUse(EntityUid? uid, TrashDetectorComponent comp, EntityUid? target, EntityUid user)
        {
            if (target == null)
                return;
            if (TryComp<TrashSerchableComponent>(target, out var trash) && trash != null)
            {
                if (trash.TimeBeforeNextSearch < 0f)
                {
                    var doAfterArgs = new DoAfterArgs(_entityManager, user, comp.SearchTime, new GetTrashDoAfterEvent(),
                        uid, target: target, used: uid)
                    {
                        BreakOnDamage = true,
                        NeedHand = true,
                        DistanceThreshold = 2f,
                    };

                    _doAfterSystem.TryStartDoAfter(doAfterArgs);
                }
                else
                {
                    _popupSystem.PopupEntity("This pile has already been checked recently", user,
                        PopupType.LargeCaution);
                }
            }

        }

        // ST:OW begin
        private void SpawnLootToHandsOrGround(EntityUid user, string protoId)
        {
            var coords = Transform(user).Coordinates;
            var spawned = Spawn(protoId, coords);
            _hands.TryPickupAnyHand(user, spawned); // if fails, stays on ground
        }

        public void OnDoAfter(EntityUid uid, TrashDetectorComponent comp, GetTrashDoAfterEvent args)
        {
            if (args.Handled || args.Cancelled || args.Args.Target == null ||
                !TryComp<TrashSerchableComponent>(args.Args.Target.Value, out var trash))
            {
                return;
            }

            args.Handled = true;

            trash.TimeBeforeNextSearch = 900f;

            if (!_random.Prob(comp.Probability))
            {
                _popupSystem.PopupEntity("The device does not make a sound", uid, PopupType.LargeCaution);
                return;
            }

            _popupSystem.PopupEntity("The device beeps", uid, PopupType.LargeCaution);

            var hardCap = Math.Max(1, comp.RollsHardCap);
            var minRolls = Math.Clamp(comp.RollsMin, 1, hardCap);
            var maxRolls = Math.Clamp(comp.RollsMax, minRolls, hardCap);

            var rolls = _random.Next(minRolls, maxRolls + 1);

            for (var i = 0; i < rolls; i++)
            {
                SpawnLootToHandsOrGround(uid, comp.Loot);
            }
        }
        // ST:OW end
    }
}
