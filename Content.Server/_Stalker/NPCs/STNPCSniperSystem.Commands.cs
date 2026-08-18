using Content.Server.Administration;
using Content.Shared.Administration;
using Robust.Shared.Console;

namespace Content.Server._Stalker.NPCs;

public sealed partial class STNPCSniperSystem
{
    private void InitializeCommands()
    {
        _consoleHost.RegisterCommand(
            "st_sniper_regenerate_map",
            Loc.GetString("st_sniper_regenerate_map"),
            "st_sniper_regenerate_map",
            RegenerateMapCommand);
    }

    [AdminCommand(AdminFlags.Host)]
    private void RegenerateMapCommand(IConsoleShell shell, string argstr, string[] args)
    {
        RegenerateMap();
    }
}
