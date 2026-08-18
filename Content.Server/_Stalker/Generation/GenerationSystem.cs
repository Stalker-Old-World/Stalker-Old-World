using Content.Server.Administration;
using Content.Shared.Administration;
using Robust.Server.GameObjects;
using Robust.Shared.Console;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;

namespace Content.Server._Stalker.Generation;

public sealed class GenerationSystem : EntitySystem
{
    [Dependency] private readonly IConsoleHost _console = default!;
    [Dependency] private readonly IMapManager _mapManager = default!;
    [Dependency] private readonly MapSystem _map = default!;

    public override void Initialize()
    {
        base.Initialize();

        _console.RegisterCommand("clone", Loc.GetString("cmd-clone-desc"), Loc.GetString("cmd-clone-help"), Clone);
    }

    [AdminCommand(AdminFlags.Mapping)]
    public void Clone(IConsoleShell shell, string argstr, string[] args)
    {
        if (!int.TryParse(args[0], out var sourceMap))
            return;

        if (!int.TryParse(args[1], out var sourceLeft))
            return;

        if (!int.TryParse(args[2], out var sourceRight))
            return;

        if (!int.TryParse(args[3], out var sourceTop))
            return;

        if (!int.TryParse(args[4], out var sourceBottom))
            return;

        if (!int.TryParse(args[5], out var targetMap))
            return;

        if (!int.TryParse(args[6], out var targetOffsetX))
            return;

        if (!int.TryParse(args[7], out var targetOffsetY))
            return;

        Clone(new MapId(sourceMap), new Box2i(sourceLeft, sourceRight, sourceTop, sourceBottom), new MapId(targetMap), new Vector2i(targetOffsetX, targetOffsetY));
    }

    public void Clone(MapId sourceMap, Box2i sourceBox, MapId targetMap, Vector2i targetOffset)
    {
        var sourceEntity = _mapManager.GetMapEntityId(sourceMap);
        var sourceGrid = EnsureComp<MapGridComponent>(sourceEntity);

        var targetEntity = _mapManager.GetMapEntityId(targetMap);
        var targetGrid = EnsureComp<MapGridComponent>(targetEntity);

        for (var x = sourceBox.Left; x < sourceBox.Right; x++)
        {
            for (var y = sourceBox.Bottom; y < sourceBox.Top; y++)
            {
                var index = new Vector2i(x, y);
                if (!_map.TryGetTile(sourceGrid, index, out var tile))
                    continue;

                _map.SetTile(targetEntity, targetGrid, index + targetOffset, tile);
            }
        }
    }
}
