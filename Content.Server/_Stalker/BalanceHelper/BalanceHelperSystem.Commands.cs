using Content.Server.Administration;
using Content.Shared.Administration;
using Robust.Shared.Console;

namespace Content.Server._Stalker.BalanceHelper;

public sealed partial class BalanceHelperSystem
{
    [Dependency] private readonly IConsoleHost _consoleHost = default!;

    private void InitializeCommands()
    {
        _consoleHost.RegisterCommand(
            "st_balance:shoparmor",
            "Shows armor from traders. It can be copiend and pasted as CSV into Excel Prints output in server's console and logs",
            "st_balance:shoparmor >",
            PrintShopArmorCommand);

        _consoleHost.RegisterCommand(
            "st_balance:shopguns",
            "Shows guns from traders. It can be copiend and pasted as CSV into Excel Prints output in server's console and logs",
            "st_balance:shopguns >",
            PrintShopGunsCommand);

        _consoleHost.RegisterCommand(
            "st_balance:guns",
            "Shows all guns. It can be copiend and pasted as CSV into Excel Prints output in server's console and logs",
            "balance:armor >",
            PrintGunsCommand);

        _consoleHost.RegisterCommand(
            "st_balance:armor",
            "Shows all armor. It can be copiend and pasted as CSV into Excel Prints output in server's console and logs",
            "balance:armor >",
            PrintArmorCommand);
    }

    [AdminCommand(AdminFlags.Round)]
    private void PrintShopArmorCommand(IConsoleShell shell, string argstr, string[] args)
    {
        var armorReport = PrintShopArmor();
        shell.WriteLine(armorReport);
    }

    [AdminCommand(AdminFlags.Round)]
    private void PrintShopGunsCommand(IConsoleShell shell, string argstr, string[] args)
    {
        var gunsReport = PrintShopGuns();
        shell.WriteLine(gunsReport);
    }

    [AdminCommand(AdminFlags.Round)]
    private void PrintGunsCommand(IConsoleShell shell, string argstr, string[] args)
    {
        var gunsReport = PrintAllGuns();
        shell.WriteLine(gunsReport);
    }

    [AdminCommand(AdminFlags.Round)]
    private void PrintArmorCommand(IConsoleShell shell, string argstr, string[] args)
    {
        var armorReport = PrintAllArmor();
        shell.WriteLine(armorReport);
    }
}
