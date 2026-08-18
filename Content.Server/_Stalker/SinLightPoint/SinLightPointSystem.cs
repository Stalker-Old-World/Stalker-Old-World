using Content.Shared.Mobs.Systems;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Timing;
using Content.Server.Discord;
using Robust.Shared.Configuration;
using Content.Shared.CCVar;
using Content.Shared._Stalker.CCCCVars;

namespace Content.Server._Stalker.SinLightPoint;

public sealed partial class SinLightPointSystem : EntitySystem
{
    [Dependency] private readonly EntityLookupSystem _lookup = default!;
    [Dependency] private readonly SharedPointLightSystem _pointLight = default!;
    [Dependency] private readonly SharedAudioSystem _audio = default!;
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly MobStateSystem _mobState = default!;
    [Dependency] private readonly DiscordWebhook _discord = default!;
    [Dependency] private readonly IConfigurationManager _configurationManager = default!;
    private WebhookIdentifier? _webhookIdentifier;

    public override void Initialize()
    {
        _configurationManager.OnValueChanged(CCCCVars.DiscordSinLightMessageWebhook, value =>
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                _discord.GetWebhook(value, data => _webhookIdentifier = data.ToIdentifier());
            }
        }, true);
    }

    private readonly TimeSpan _updateDelay = TimeSpan.FromSeconds(40f);
    private TimeSpan _updateNext;

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        if (_updateNext > _timing.CurTime)
            return;

        _updateNext = _timing.CurTime + _updateDelay;

        var query = EntityQueryEnumerator<SinAlarmPointComponent, TransformComponent>();
        while (query.MoveNext(out var uid, out var point, out var transform))
        {
            var entity = (uid, point);
            var newCount = GetTargetCount(entity);

            if (point.Count != newCount)
                SetCount(entity, newCount);
        }
    }

    private void SetCount(Entity<SinAlarmPointComponent> point, int count)
    {
        point.Comp.Count = count;

        var lightPoints = GetLightPoints(point);
        foreach (var lightPoint in lightPoints)
        {
            if (!TryComp<SinLightPointComponent>(lightPoint, out var sinLightPoint))
                return;

            if (!_pointLight.TryGetLight(lightPoint, out var light))
                continue;

            _pointLight.SetEnergy(lightPoint, point.Comp.Count * 30f, light);
            _audio.PlayPvs(sinLightPoint.Sound, lightPoint);
            SendMessageDiscordMessage(count, point.Comp.Side);

        }
    }

    private async void SendMessageDiscordMessage(int people, string location)
    {
        if (people == 0)
            return;
        try
        {
            if (_webhookIdentifier is null)
                return;
// - main server greh role
//<@&1224720139577069730> - test server greh role
            var mentions = new WebhookMentions();
            mentions.AllowRoleMentions();
            var payload = new WebhookPayload
            {
                Content = $"<@&1218533341691772958>\n`{people} людей сейчас на {location}.`",
                AllowedMentions = mentions,
            };
            Log.Info(payload.Content);
            var res = await _discord.CreateMessage(_webhookIdentifier.Value, payload);

            await _discord.CreateMessage(_webhookIdentifier.Value, payload);
        }
        catch (Exception e)
        {
            Log.Error($"Error while sending discord sin light message:\n{e}");
        }
    }


    private HashSet<EntityUid> GetLightPoints(Entity<SinAlarmPointComponent> point)
    {
        if (point.Comp.LightPoints.Count != 0)
            return point.Comp.LightPoints;

        var lightPoints = new HashSet<EntityUid>();

        var query = EntityQueryEnumerator<SinLightPointComponent, TransformComponent>();
        while (query.MoveNext(out var uid, out var light, out var transform))
        {
            if (point.Comp.Side != light.Side)
                continue;

            lightPoints.Add(uid);
        }

        point.Comp.LightPoints = lightPoints;
        return lightPoints;
    }

    private int GetTargetCount(Entity<SinAlarmPointComponent> point)
    {
        var position = Transform(point).Coordinates;
        var entities = _lookup.GetEntitiesInRange<SinAlarmTargetComponent>(position, point.Comp.Distance);
        var count = 0;
//Triggering
        foreach (var target in entities)
        {
            if (!_mobState.IsAlive(target))
                continue;
            if (TryComp<SinAlarmTargetComponent>(target, out var comp) && !comp.Triggering)
                continue;

            count++;
        }

        return count;
    }
}
