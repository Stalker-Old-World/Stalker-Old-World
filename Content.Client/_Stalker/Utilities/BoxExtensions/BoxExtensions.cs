namespace Content.Client._Stalker.Utilities.BoxExtensions;

public sealed class BoxExtensions : EntitySystem
{
    public static IEnumerable<Vector2i> GetAllPoints(Box2i box)
    {
        for (var y = box.Top; y >= box.Bottom; y--)
        {
            for (var x = box.Left; x <= box.Right; x++)
            {
                yield return new Vector2i(x, y);
            }
        }
    }
}
