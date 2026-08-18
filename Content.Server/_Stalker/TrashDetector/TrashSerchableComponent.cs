namespace Content.Server._Stalker.TrashSerchable;

[RegisterComponent]
public sealed partial class TrashSerchableComponent : Component
{
    [DataField]
    public float TimeBeforeNextSearch = 0f;
}
