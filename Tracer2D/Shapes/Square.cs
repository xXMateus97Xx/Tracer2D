using System.Runtime.Intrinsics;
using System.Text.Json;

namespace Tracer2D.Shapes;

public class Square : Shape
{
    readonly float _topY, _bottomY, _leftX, _rightX;

    public Square(int width, int height, in Color color, in Point center)
        : base(color)
    {
        var halfHeight = height / 2;
        var halfWidth = width / 2;
        _topY = center.y - halfHeight;
        _bottomY = center.y + halfHeight;
        _leftX = center.x - halfWidth;
        _rightX = center.x + halfWidth;
    }

    public new static Square FromJson(JsonElement el)
    {
        if (el.ValueKind != JsonValueKind.Object)
            throw new InvalidOperationException("el is not an object");

        var width = el.GetInt("width");
        var height = el.GetInt("height");
        var centerEl = el.GetObject("center");
        var center = Point.FromJson(centerEl);
        var colorEl = el.GetObject("color");
        var color = Color.FromJson(colorEl);

        return new Square(width, height, color, center);
    }

    public override bool Intersect(in Point p) =>
        p.x >= _leftX && p.x <= _rightX && p.y >= _topY && p.y <= _bottomY;

    public override Vector256<int> Intersect(Vector256<float> x, Vector256<float> y)
    {
        var v1 = Vector256.GreaterThanOrEqual(x, Vector256.Create(_leftX)).AsInt32();
        var v2 = Vector256.LessThanOrEqual(x, Vector256.Create(_rightX)).AsInt32();
        var v3 = Vector256.GreaterThanOrEqual(y, Vector256.Create(_topY)).AsInt32();
        var v4 = Vector256.LessThanOrEqual(y, Vector256.Create(_bottomY)).AsInt32();

        return v1 & v2 & v3 & v4;
    }
}