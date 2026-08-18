using Content.Shared.StatusIcon;
using Robust.Client.GameObjects;
using Robust.Client.Graphics;
using Robust.Client.ResourceManagement;
using Robust.Client.UserInterface.Controls;
using Robust.Shared.IoC;
using Robust.Shared.Maths;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;
using System.Numerics;
using static Robust.Client.UserInterface.Control;

namespace Content.Client._Stalker_EN.UI;

/// <summary>
/// Helper class for loading portrait and patch icons.
/// Used by PDA notifications and leaderboard to avoid code duplication.
/// </summary>
public static class PortraitIconHelper
{
    /// <summary>
    /// Loads a PNG portrait texture from the resource cache.
    /// </summary>
    public static Texture? LoadPortraitTexture(string? portraitPath)
    {
        if (string.IsNullOrEmpty(portraitPath))
            return null;

        try
        {
            var resourceCache = IoCManager.Resolve<IResourceCache>();
            if (resourceCache.TryGetResource<TextureResource>(portraitPath, out var texture))
                return texture;
        }
        catch
        {
            // Fall through to null
        }

        return null;
    }

    /// <summary>
    /// Loads a JobIconPrototype sprite texture.
    /// </summary>
    public static Texture? LoadPatchTexture(string? bandIcon)
    {
        if (string.IsNullOrEmpty(bandIcon))
            return null;

        try
        {
            var protoManager = IoCManager.Resolve<IPrototypeManager>();
            var spriteSystem = IoCManager.Resolve<IEntitySystemManager>().GetEntitySystem<SpriteSystem>();

            if (protoManager.TryIndex<JobIconPrototype>(bandIcon, out var iconProto))
                return spriteSystem.Frame0(iconProto.Icon);
        }
        catch
        {
            // Fall through to null
        }

        return null;
    }

    /// <summary>
    /// Creates a TextureRect with either a portrait or patch icon.
    /// </summary>
    /// <param name="portraitPath">Path to PNG portrait (optional)</param>
    /// <param name="bandIcon">JobIconPrototype ID for patch (optional)</param>
    /// <param name="usePngIcons">If true, prefers PNG portrait over patch</param>
    /// <param name="size">Size of the TextureRect</param>
    /// <param name="fallbackTexture">Fallback texture if both portrait and patch fail (optional)</param>
    public static TextureRect CreatePortraitOrPatchRect(
        string? portraitPath,
        string? bandIcon,
        bool usePngIcons,
        Vector2 size,
        Texture? fallbackTexture = null)
    {
        Texture? texture = null;

        // Try portrait first if PNG mode is enabled
        if (usePngIcons && !string.IsNullOrEmpty(portraitPath))
        {
            texture = LoadPortraitTexture(portraitPath);
        }

        // Fall back to patch icon
        if (texture == null && !string.IsNullOrEmpty(bandIcon))
        {
            texture = LoadPatchTexture(bandIcon);
        }

        // Use fallback if both failed
        texture ??= fallbackTexture;

        return new TextureRect
        {
            Texture = texture,
            TextureScale = new Vector2(1f, 1f),
            Stretch = TextureRect.StretchMode.KeepAspectCentered,
            SetSize = size,
            HorizontalAlignment = HAlignment.Center,
            VerticalAlignment = VAlignment.Center,
            Modulate = Color.White,
        };
    }
}
