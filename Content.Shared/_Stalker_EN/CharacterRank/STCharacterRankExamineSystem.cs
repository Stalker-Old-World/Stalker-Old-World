using Content.Shared.Examine;

namespace Content.Shared._Stalker_EN.CharacterRank;

/// <summary>
/// Shows the character's rank name when examined.
/// </summary>
public sealed class STCharacterRankExamineSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<STCharacterRankComponent, ExaminedEvent>(OnExamined);
    }

    private void OnExamined(EntityUid uid, STCharacterRankComponent comp, ExaminedEvent args)
    {
        if (!args.IsInDetailsRange)
            return;

        var rankText = Loc.GetString("st-rank-examine",
            ("rank", Loc.GetString(comp.RankName)));
        args.PushMarkup(rankText, -1);
    }
}
