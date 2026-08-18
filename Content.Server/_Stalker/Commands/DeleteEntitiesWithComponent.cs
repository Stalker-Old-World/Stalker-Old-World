using System.Linq;
using Content.Shared.Administration;
using Robust.Shared.Console;

namespace Content.Server.Administration.Commands
{
    [AdminCommand(AdminFlags.Spawn)]
    public sealed class DeleteEntitiesWithComponent : IConsoleCommand
    {
        public string Command => "deleteentitieswithcomponent";
        public string Description => "Delete all entities with component.";
        public string Help => $"Usage: {Command} <name>";

        public void Execute(IConsoleShell shell, string argStr, string[] args)
        {
            switch (args.Length)
            {
                case 0:
                    shell.WriteLine($"Not enough arguments.\n{Help}");
                    break;
                default:
                    var name = string.Join(" ", args);
                    var componentFactory = IoCManager.Resolve<IComponentFactory>();
                    var entityManager = IoCManager.Resolve<IEntityManager>();

                    if (!componentFactory.TryGetRegistration(name, out var registration))
                    {
                        shell.WriteLine($"No component exists with name {name}.");
                        break;
                    }


                    var Ents = entityManager.GetEntities();
                    name += "Component";


                    int DeletedComponentsCount=0;
                    foreach (var VARIABLE in Ents)
                    {
                        foreach (var component in entityManager.GetComponents(VARIABLE))
                        {
                            string Cname = component.GetType().ToString();
                            Cname = Cname.Split('.').Last();
                            if (Cname==name)
                            {
                                entityManager.DeleteEntity(VARIABLE);
                                DeletedComponentsCount++;
                            }
                        }
                    }
                    shell.WriteLine("Deleted ents count "+DeletedComponentsCount+" with component name =\""+name+"\"");
                    break;
            }
        }
    }
}
