using System.Text.Json;

namespace Tracer2D.Shapes;

public class Ellipse(in Point radius, in Color color, in Point center) : Shape(color)
{
    readonly Point _center = center, _radius = radius;

    public new static Ellipse FromJson(JsonElement el)
    {
        if (el.ValueKind != JsonValueKind.Object)
            throw new InvalidOperationException("el is not an object");

        var radiusEl = el.GetObject("radius");
        var radius = Point.FromJson(radiusEl);
        var centerEl = el.GetObject("center");
        var center = Point.FromJson(centerEl);
        var colorEl = el.GetObject("color");
        var color = Color.FromJson(colorEl);

        return new Ellipse(radius, color, center);
    }

    public override bool Intersect(in Point p) =>
        (p.x - _center.x) * (p.x - _center.x) / _radius.x + (p.y - _center.y) * (p.y - _center.y) / _radius.y <= _radius.x;
}