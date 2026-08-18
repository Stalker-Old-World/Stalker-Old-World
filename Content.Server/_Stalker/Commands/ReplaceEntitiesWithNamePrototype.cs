using System.Linq;
using Content.Shared.Administration;
using Robust.Shared.Console;

namespace Content.Server.Administration.Commands
{
    [AdminCommand(AdminFlags.Spawn)]
    public sealed class ReplaceEntitiesWithNamePrototype : IConsoleCommand
    {

        public string Command => "replaceentitieswithnameprototype";
        public string Description => "Replace all entities with name prototype by name prototype.";
        public string Help => $"Usage: {Command} <name>";

        public void Execute(IConsoleShell shell, string argStr, string[] args)
        {
            switch (args.Length)
            {
                case 0:
                    shell.WriteLine($"Not enough arguments.\n{Help}");
                    break;
                default:
                    if (args.Length!=2)
                    {
                        shell.WriteLine($"Arguments must be 2, now args="+args.Length);
                        return;
                    }

                    var name = args[0];
                    var NeedSpawn = args[1];
                    shell.WriteLine($"name="+name);
                    shell.WriteLine($"NeedSpawn="+NeedSpawn);

                    var componentFactory = IoCManager.Resolve<IComponentFactory>();
                    var entityManager = IoCManager.Resolve<IEntityManager>();

                    var Ents = entityManager.GetEntities();


                    int ReplacedComponentsCount=0;
                    List<EntityUid> FindedEnts = new List<EntityUid>(0);


                    foreach (var VARIABLE in Ents)
                    {
                        if (entityManager.TryGetComponent(VARIABLE, out MetaDataComponent? MComponent))
                        {
                            if (MComponent.EntityPrototype != null)
                            {
                                if (MComponent.EntityPrototype.ID == name)
                                {
                                    if (entityManager.TryGetComponent(VARIABLE, out TransformComponent? TComponent))
                                    {
                                        FindedEnts.Add(VARIABLE);
                                    }
                                }
                            }
                        }
                    }

                    foreach (var VARIABLE in FindedEnts)
                    {
                        if (entityManager.TryGetComponent(VARIABLE, out TransformComponent? TComponent))
                        {
                            var coords = TComponent.Coordinates;
                            entityManager.SpawnEntity(NeedSpawn,coords);
                            entityManager.DeleteEntity(VARIABLE);
                            ReplacedComponentsCount++;
                        }
                    }

                    shell.WriteLine("Replaced ents count "+ReplacedComponentsCount+". \""+name+"\" Changed to \""+name+"\"");
                    break;
            }
        }
    }
}
