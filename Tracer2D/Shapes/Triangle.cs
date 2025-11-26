using System.Runtime.Intrinsics;
using System.Text.Json;

namespace Tracer2D.Shapes;

public class Triangle(in Point v0, in Point v1, in Point v2, in Color color) : Shape(color)
{
    readonly Point _v0 = v0, _v1 = v1, _v2 = v2;
    readonly float _area = Area(v0, v1, v2);

    public new static Triangle FromJson(JsonElement el)
    {
        if (el.ValueKind != JsonValueKind.Object)
            throw new InvalidOperationException("el is not an object");

        var v0El = el.GetObject("v0");
        var v0 = Point.FromJson(v0El);
        var v1El = el.GetObject("v1");
        var v1 = Point.FromJson(v1El);
        var v2El = el.GetObject("v2");
        var v2 = Point.FromJson(v2El);
        var colorEl = el.GetObject("color");
        var color = Color.FromJson(colorEl);

        return new Triangle(v0, v1, v2, color);
    }

    public override bool Intersect(in Point p)
    {
        var a1 = Area(p, _v1, _v2);
        var a2 = Area(_v0, p, _v2);
        var a3 = Area(_v0, _v1, p);

        return _area == a1 + a2 + a3;
    }

    public override Vector256<int> Intersect(Vector256<float> x, Vector256<float> y)
    {
        var v0x = Vector256.Create(_v0.x);
        var v0y = Vector256.Create(_v0.y);
        var v1x = Vector256.Create(_v1.x);
        var v1y = Vector256.Create(_v1.y);
        var v2x = Vector256.Create(_v2.x);
        var v2y = Vector256.Create(_v2.y);

        var a1 = Area(x, y, v1x, v1y, v2x, v2y);
        var a2 = Area(v0x, v0y, x, y, v2x, v2y);
        var a3 = Area(v0x, v0y, v1x, v1y, x, y);

        var area = Vector256.Create(_area);
        return Vector256.Equals(area, a1 + a2 + a3).AsInt32();
    }

    static float Area(in Point p1, in Point p2, in Point p3) =>
            MathF.Abs((p1.x * (p2.y - p3.y) + p2.x * (p3.y - p1.y) + p3.x * (p1.y - p2.y)) / 2f);

    static Vector256<float> Area(Vector256<float> p1x, Vector256<float> p1y, Vector256<float> p2x, Vector256<float> p2y, Vector256<float> p3x, Vector256<float> p3y) =>
         Vector256.Abs((p1x * (p2y - p3y) + p2x * (p3y - p1y) + p3x * (p1y - p2y)) / 2f);

}