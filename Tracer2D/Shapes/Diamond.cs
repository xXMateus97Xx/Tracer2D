using System.Runtime.Intrinsics;
using System.Text.Json;

namespace Tracer2D.Shapes;

public class Diamond : Shape
{
    readonly Triangle _a, _b;

    public Diamond(int width, int height, in Color color, in Point center)
        : base(color)
    {
        var halfWidth = width / 2;
        var halfHeight = height / 2;
        var left = new Point(center.x - halfWidth, center.y);
        var right = new Point(center.x + halfWidth, center.y);
        var top = new Point(center.x, center.y - halfHeight);
        var bottom = new Point(center.x, center.y + halfHeight);

        _a = new Triangle(left, right, top, color);
        _b = new Triangle(left, right, bottom, color);
    }

    public new static Diamond FromJson(JsonElement el)
    {
        if (el.ValueKind != JsonValueKind.Object)
            throw new InvalidOperationException("el is not an object");

        var width = el.GetInt("width");
        var height = el.GetInt("height");
        var centerEl = el.GetObject("center");
        var center = Point.FromJson(centerEl);
        var colorEl = el.GetObject("color");
        var color = Color.FromJson(colorEl);

        return new Diamond(width, height, color, center);
    }

    public override bool Intersect(in Point p) => _a.Intersect(p) || _b.Intersect(p);

    public override Vector256<int> Intersect(Vector256<float> x, Vector256<float> y)
        => _a.Intersect(x, y) | _b.Intersect(x, y);
}