using Content.Server.Administration;
using Content.Server.Database;
using Content.Server.Preferences.Managers;
using Content.Shared.Administration;
using Content.Shared.Preferences;
using Robust.Shared.Console;
using Robust.Shared.Player;

namespace Content.Server._Stalker.Commands;

//[AdminCommand(AdminFlags.Admin)]
[AnyCommand]
public sealed class SetCharacterChangeable : IConsoleCommand
{
    public string Command => "st_set_character_changeable";
    public string Description => "";
    public string Help => "set_character_changeable <username> <changeable> <slot>";

    public void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        if (args.Length is < 1 or > 3)
        {
            shell.WriteError(Loc.GetString("shell-need-between-arguments",("lower", 1), ("upper", 3)));
            return;
        }

        var playerManager = IoCManager.Resolve<ISharedPlayerManager>();
        if (!playerManager.TryGetUserId(args[0], out var netUserId))
        {
            shell.WriteError(Loc.GetString("shell-target-player-does-not-exist "));
            return;
        }

        if (!bool.TryParse(args[1], out var changeable))
        {
            shell.WriteError(Loc.GetString("shell-invalid-bool"));
            return;
        }

        if (!int.TryParse(args[2], out var slot))
        {
            shell.WriteError(Loc.GetString("shell-invalid-int"));
            return;
        }

        var dbMan = IoCManager.Resolve<IServerDbManager>();
        dbMan.SaveCharacterChangeable(netUserId, changeable, slot);

        // TODO: Update the cached preference
        var prefManager = IoCManager.Resolve<IServerPreferencesManager>();
        var prefs = prefManager.GetPreferences(netUserId);
        var profile = prefs.Characters[slot];
        if (profile is not HumanoidCharacterProfile prof)
            return;
        prof.Changeable = changeable;
    }
}
