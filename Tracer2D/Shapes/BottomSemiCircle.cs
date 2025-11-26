using System.Runtime.Intrinsics;
using System.Text.Json;

namespace Tracer2D.Shapes;

public class BottomSemiCircle(int radius, in Color color, in Point center) : Circle(radius, color, center)
{
    public new static BottomSemiCircle FromJson(JsonElement el)
    {
        if (el.ValueKind != JsonValueKind.Object)
            throw new InvalidOperationException("el is not an object");

        var radius = el.GetInt("radius");
        var centerEl = el.GetObject("center");
        var center = Point.FromJson(centerEl);
        var colorEl = el.GetObject("color");
        var color = Color.FromJson(colorEl);

        return new BottomSemiCircle(radius, color, center);
    }

    public override bool Intersect(in Point p)
    {
        return p.y >= Center.y && base.Intersect(p);
    }

    public override Vector256<int> Intersect(Vector256<float> x, Vector256<float> y)
    {
        return Vector256.GreaterThanOrEqual(y, Vector256.Create(Center.y)).AsInt32() & base.Intersect(x, y);
    }
}