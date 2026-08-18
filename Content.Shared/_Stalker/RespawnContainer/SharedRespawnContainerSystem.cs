using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Content.Shared.Administration.Logs;
using Content.Shared.Database;
using Robust.Shared.Player;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;
using Robust.Shared.Utility;

namespace Content.Shared._Stalker.RespawnContainer;

// TODO: Add components saving, to bring them with data at once, i think that will be pretty good
public abstract class SharedRespawnContainerSystem : EntitySystem
{
    [Dependency] private readonly ISharedAdminLogManager _adminLogger = default!;

    #region Get

    public bool TryGetData<T>(
        EntityUid uid,
        string record,
        [NotNullWhen(true)] out T? data,
        RespawnContainerComponent? component = null)
    {
        data = default;
        return Resolve(uid, ref component) && TryGetData<T>(record, component, out data);
    }

    public bool TryGetData<T>(string record, RespawnContainerComponent component, [NotNullWhen(true)] out T? data)
    {
        data = default;
        if (!component.Data.TryGetValue(record, out var objData))
            return false;

        if (objData is not T tData)
            return false;

        data = tData;
        return true;
    }

    public bool TryGetData<T>(Entity<RespawnContainerComponent> entity, string record,
        [NotNullWhen(true)] out T? data)
    {
        return TryGetData(record, entity.Comp, out data);
    }

    public T GetData<T>(string record, RespawnContainerComponent component)
    {
        // Джери еблан нельзятак делать, какой нахуй Debug
        DebugTools.Assert(component.Data.TryGetValue(record, out _));
        return (T)component.Data[record];
    }

    #endregion

    #region Set

    public bool TrySetData(EntityUid uid, string record, object value, RespawnContainerComponent? comp = null)
    {
        return Resolve(uid, ref comp) && TrySetData((uid, comp), record, value);
    }

    public bool TrySetData(Entity<RespawnContainerComponent> entity, string record, object value)
    {
        if (!entity.Comp.Data.ContainsKey(record))
            return false;

        entity.Comp.Data[record] = value;
        RaiseLocalEvent(entity.Owner, new RespawnDataUpdated());
        return true;
    }

    #endregion

    #region Transfer

    /// <summary>
    /// Transfers respawnContainer from ent to uid
    /// </summary>
    /// <param name="ent">Entity to transfer from</param>
    /// <param name="uid">Entity to transfer to</param>
    public void TransferTo(Entity<RespawnContainerComponent> ent, EntityUid uid)
    {
        if (!Exists(uid))
            return;

        var newComp = EnsureComp<RespawnContainerComponent>(uid);

        // Transfer all data
        DataTransfer(ent.Comp, newComp);

        if (TryComp<ActorComponent>(ent, out var actor))
        {
            _adminLogger.Add(LogType.Respawn,
                LogImpact.Low, $"{actor.PlayerSession.Name}'s respawn container has been transferred to {ToPrettyString(uid)}");
        }

        var ev = new RespawnGotTransferredEvent(GetNetEntity(ent.Owner));
        RaiseLocalEvent(uid, ev);
    }
    private void DataTransfer(RespawnContainerComponent old, RespawnContainerComponent newComp)
    {
        // Add your data here
        newComp.Data = old.Data;

        // TODO: Maybe a little cleanup on transfer, like, clearing old values in old comp
    }
    public void Visit(ICommonSession session, EntityUid newEnt)
    {
        if (session.AttachedEntity == null)
            return;

        if (!TryComp<RespawnContainerComponent>(session.AttachedEntity.Value, out var respComp))
            return;

        TransferTo((session.AttachedEntity.Value, respComp), newEnt);
    }

    public void UnVisit(ICommonSession session, EntityUid? newEnt)
    {
        if (session.AttachedEntity == null || newEnt == null)
            return;

        if (!TryComp<RespawnContainerComponent>(session.AttachedEntity.Value, out var respComp))
            return;

        TransferTo((session.AttachedEntity.Value, respComp), newEnt.Value);

        // Cleanup an old RespawnContainer
        RemComp<RespawnContainerComponent>(session.AttachedEntity.Value);
        EnsureComp<RespawnContainerComponent>(session.AttachedEntity.Value);
    }

    #endregion

    #region Ensure

    public T EnsureData<T>(Entity<RespawnContainerComponent> entity, string record, T value)
    {
        return EnsureData(entity.Owner, record, value);
    }

    public T EnsureData<T>(EntityUid uid, string record, T defaultValue)
    {
        var comp = EnsureComp<RespawnContainerComponent>(uid);
        return EnsureData<T>(comp, record, defaultValue);
    }

    public T EnsureData<T>(RespawnContainerComponent comp, string record, T defaultValue)
    {
        if (TryGetData<T>(record, comp, out var data))
            return data;

        if (defaultValue == null)
            return default!;

        comp.Data.Add(record, defaultValue);
        return GetData<T>(record, comp);
    }

    #endregion
}

#region Events

/// <summary>
/// Raise straight on an new entity, old entity can be deleted or in terminating, be careful
/// </summary>
public sealed class RespawnGotTransferredEvent : EntityEventArgs
{
    public NetEntity? OldEntity;

    public RespawnGotTransferredEvent(NetEntity? oldEntity)
    {
        OldEntity = oldEntity;
    }
}

/// <summary>
/// Raised directly at entity, which has been updated
/// </summary>
public sealed class RespawnDataUpdated : EntityEventArgs
{
}

#endregion

