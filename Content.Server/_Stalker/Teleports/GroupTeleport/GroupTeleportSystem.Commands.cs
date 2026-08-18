using Content.Server.Administration;
using Content.Shared._Stalker.Teleport;
using Content.Shared.Administration;
using Content.Shared.Anomaly.Components;
using Robust.Shared.Console;

namespace Content.Server._Stalker.Teleports.GroupTeleport;

public sealed partial class GroupTeleportSystem
{
    [Dependency] private readonly IConsoleHost _consoleHost = default!;

    public void InitializeCommands()
    {
        _consoleHost.RegisterCommand("reload_grouped", "Reloads grouped portals, for debug purpose.", "reload_grouped", ReloadGroupsCommand);
    }
    [AdminCommand(AdminFlags.Debug)]
    private void ReloadGroupsCommand(IConsoleShell shell, string argstr, string[] args)
    {
        _cachedPortals.Clear();
        var query = EntityQueryEnumerator<GroupTeleportComponent>();
        while (query.MoveNext(out var uid, out var group))
        {
            if (!_cachedPortals.ContainsKey(group.Group))
                _cachedPortals.Add(group.Group, new HashSet<EntityUid>());
            _cachedPortals[group.Group].Add(uid);
        }
        shell.WriteLine("Successfully reloaded group portals");
    }
}
