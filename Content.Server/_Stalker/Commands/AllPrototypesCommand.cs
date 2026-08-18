using Content.Server.Administration;
using Content.Shared.Administration;
using Robust.Shared.Console;
using Robust.Shared.Prototypes;

namespace Content.Server._Stalker.Commands;

[AdminCommand(AdminFlags.Host)]
public sealed class AllPrototypesCommand : IConsoleCommand
{
    public string Command => "all_prototype";
    public string Description => "UwU";
    public string Help => "all_prototype <id>";

    public void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        var prototypeManager = IoCManager.Resolve<IPrototypeManager>();

        foreach (var prototype in prototypeManager.EnumeratePrototypes(args[0]))
        {
            shell.WriteLine(prototype.ID);
        }
    }
}
