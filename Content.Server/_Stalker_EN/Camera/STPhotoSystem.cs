using Content.Server.Database;
using Content.Server._Stalker_EN.News;
using Content.Shared._Stalker_EN.Camera;
using Content.Shared.UserInterface;
using Robust.Server.GameObjects;
using Robust.Shared.Network;
using Robust.Shared.Player;
using Content.Shared.GameTicking;
using Robust.Shared.Timing;

namespace Content.Server._Stalker_EN.Camera;

/// <summary>
/// Serves photo image data on demand to clients and manages photo BUI state.
/// Handles both entity-backed photo requests and shared (entity-less) photo requests
/// for photos embedded in news articles.
/// </summary>
public sealed class STPhotoSystem : EntitySystem
{
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly IServerDbManager _dbManager = default!;
    [Dependency] private readonly UserInterfaceSystem _ui = default!;
    [Dependency] private readonly STNewsSystem _news = default!;

    /// <summary>
    /// Rate limiting: last entity photo request time per player.
    /// </summary>
    private readonly Dictionary<NetUserId, TimeSpan> _lastEntityPhotoRequest = new();

    /// <summary>
    /// Rate limiting: last shared photo request time per player.
    /// </summary>
    private readonly Dictionary<NetUserId, TimeSpan> _lastSharedPhotoRequest = new();

    private static readonly TimeSpan RequestCooldown = TimeSpan.FromSeconds(1);
    private static readonly TimeSpan SharedRequestCooldown = TimeSpan.FromMilliseconds(250);

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<STPhotoComponent, BeforeActivatableUIOpenEvent>(OnBeforeUI);
        SubscribeNetworkEvent<STPhotoRequestEvent>(OnPhotoRequest);
        SubscribeNetworkEvent<STSharedPhotoRequestEvent>(OnSharedPhotoRequest);
        SubscribeLocalEvent<PlayerDetachedEvent>(OnPlayerDetached);
        SubscribeLocalEvent<RoundRestartCleanupEvent>(OnRoundRestart);
    }

    private void OnBeforeUI(EntityUid uid, STPhotoComponent comp, BeforeActivatableUIOpenEvent args)
    {
        var state = new STPhotoBoundUiState(comp.PhotoId);
        _ui.SetUiState(uid, STPhotoUiKey.Key, state);
    }

    private void OnPhotoRequest(STPhotoRequestEvent ev, EntitySessionEventArgs args)
    {
        var userId = args.SenderSession.UserId;
        var now = _timing.CurTime;

        // Rate limit: 1 request per second per session
        if (_lastEntityPhotoRequest.TryGetValue(userId, out var lastRequest) && now - lastRequest < RequestCooldown)
            return;

        _lastEntityPhotoRequest[userId] = now;

        var photoUid = GetEntity(ev.PhotoEntity);

        if (!TryComp<STPhotoComponent>(photoUid, out var photo))
            return;

        if (photo.ImageData.Length == 0)
            return;

        if (photo.PhotoId != ev.PhotoId)
            return;

        RaiseNetworkEvent(new STPhotoResponseEvent
        {
            PhotoId = photo.PhotoId,
            ImageData = photo.ImageData,
        }, args.SenderSession);
    }

    /// <summary>
    /// Handles shared photo requests (no entity required).
    /// Looks up photo bytes from the news DB.
    /// </summary>
    private void OnSharedPhotoRequest(STSharedPhotoRequestEvent ev, EntitySessionEventArgs args)
    {
        var userId = args.SenderSession.UserId;
        var now = _timing.CurTime;

        if (_lastSharedPhotoRequest.TryGetValue(userId, out var lastRequest) && now - lastRequest < SharedRequestCooldown)
            return;

        _lastSharedPhotoRequest[userId] = now;

        if (ev.PhotoId == Guid.Empty)
            return;

        // Only serve photos referenced by a cached article to prevent DB enumeration (F-04)
        if (!_news.IsPhotoInCache(ev.PhotoId))
            return;

        LookupNewsPhotoAsync(ev.PhotoId, args.SenderSession);
    }

    private async void LookupNewsPhotoAsync(Guid photoId, ICommonSession session)
    {
        try
        {
            var dbPhoto = await _dbManager.GetStalkerNewsArticlePhotoAsync(photoId);
            if (dbPhoto == null)
                return;

            RaiseNetworkEvent(new STPhotoResponseEvent
            {
                PhotoId = photoId,
                ImageData = dbPhoto.PhotoData,
            }, session);
        }
        catch (Exception e)
        {
            Log.Error($"Failed to load news photo {photoId}: {e.Message}");
        }
    }

    private void OnRoundRestart(RoundRestartCleanupEvent ev)
    {
        _lastEntityPhotoRequest.Clear();
        _lastSharedPhotoRequest.Clear();
    }

    private void OnPlayerDetached(PlayerDetachedEvent args)
    {
        _lastEntityPhotoRequest.Remove(args.Player.UserId);
        _lastSharedPhotoRequest.Remove(args.Player.UserId);
    }
}
