using System.Linq;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace Content.Server._Stalker.Procedural;

public sealed class DecalBlueprint
{

    public int MaxPosX = 0;
    public int MaxPosY = 0;

    public List<Cell> AllCells = new List<Cell>(0);
    private int NextRoomID = 0;

    public DecalBlueprint(int SizeX,int SizeY)
    {
        GenerateBox(SizeX,SizeY);
    }

    public void GenerateBox(int SizeX,int SizeY)
    {
        StageGenerageSize(SizeX,SizeY);
    }

    public void StageGenerageSize(int SizeX,int SizeY)
    {
        MaxPosX = SizeX;
        MaxPosY = SizeY;
        Console.WriteLine("StageGenerageSize");

        //Генерация начинаеться с левого нижнего угла
        int PosXCur = 0;
        int PosYCur = 0;

        for (;PosYCur < SizeY; PosYCur++)
        {
            for (;PosXCur < SizeX; PosXCur++)
            {
                AllCells.Add(new Cell(TypeCell.Empty,PosXCur,PosYCur));
            }
            PosXCur = 0;
        }
        Console.WriteLine("StageGenerageSize OK");
    }

}


public class Cell
{
    public TypeCell Type = TypeCell.Empty;
    public string Prototype = "EMPTY";
    public int PosX=0;
    public int PosY=0;
    public float RandomOffset = 0f;

    public void DebugPos()
    {
        Console.WriteLine("C X="+PosX+" Y="+PosY);
    }

    public Cell(TypeCell type, int posX, int posY)
    {
        Type = type;
        PosX = posX;
        PosY = posY;
    }

    public Cell(TypeCell type,string Text="Created empty cell")
    {
        Type = type;
        Console.WriteLine(Text);
    }
}

public enum TypeCell
{
    NotFind=-1,
    Empty=0,
    Corridor=1,
    Door=2,
    Wall=3,
    Room=4,
}

[System.Serializable]
public class OptionsDecal
{
    public List<ZoneDecal> ImportedDecalsFromJson = new List<ZoneDecal>(0);
    public DecalBlueprint DecalBlueprintCells = new DecalBlueprint(0,0);


    public int SumCells = 0;
    public float SumWeight = 0f;

    public void Install(string Json, int Xsize,int Ysize)
    {
        InstallJson(Json);
        CalculateZoneCellsInBox(Xsize,Ysize);
        InstallDecals();
        InstallBlueprint(Xsize,Ysize);
        FillCells();


    }

    private void InstallBlueprint(int Xsize,int Ysize)
    {
        Logger.Debug("InstallBlueprint");
        DecalBlueprintCells = new DecalBlueprint(Xsize, Ysize);
        Logger.Debug("InstallBlueprint Count="+DecalBlueprintCells.AllCells.Count);
    }

    void FillCells()
    {
        Random rndint = new Random();

        List<int> Indexes = new List<int>(0);
        for (int i = 0; i < DecalBlueprintCells.AllCells.Count; i++)
        {
            Indexes.Add(i);
        }



        foreach (var VARIABLE in ImportedDecalsFromJson)
        {
            Logger.Debug("Name="+VARIABLE.NameDecal+" MustSpawned="+VARIABLE.MustSpawned);
            while (VARIABLE.CurrentSpawned<VARIABLE.MustSpawned)
            {
                if (Indexes.Count>0)
                {
                    int RndIndex = rndint.Next(0, Indexes.Count);
                    DecalBlueprintCells.AllCells[Indexes[RndIndex]].Prototype = VARIABLE.NameDecal;
                    DecalBlueprintCells.AllCells[Indexes[RndIndex]].RandomOffset = VARIABLE.RandomOffset;
                    Indexes.RemoveAt(RndIndex);
                    VARIABLE.CurrentSpawned++;
                }
                else
                {
                    break;
                }

                //Logger.Debug("W AllCells.Count"+DecalBlueprintCells.AllCells.Count);

            }
        }



    }

    void InstallJson(string InputJson)
    {

    }

    void InstallDecals()
    {
        /*
        foreach (var OneDecal in ImportedDecalsFromJson.OrderBy(x => x.Layer))
        {
            Decals.Add(OneDecal.Layer,OneDecal);
        }
        */

        foreach (var VARIABLE in ImportedDecalsFromJson)
        {
            SumWeight += VARIABLE.Weight;
        }



        Logger.Debug("SumWeight ="+SumWeight);
        Logger.Debug("SumCells ="+SumCells);


        foreach (var VARIABLE in ImportedDecalsFromJson)
        {
            //(4000/(3+1))*3=
            VARIABLE.MustSpawned=(int)(((float)SumCells/SumWeight)*VARIABLE.Weight);
        }


    }

    void CalculateZoneCellsInBox(int Xsize,int Ysize)
    {
        int Result = 0;
        Result = Xsize * Ysize;
        SumCells = Result;
    }

}

[System.Serializable]
public class ZoneDecal
{
    public string NameDecal { get; set; } = "";
    public float Weight { get; set; } = 0f;
    public float RandomOffset { get; set; } = 0f;
    public int Layer = 0;
    public int CurrentSpawned = 0;
    public int MustSpawned = 0;

}
