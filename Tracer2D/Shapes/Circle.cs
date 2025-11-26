using System.Runtime.Intrinsics;
using System.Text.Json;

namespace Tracer2D.Shapes;

public class Circle(int radius, in Color color, in Point center) : Shape(color)
{
    readonly int _radius = radius;
    protected readonly Point Center = center;

    public new static Circle FromJson(JsonElement el)
    {
        if (el.ValueKind != JsonValueKind.Object)
            throw new InvalidOperationException("el is not an object");

        var radius = el.GetInt("radius");
        var centerEl = el.GetObject("center");
        var center = Point.FromJson(centerEl);
        var colorEl = el.GetObject("color");
        var color = Color.FromJson(colorEl);

        return new Circle(radius, color, center);
    }

    public override bool Intersect(in Point p)
    {
        var distance = MathF.Sqrt((p.x - Center.x) * (p.x - Center.x) + (p.y - Center.y) * (p.y - Center.y));
        return distance <= _radius;
    }

    public override Vector256<int> Intersect(Vector256<float> x, Vector256<float> y)
    {
        var cX = Vector256.Create(Center.x);
        var cY = Vector256.Create(Center.y);
        var distances = Vector256.Sqrt((x - cX) * (x - cX) + (y - cY) * (y - cY));
        return Vector256.LessThanOrEqual(distances, Vector256.Create((float)_radius)).AsInt32();
    }
}