using System.Diagnostics.CodeAnalysis;
using System.IO;
using Content.Shared._Stalker_EN.Camera;
using Robust.Client.Graphics;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Content.Client._Stalker_EN.Camera;

/// <summary>
/// Client-side photo system: requests, caches, and provides photo textures.
/// </summary>
public sealed class STPhotoSystem : EntitySystem
{
    [Dependency] private readonly IClyde _clyde = default!;

    private const int MaxCacheSize = 50;

    /// <summary>
    /// LRU-bounded texture cache keyed by PhotoId.
    /// </summary>
    private readonly Dictionary<Guid, CachedPhoto> _photoCache = new();

    /// <summary>
    /// Insertion order for LRU eviction.
    /// </summary>
    private readonly LinkedList<Guid> _lruOrder = new();

    /// <summary>
    /// Dedup in-flight requests.
    /// </summary>
    private readonly HashSet<Guid> _pendingRequests = new();

    /// <summary>
    /// Raised when a photo texture becomes available.
    /// </summary>
    public event Action<Guid>? PhotoReceived;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeNetworkEvent<STPhotoResponseEvent>(OnPhotoResponse);
    }

    /// <summary>
    /// Try to get a cached texture for the given photo ID.
    /// </summary>
    public bool TryGetCachedTexture(Guid photoId, [NotNullWhen(true)] out Texture? texture)
    {
        if (_photoCache.TryGetValue(photoId, out var cached))
        {
            _lruOrder.Remove(cached.LruNode);
            _lruOrder.AddLast(cached.LruNode);
            texture = cached.Texture;
            return true;
        }

        texture = null;
        return false;
    }

    /// <summary>
    /// Request photo image data from the server if not already cached or pending.
    /// </summary>
    public void RequestPhoto(NetEntity entity, Guid photoId)
    {
        if (_photoCache.ContainsKey(photoId))
            return;

        if (!_pendingRequests.Add(photoId))
            return;

        RaiseNetworkEvent(new STPhotoRequestEvent
        {
            PhotoEntity = entity,
            PhotoId = photoId,
        });
    }

    /// <summary>
    /// Request shared photo data by ID only (no entity required).
    /// Used for photos embedded in news articles.
    /// </summary>
    public void RequestSharedPhoto(Guid photoId)
    {
        if (photoId == Guid.Empty)
            return;

        if (_photoCache.ContainsKey(photoId))
            return;

        if (!_pendingRequests.Add(photoId))
            return;

        RaiseNetworkEvent(new STSharedPhotoRequestEvent
        {
            PhotoId = photoId,
        });
    }

    private void OnPhotoResponse(STPhotoResponseEvent ev)
    {
        _pendingRequests.Remove(ev.PhotoId);

        if (ev.ImageData.Length == 0)
            return;

        try
        {
            using var stream = new MemoryStream(ev.ImageData);
            using var image = Image.Load<Rgba32>(stream);
            var texture = _clyde.LoadTextureFromImage(image);
            CacheTexture(ev.PhotoId, texture);
            PhotoReceived?.Invoke(ev.PhotoId);
        }
        catch (Exception e)
        {
            Log.Error($"Failed to decode photo {ev.PhotoId}: {e.Message}");
        }
    }

    private void CacheTexture(Guid photoId, Texture texture)
    {
        while (_photoCache.Count >= MaxCacheSize && _lruOrder.First != null)
        {
            var oldestId = _lruOrder.First.Value;
            _lruOrder.RemoveFirst();
            _photoCache.Remove(oldestId);
        }

        var node = _lruOrder.AddLast(photoId);
        _photoCache[photoId] = new CachedPhoto(texture, node);
    }

    private sealed record CachedPhoto(Texture Texture, LinkedListNode<Guid> LruNode);
}
