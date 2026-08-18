using System.IO;
using Content.Server.Administration.Logs;
using Content.Server.Popups;
using Content.Shared._Stalker_EN.Camera;
using Content.Shared.Charges.Systems;
using Content.Shared.Containers.ItemSlots;
using Content.Shared.Database;
using Content.Shared.DoAfter;
using Content.Shared.GameTicking;
using Content.Shared.Hands.EntitySystems;
using Content.Shared.Interaction.Events;
using Robust.Server.Player;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Network;
using Robust.Shared.Player;
using Robust.Shared.Timing;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Content.Server._Stalker_EN.Camera;

/// <summary>
/// Handles camera use-in-hand, DoAfter, capture request/response, and photo spawning.
/// </summary>
public sealed class STCameraSystem : SharedSTCameraSystem
{
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly IPlayerManager _playerManager = default!;
    [Dependency] private readonly SharedDoAfterSystem _doAfter = default!;
    [Dependency] private readonly SharedHandsSystem _hands = default!;
    [Dependency] private readonly SharedAudioSystem _audio = default!;
    [Dependency] private readonly SharedTransformSystem _transform = default!;
    [Dependency] private readonly ItemSlotsSystem _itemSlots = default!;
    [Dependency] private readonly SharedChargesSystem _charges = default!;
    [Dependency] private readonly PopupSystem _popup = default!;
    [Dependency] private readonly IAdminLogManager _adminLogger = default!;

    /// <summary>
    /// Tracks pending viewport capture requests per player.
    /// </summary>
    private readonly Dictionary<NetUserId, PendingCapture> _pendingCaptures = new();

    private static readonly TimeSpan TokenExpiry = TimeSpan.FromSeconds(10);
    private static readonly byte[] JpegMagic = { 0xFF, 0xD8, 0xFF };
    private static readonly byte[] JpegEoi = { 0xFF, 0xD9 };
    private const int MaxImageWidth = 1920;
    private const int MaxImageHeight = 1080;

    /// <summary>
    /// Lazy-allocated list for expired token cleanup.
    /// </summary>
    private List<NetUserId>? _toRemove;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<STCameraComponent, UseInHandEvent>(OnUseInHand);
        SubscribeLocalEvent<STCameraComponent, STCameraDoAfterEvent>(OnCameraDoAfterEvent);
        SubscribeNetworkEvent<STCaptureViewportResponseEvent>(OnViewportResponse);
        SubscribeLocalEvent<PlayerDetachedEvent>(OnPlayerDetached);
        SubscribeLocalEvent<RoundRestartCleanupEvent>(OnRoundRestart);
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        if (_pendingCaptures.Count == 0)
            return;

        var now = _timing.CurTime;
        _toRemove?.Clear();

        foreach (var (userId, pending) in _pendingCaptures)
        {
            if (now > pending.ExpiresAt)
                (_toRemove ??= new List<NetUserId>()).Add(userId);
        }

        if (_toRemove != null)
        {
            foreach (var userId in _toRemove)
            {
                _pendingCaptures.Remove(userId);
            }
        }
    }

    private void OnUseInHand(EntityUid uid, STCameraComponent comp, UseInHandEvent args)
    {
        if (args.Handled)
            return;

        args.Handled = true;

        if (_timing.CurTime < comp.NextCaptureTime)
        {
            _popup.PopupEntity(Loc.GetString("st-camera-cooldown"), uid, args.User);
            return;
        }

        if (!_itemSlots.TryGetSlot(uid, STCameraComponent.FilmSlotId, out var filmSlot) || filmSlot.Item is not { } filmItem)
        {
            _popup.PopupEntity(Loc.GetString("st-camera-no-film"), uid, args.User);
            return;
        }

        if (_charges.IsEmpty(filmItem))
        {
            _popup.PopupEntity(Loc.GetString("st-camera-film-empty"), uid, args.User);
            return;
        }

        var doAfterArgs = new DoAfterArgs(EntityManager, args.User, comp.CaptureDelay, new STCameraDoAfterEvent(), uid, used: uid)
        {
            BreakOnMove = true,
            NeedHand = true,
            BreakOnDamage = true,
            BlockDuplicate = true,
        };

        _doAfter.TryStartDoAfter(doAfterArgs);
    }

    private void OnCameraDoAfterEvent(EntityUid uid, STCameraComponent comp, STCameraDoAfterEvent args)
    {
        if (args.Cancelled || args.Handled)
            return;

        args.Handled = true;

        var user = args.User;

        if (!_playerManager.TryGetSessionByEntity(user, out var session))
            return;

        var token = Guid.NewGuid();
        _pendingCaptures[session.UserId] = new PendingCapture(
            token,
            uid,
            user,
            _timing.CurTime + TokenExpiry);

        RaiseNetworkEvent(new STCaptureViewportRequestEvent
        {
            Token = token,
            Camera = GetNetEntity(uid),
            Effect = comp.Effect,
        }, session);

        _audio.PlayPvs(comp.CaptureSound, uid);
        comp.NextCaptureTime = _timing.CurTime + comp.CaptureCooldown;
        Dirty(uid, comp);
    }

    private void OnViewportResponse(STCaptureViewportResponseEvent ev, EntitySessionEventArgs args)
    {
        var userId = args.SenderSession.UserId;

        if (!_pendingCaptures.TryGetValue(userId, out var pending))
            return;

        if (ev.Token != pending.Token)
            return;

        _pendingCaptures.Remove(userId);

        if (ev.ImageData.Length == 0)
            return;

        if (ev.ImageData.Length < JpegMagic.Length
            || !ev.ImageData.AsSpan(0, JpegMagic.Length).SequenceEqual(JpegMagic))
        {
            _adminLogger.Add(LogType.STPhoto, LogImpact.Medium,
                $"{ToPrettyString(pending.User):player} sent invalid photo data (bad JPEG header)");
            return;
        }

        if (ev.ImageData.Length < JpegEoi.Length
            || !ev.ImageData.AsSpan(ev.ImageData.Length - JpegEoi.Length).SequenceEqual(JpegEoi))
        {
            _adminLogger.Add(LogType.STPhoto, LogImpact.Medium,
                $"{ToPrettyString(pending.User):player} sent invalid photo data (missing JPEG EOI marker)");
            return;
        }

        var cameraUid = pending.Camera;

        if (!TryComp<STCameraComponent>(cameraUid, out var comp))
            return;

        if (ev.ImageData.Length > comp.MaxImageBytes)
        {
            _adminLogger.Add(LogType.STPhoto, LogImpact.Medium,
                $"{ToPrettyString(pending.User):player} sent oversized photo: {ev.ImageData.Length} bytes (max {comp.MaxImageBytes})");
            return;
        }

        // Decode the JPEG to validate it is a well-formed image and check dimensions.
        // Prevents malformed JPEGs from propagating to viewing clients as a decompression bomb.
        try
        {
            using var stream = new MemoryStream(ev.ImageData);
            using var image = Image.Load<Rgba32>(stream);
            if (image.Width > MaxImageWidth || image.Height > MaxImageHeight)
            {
                _adminLogger.Add(LogType.STPhoto, LogImpact.Medium,
                    $"{ToPrettyString(pending.User):player} sent excessive photo dimensions: {image.Width}x{image.Height}");
                return;
            }
        }
        catch (Exception e)
        {
            _adminLogger.Add(LogType.STPhoto, LogImpact.Medium,
                $"{ToPrettyString(pending.User):player} sent undecodable JPEG: {e.Message}");
            return;
        }

        if (!Exists(pending.User))
            return;

        var photoUid = Spawn(comp.PhotoPrototype, _transform.GetMoverCoordinates(cameraUid));

        if (!TryComp<STPhotoComponent>(photoUid, out var photo))
        {
            Del(photoUid);
            return;
        }

        photo.ImageData = ev.ImageData;
        photo.PhotoId = Guid.NewGuid();
        Dirty(photoUid, photo);

        // Consume a film charge and auto-delete empty film
        if (_itemSlots.TryGetSlot(cameraUid, STCameraComponent.FilmSlotId, out var filmSlot)
            && filmSlot.Item is { } filmItem)
        {
            _charges.TryUseCharge(filmItem);

            if (_charges.IsEmpty(filmItem))
            {
                _itemSlots.TryEject(cameraUid, STCameraComponent.FilmSlotId, null, out _);
                Del(filmItem);
            }
        }

        _adminLogger.Add(LogType.STPhoto, LogImpact.Low,
            $"{ToPrettyString(pending.User):player} took a photo (PhotoId: {photo.PhotoId}, Camera: {ToPrettyString(cameraUid)})");

        // Try to give to player, fall back to dropping at feet
        _hands.PickupOrDrop(pending.User, photoUid);
    }

    private void OnRoundRestart(RoundRestartCleanupEvent ev)
    {
        _pendingCaptures.Clear();
    }

    private void OnPlayerDetached(PlayerDetachedEvent args)
    {
        _pendingCaptures.Remove(args.Player.UserId);
    }

    private readonly record struct PendingCapture(Guid Token, EntityUid Camera, EntityUid User, TimeSpan ExpiresAt);
}
