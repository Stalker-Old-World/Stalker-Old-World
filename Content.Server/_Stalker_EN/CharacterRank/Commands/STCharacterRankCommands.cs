using System.Linq;
using Content.Server.Administration;
using Content.Shared._Stalker_EN.CharacterRank;
using Content.Shared.Administration;
using Robust.Server.Player;
using Robust.Shared.Console;
using Robust.Shared.Player;

namespace Content.Server._Stalker_EN.CharacterRank.Commands;

/// <summary>
/// Gets the rank info of a player's current character.
/// </summary>
[AdminCommand(AdminFlags.Moderator)]
public sealed class STRankGetCommand : LocalizedCommands
{
    [Dependency] private readonly IPlayerManager _players = default!;
    [Dependency] private readonly IEntityManager _entities = default!;

    public override string Command => "strank_get";

    public override void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        if (args.Length != 1)
        {
            shell.WriteError(Loc.GetString("cmd-strank-error-args"));
            return;
        }

        var session = _players.Sessions
            .FirstOrDefault(s => s.Name.Equals(args[0], StringComparison.OrdinalIgnoreCase));

        if (session?.AttachedEntity is not { } uid)
        {
            shell.WriteError(Loc.GetString("cmd-strank-error-no-entity"));
            return;
        }

        if (!_entities.TryGetComponent<STCharacterRankComponent>(uid, out var comp))
        {
            shell.WriteError(Loc.GetString("cmd-strank-error-no-component"));
            return;
        }

        var rankSystem = _entities.System<STCharacterRankSystem>();
        rankSystem.FlushTracking(uid);

        var data = rankSystem.GetTrackingData(uid);
        var time = data?.AccumulatedTime ?? comp.AccumulatedTime;
        var charName = data?.CharacterName ?? Loc.GetString("cmd-strank-error-unknown-character");

        shell.WriteLine(Loc.GetString("cmd-strank-get-success",
            ("username", session.Name),
            ("character", charName),
            ("rank", Loc.GetString(comp.RankName)),
            ("index", comp.RankIndex),
            ("time", $"{time.TotalHours:F1}h")));
    }

    public override CompletionResult GetCompletion(IConsoleShell shell, string[] args)
    {
        if (args.Length == 1)
        {
            var options = _players.Sessions.Select(s => s.Name).OrderBy(s => s);
            return CompletionResult.FromHintOptions(options, Loc.GetString("cmd-strank_get-help"));
        }

        return CompletionResult.Empty;
    }
}

/// <summary>
/// Transfers accumulated rank hours from an old character name to the player's current character.
/// </summary>
[AdminCommand(AdminFlags.Admin)]
public sealed class STRankTransferCommand : LocalizedCommands
{
    [Dependency] private readonly IPlayerManager _players = default!;
    [Dependency] private readonly IEntityManager _entities = default!;

    public override string Command => "strank_transfer";

    public override void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        if (args.Length < 2)
        {
            shell.WriteError(Loc.GetString("cmd-strank-error-args"));
            return;
        }

        var session = _players.Sessions
            .FirstOrDefault(s => s.Name.Equals(args[0], StringComparison.OrdinalIgnoreCase));

        if (session?.AttachedEntity is not { } uid)
        {
            shell.WriteError(Loc.GetString("cmd-strank-error-no-entity"));
            return;
        }

        if (!_entities.HasComponent<STCharacterRankComponent>(uid))
        {
            shell.WriteError(Loc.GetString("cmd-strank-error-no-component"));
            return;
        }

        var oldCharacterName = string.Join(" ", args[1..]);
        var rankSystem = _entities.System<STCharacterRankSystem>();
        rankSystem.TransferRankAsync(uid, oldCharacterName, session.Name, shell);
    }

    public override CompletionResult GetCompletion(IConsoleShell shell, string[] args)
    {
        if (args.Length == 1)
        {
            var options = _players.Sessions.Select(s => s.Name).OrderBy(s => s);
            return CompletionResult.FromHintOptions(options, Loc.GetString("cmd-strank_transfer-help"));
        }

        return CompletionResult.Empty;
    }
}

/// <summary>
/// Sets a player's character rank by name, auto-setting the required hours.
/// </summary>
[AdminCommand(AdminFlags.Admin)]
public sealed class STRankSetCommand : LocalizedCommands
{
    [Dependency] private readonly IPlayerManager _players = default!;
    [Dependency] private readonly IEntityManager _entities = default!;

    public override string Command => "strank_set";

    public override void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        if (args.Length != 2)
        {
            shell.WriteError(Loc.GetString("cmd-strank-error-args"));
            return;
        }

        var session = _players.Sessions
            .FirstOrDefault(s => s.Name.Equals(args[0], StringComparison.OrdinalIgnoreCase));

        if (session?.AttachedEntity is not { } uid)
        {
            shell.WriteError(Loc.GetString("cmd-strank-error-no-entity"));
            return;
        }

        if (!_entities.HasComponent<STCharacterRankComponent>(uid))
        {
            shell.WriteError(Loc.GetString("cmd-strank-error-no-component"));
            return;
        }

        var rankSystem = _entities.System<STCharacterRankSystem>();
        var rank = rankSystem.GetRankByName(args[1]);

        if (rank == null)
        {
            shell.WriteError(Loc.GetString("cmd-strank-error-invalid-rank"));
            return;
        }

        rankSystem.SetAccumulatedTime(uid, rank.TimeRequired);

        shell.WriteLine(Loc.GetString("cmd-strank-set-success",
            ("username", session.Name),
            ("rank", Loc.GetString(rank.Name)),
            ("index", rank.Index)));
    }

    public override CompletionResult GetCompletion(IConsoleShell shell, string[] args)
    {
        if (args.Length == 1)
        {
            var options = _players.Sessions.Select(s => s.Name).OrderBy(s => s);
            return CompletionResult.FromHintOptions(options, Loc.GetString("cmd-strank_set-help"));
        }

        if (args.Length == 2)
        {
            var rankSystem = _entities.System<STCharacterRankSystem>();
            var ranks = rankSystem.GetAllRanks();
            var options = ranks.Select(r =>
                new CompletionOption(r.Name, $"{Loc.GetString(r.Name)} ({r.TimeRequired.TotalHours}h)"));
            return CompletionResult.FromHintOptions(options, Loc.GetString("cmd-strank_set-help"));
        }

        return CompletionResult.Empty;
    }
}
