using System.Linq;
using Content.Shared.Administration;
using Robust.Shared.Console;

namespace Content.Server.Administration.Commands
{
    [AdminCommand(AdminFlags.Spawn)]
    public sealed class AllEntsInfo : IConsoleCommand
    {
        public string Command => "allentsinfo";
        public string Description => "All Ents Info.";
        public string Help => $"Usage: {Command} <name>";
        int tableWidth = 180;

        public void Execute(IConsoleShell shell, string argStr, string[] args)
        {
            switch (args.Length)
            {
                case 0:
                    shell.WriteLine($"allentsinfo scan start");
                    var name = string.Join(" ", args);
                    var componentFactory = IoCManager.Resolve<IComponentFactory>();
                    var entityManager = IoCManager.Resolve<IEntityManager>();

                    var Ents = entityManager.GetEntities();


                    int AllEnts=0;
                    Dictionary<string, int> AllEntsDictionary = new Dictionary<string, int>(0);
                    foreach (var VARIABLE in Ents)
                    {
                        if (entityManager.TryGetComponent(VARIABLE, out MetaDataComponent? MComponent))
                        {
                            if (MComponent.EntityPrototype != null)
                            {
                                Console.WriteLine("MComponent.EntityPrototype="+MComponent.EntityPrototype.ID);
                                if (AllEntsDictionary.ContainsKey(""+MComponent.EntityPrototype.ID)==true)
                                {
                                    AllEntsDictionary[""+MComponent.EntityPrototype.ID]++;
                                }
                                else
                                {
                                    AllEntsDictionary.Add(""+MComponent.EntityPrototype.ID,1);
                                }
                                AllEnts++;
                            }
                        }
                    }


                    var ordered = AllEntsDictionary.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
                    List<DataConsoleSS14?> DataForConsole = new List<DataConsoleSS14?>(0);

                    float AllProcent=0f;
                    foreach (var VARIABLE in ordered)
                    {
                        float Procent = (VARIABLE.Value) / ((float)AllEnts / 100f);
                        //shell.WriteLine("["+VARIABLE.Key+"]="+VARIABLE.Value+" "+Procent+"%");
                        DataForConsole.Add(new DataConsoleSS14(VARIABLE.Key,VARIABLE.Value,Procent+"%"));
                        AllProcent += Procent;
                    }

                    //DataForConsole

                    PrintRow(shell,"Name prototype", "Count", "Procent");
                    foreach (var VARIABLE in DataForConsole)
                    {
                        if (VARIABLE!=null)
                        {
                            PrintRow(shell,""+VARIABLE.Name, ""+VARIABLE.Count, ""+VARIABLE.Procent);
                        }

                    }
                    PrintRow(shell,"########## Name prototype ##########", "## Count##", "## Procent##");






                    shell.WriteLine("Info AllProcent="+AllProcent+" AllEnts count="+AllEnts);



                    break;
            }




        }


        void PrintRow(IConsoleShell shell,params string[] columns)
        {
           // int width = (tableWidth - columns.Length) / columns.Length;
            string row = "|";


            for (int i = 0; i < columns.Length; i++)
            {
                if (i==0)
                {
                    row += AlignCentre(columns[i], 70) + "|";
                }
                if (i==1)
                {
                    row += AlignCentre(columns[i], 10) + "|";
                }
                if (i==2)
                {
                    row += AlignCentre(columns[i], 15) + "|";
                }
            }

            /*
            foreach (string column in columns)
            {
                row += AlignCentre(column, width) + "|";
            }
            */

            shell.WriteLine(row);
        }


        string AlignCentre(string text, int width)
        {
            text = text.Length > width ? text.Substring(0, width - 3) + "..." : text;

            if (string.IsNullOrEmpty(text))
            {
                return new string(' ', width);
            }
            else
            {
                return text.PadRight(width - (width - text.Length) / 2).PadLeft(width);
            }
        }

        // Define a class for your table data
        public class DataConsoleSS14
        {
            public DataConsoleSS14(string name, int count, string procent)
            {
                Name = name;
                Count = count;
                Procent = procent;
            }

            public string Name { get; set; }= "";
            public int Count { get; set; } = 0;
            public string Procent { get; set; } = "";
        }
    }
}
