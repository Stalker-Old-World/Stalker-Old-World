using Content.Server.Administration;
using Content.Server.Database;
using Content.Server.EUI;
using Content.Shared._Stalker_EN.Camera;
using Content.Shared.Administration;
using Robust.Shared.Console;
using Robust.Shared.Player;

namespace Content.Server._Stalker_EN.Camera.Commands;

/// <summary>
/// Admin command that opens a photo preview window by PhotoId.
/// Searches in-world entities first, then falls back to the news database.
/// </summary>
[AdminCommand(AdminFlags.Admin)]
public sealed class STPhotoPreviewCommand : IConsoleCommand
{
    [Dependency] private readonly IEntityManager _entities = default!;
    [Dependency] private readonly IServerDbManager _dbManager = default!;
    [Dependency] private readonly EuiManager _euiManager = default!;

    public string Command => "st_photo_preview";
    public string Description => Loc.GetString("cmd-st-photo-preview-desc");
    public string Help => Loc.GetString("cmd-st-photo-preview-help");

    public void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        var player = shell.Player;
        if (player == null)
        {
            shell.WriteError(Loc.GetString("shell-cannot-run-command-from-server"));
            return;
        }

        if (args.Length != 1)
        {
            shell.WriteError(Loc.GetString("cmd-st-photo-preview-error-args"));
            return;
        }

        if (!Guid.TryParse(args[0], out var photoId))
        {
            shell.WriteError(Loc.GetString("cmd-st-photo-preview-error-guid"));
            return;
        }

        // Try in-world entities first
        var query = _entities.EntityQueryEnumerator<STPhotoComponent>();
        while (query.MoveNext(out _, out var photo))
        {
            if (photo.PhotoId != photoId || photo.ImageData.Length == 0)
                continue;

            _euiManager.OpenEui(new STAdminPhotoPreviewEui(photoId, photo.ImageData), player);
            return;
        }

        // Fall back to news database
        LookupDbAsync(shell, player, photoId);
    }

    private async void LookupDbAsync(
        IConsoleShell shell,
        ICommonSession player,
        Guid photoId)
    {
        try
        {
            var dbPhoto = await _dbManager.GetStalkerNewsArticlePhotoAsync(photoId);
            if (dbPhoto == null || dbPhoto.PhotoData.Length == 0)
            {
                shell.WriteError(Loc.GetString("cmd-st-photo-preview-not-found", ("photoId", photoId)));
                return;
            }

            _euiManager.OpenEui(new STAdminPhotoPreviewEui(photoId, dbPhoto.PhotoData), player);
        }
        catch (Exception e)
        {
            shell.WriteError(Loc.GetString("cmd-st-photo-preview-error-db", ("error", e.Message)));
        }
    }
}
