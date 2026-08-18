using System.Linq;
using Content.Shared.Administration;
using Robust.Shared.Console;

namespace Content.Server.Administration.Commands
{
    [AdminCommand(AdminFlags.Spawn)]
    public sealed class DeleteEntitiesWithNamePrototype : IConsoleCommand
    {
        public string Command => "deleteentitieswithnameprototype";
        public string Description => "Delete all entities with name prototype.";
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

                    var Ents = entityManager.GetEntities();


                    int DeletedComponentsCount=0;
                    foreach (var VARIABLE in Ents)
                    {
                        if (entityManager.TryGetComponent(VARIABLE, out MetaDataComponent? MComponent))
                        {
                            if (MComponent.EntityPrototype != null)
                            {
                                if (MComponent.EntityPrototype.ID == name)
                                {
                                    entityManager.DeleteEntity(VARIABLE);
                                    DeletedComponentsCount++;
                                }
                            }
                        }
                    }
                    shell.WriteLine("Deleted ents count "+DeletedComponentsCount+" with name prototype=\""+name+"\"");
                    break;
            }
        }
    }
}
