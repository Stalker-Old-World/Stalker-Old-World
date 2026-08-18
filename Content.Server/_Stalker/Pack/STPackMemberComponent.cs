namespace Content.Server._Stalker.Pack;

[RegisterComponent]
public sealed partial class STPackMemberComponent : Component
{
    [DataField]
    public EntityUid Head;

    [DataField]
    public string BlackboardHeadKey = "FollowTarget";
}
