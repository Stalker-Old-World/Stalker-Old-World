using Content.Shared._Stalker.ClownScream;
using Robust.Client.GameObjects;
using Robust.Client.Player;
using Robust.Shared.Utility;

namespace Content.Client._Stalker.ClownScream;

//TODO: Probably all system should be moved to client
public sealed class ClownScreamSystem : SharedClownScreamSystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeNetworkEvent<ToggleClownScreamMessage>(OnToggle);
    }

    private void OnToggle(ToggleClownScreamMessage ev)
    {
        var entity = GetEntity(ev.Entity);

        if (!TryComp<SpriteComponent>(entity, out var sprite))
            return;


        Toggle(entity, sprite, ev.Sprite, ev.Enable);
    }

    private void Toggle(EntityUid player, SpriteComponent sprite, SpriteSpecifier texture, bool enable)
    {
        if (!enable)
        {
            if (!sprite.LayerMapTryGet(ClownScreamKey.Key, out var layer))
                return;

            sprite.RemoveLayer(layer);
        }
        else
        {
            if (sprite.LayerMapTryGet(ClownScreamKey.Key, out _))
                return;

            var layer = sprite.AddLayer(texture);
            sprite.LayerMapSet(ClownScreamKey.Key, layer);
        }

        Dirty(player, sprite);
    }
    private enum ClownScreamKey
    {
        Key,
    }
}
