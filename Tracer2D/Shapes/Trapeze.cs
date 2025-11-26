using System.Runtime.Intrinsics;
using System.Text.Json;

namespace Tracer2D.Shapes;

public class Trapeze : Shape
{
    readonly Square _square;
    readonly Triangle _left, _right;

    public Trapeze(int height, int topWidth, int bottomWidth, in Color color, in Point center)
        : base(color)
    {
        _square = new Square(topWidth, height, color, center);

        var halfHeight = height / 2;
        var leftX = center.x - topWidth / 2;
        var rightX = center.x + topWidth / 2;
        var topBottomDiff = (bottomWidth - topWidth) / 2;
        var topY = center.y - halfHeight;
        var bottomY = center.y + halfHeight;

        var topLeft = new Point(leftX, topY);
        var bottomLeft = new Point(leftX, bottomY);
        var deepestLeft = new Point(leftX - topBottomDiff, bottomLeft.y);

        var topRight = new Point(rightX, topY);
        var bottomRight = new Point(rightX, bottomY);
        var deepestRight = new Point(rightX + topBottomDiff, bottomRight.y);

        _left = new Triangle(topLeft, bottomLeft, deepestLeft, color);
        _right = new Triangle(topRight, bottomRight, deepestRight, color);
    }

    public new static Trapeze FromJson(JsonElement el)
    {
        if (el.ValueKind != JsonValueKind.Object)
            throw new InvalidOperationException("el is not an object");

        var height = el.GetInt("height");
        var topWidth = el.GetInt("topwidth");
        var bottomWidth = el.GetInt("bottomwidth");
        var centerEl = el.GetObject("center");
        var center = Point.FromJson(centerEl);
        var colorEl = el.GetObject("color");
        var color = Color.FromJson(colorEl);

        return new Trapeze(height, topWidth, bottomWidth, color, center);
    }

    public override bool Intersect(in Point p) => _square.Intersect(p) || _left.Intersect(p) || _right.Intersect(p);

    public override Vector256<int> Intersect(Vector256<float> x, Vector256<float> y)
        => _square.Intersect(x, y) | _left.Intersect(x, y) | _right.Intersect(x, y);
}