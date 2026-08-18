using Content.Server.Popups;
using Content.Server.Speech.EntitySystems;
using Content.Shared.Abilities.Mime;
using Content.Shared.Chat;
using Content.Shared.Chat.Prototypes;
using Content.Shared.Puppet;
using Content.Shared.Speech;
using Content.Shared.Speech.Muting;
using Content.Shared._Stalker_OW.Monolith; // ST:OW

namespace Content.Server.Speech.Muting
{
    public sealed class MutingSystem : EntitySystem
    {
        [Dependency] private readonly PopupSystem _popupSystem = default!;
        public override void Initialize()
        {
            base.Initialize();
            SubscribeLocalEvent<MutedComponent, SpeakAttemptEvent>(OnSpeakAttempt);
            SubscribeLocalEvent<MutedComponent, EmoteEvent>(OnEmote, before: new[] { typeof(VocalSystem), typeof(MumbleAccentSystem) });
            SubscribeLocalEvent<MutedComponent, ScreamActionEvent>(OnScreamAction, before: new[] { typeof(VocalSystem) });
            SubscribeLocalEvent<MonolithHivemindSendAttemptEvent>(OnMonolithHivemindSendAttempt); // ST:OW
        }

        private void OnEmote(EntityUid uid, MutedComponent component, ref EmoteEvent args)
        {
            if (args.Handled)
                return;

            //still leaves the text so it looks like they are pantomiming a laugh
            if (args.Emote.Category.HasFlag(EmoteCategory.Vocal))
                args.Handled = true;
        }

        private void OnScreamAction(EntityUid uid, MutedComponent component, ScreamActionEvent args)
        {
            if (args.Handled)
                return;

            if (HasComp<MimePowersComponent>(uid))
                _popupSystem.PopupEntity(Loc.GetString("mime-cant-speak"), uid, uid);

            else
                _popupSystem.PopupEntity(Loc.GetString("speech-muted"), uid, uid);
            args.Handled = true;
        }

        // ST:OW begin
        private void OnSpeakAttempt(EntityUid uid, MutedComponent component, SpeakAttemptEvent args)
        {
            if (HasComp<MimePowersComponent>(uid))
                _popupSystem.PopupEntity(Loc.GetString("mime-cant-speak"), uid, uid);
            else if (HasComp<VentriloquistPuppetComponent>(uid))
                _popupSystem.PopupEntity(Loc.GetString("ventriloquist-puppet-cant-speak"), uid, uid);
            else
                _popupSystem.PopupEntity(Loc.GetString("speech-muted"), uid, uid);

            args.Cancel();
        }
        
        // Handles attempts to message using Monolith hivemind
        private void OnMonolithHivemindSendAttempt(ref MonolithHivemindSendAttemptEvent args)
        {
            var sender = args.Sender;

            if (!HasComp<MutedComponent>(sender))
                return;

            // If sender is Monolith but muted then they are still allowed to send hivemind messages
            if (HasComp<MonolithHivemindComponent>(sender))
                return;

            args.Cancelled = true;

            if (TryComp<MimePowersComponent>(sender, out _))
                _popupSystem.PopupEntity(Loc.GetString("mime-cant-speak"), sender, sender);
            else if (TryComp<VentriloquistPuppetComponent>(sender, out _))
                _popupSystem.PopupEntity(Loc.GetString("ventriloquist-puppet-cant-speak"), sender, sender);
            else
                _popupSystem.PopupEntity(Loc.GetString("speech-muted"), sender, sender);
        }
        // ST:OW end
    }
}
