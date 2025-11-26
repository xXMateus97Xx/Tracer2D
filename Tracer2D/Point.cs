using System.Text.Json;

namespace Tracer2D;

public struct Point(float x, float y)
{
    public float x = x, y = y;

    public static Point FromJson(JsonElement element)
    {
        if (element.ValueKind != JsonValueKind.Object)
            throw new ArgumentException("Element is not an object", nameof(element));

        var point = new Point
        {
            x = element.GetInt("x"),
            y = element.GetInt("y")
        };

        return point;
    }
}
