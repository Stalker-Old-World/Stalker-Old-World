using Content.Server.Administration;
using Content.Server.Database;
using Content.Server.Preferences.Managers;
using Content.Shared.Administration;
using Content.Shared.Humanoid.Markings;
using Content.Shared.Preferences;
using Robust.Shared.Console;
using Robust.Shared.Player;

namespace Content.Server._Stalker.Character;

public sealed class STCharacterSystem : EntitySystem
{
    [Dependency] private readonly IConsoleHost _consoleHost = default!;
    [Dependency] private readonly IServerDbManager _serverDb = default!;
    [Dependency] private readonly ISharedPlayerManager _playerManager = default!;
    [Dependency] private readonly IServerPreferencesManager _serverPreferences = default!;

    public override void Initialize()
    {
        base.Initialize();

        _consoleHost.RegisterCommand("st_character_add_marking",
            Loc.GetString("st-character-add-marking"),
            "st_character_add_marking <username> <slot> <markingId> [colors...]",
            OoCharacterAddMarkingCommand);
    }

    [AdminCommand(AdminFlags.Host)]
    private void OoCharacterAddMarkingCommand(IConsoleShell shell, string argstr, string[] args)
    {
        if (args.Length < 3)
        {
            shell.WriteError(Loc.GetString("shell-wrong-arguments-number"));
            return;
        }

        if (!_playerManager.TryGetUserId(args[0], out var netUserId))
        {
            shell.WriteError(Loc.GetString("shell-target-player-does-not-exist "));
            return;
        }

        if (!int.TryParse(args[1], out var slot))
        {
            shell.WriteError(Loc.GetString("shell-invalid-int"));
            return;
        }

        var markingId = args[2];

        var colors = new List<Color>();
        for (var i = 3; i < args.Length; i++)
        {
            if (!Color.TryParse(args[i], out var color))
            {
                shell.WriteError(Loc.GetString("shell-invalid-color"));
                continue;
            }

            colors.Add(color);
        }

        var prefs = _serverPreferences.GetPreferences(netUserId);
        var profile = prefs.Characters[slot];

        if (profile is not HumanoidCharacterProfile prof)
            return;

        var marking = new Marking(markingId, colors);
        prof.Appearance.Markings.Add(marking);

        _serverDb.SaveCharacterSlotAsync(netUserId, prof, slot);
    }
}
