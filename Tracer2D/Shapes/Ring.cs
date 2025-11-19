using System.Text.Json;

namespace Tracer2D.Shapes;

public class Ring(int radius, int thickness, in Color color, in Point center) : Shape(color)
{
    readonly int _radius = radius, _thickness = thickness;
    readonly Point _center = center;

    public new static Ring FromJson(JsonElement el)
    {
        if (el.ValueKind != JsonValueKind.Object)
            throw new InvalidOperationException("el is not an object");

        var radius = el.GetInt("radius");
        var thickness = el.GetInt("thickness");
        var centerEl = el.GetObject("center");
        var center = Point.FromJson(centerEl);
        var colorEl = el.GetObject("color");
        var color = Color.FromJson(colorEl);

        return new Ring(radius, thickness, color, center);
    }

    public override bool Intersect(in Point p)
    {
        var distance = MathF.Sqrt((p.x - _center.x) * (p.x - _center.x) + (p.y - _center.y) * (p.y - _center.y));
        return distance <= _radius && distance > _thickness;
    }
}