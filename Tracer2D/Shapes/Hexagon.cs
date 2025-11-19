using System.Text.Json;

namespace Tracer2D.Shapes;

public class Hexagon : Shape
{
    readonly Triangle _a, _b;
    readonly Square _s;

    public Hexagon(int side, in Color color, in Point center)
        : base(color)
    {
        var halfSide = side / 2;
        var apothem = MathF.Sqrt(side * side - halfSide * halfSide);

        var topLeft = new Point(center.x - halfSide, (int)(center.y - apothem));
        var topRight = new Point(center.x + halfSide, (int)(center.y - apothem));
        var bottomLeft = new Point(center.x - halfSide, (int)(center.y + apothem));
        var bottomRight = new Point(center.x + halfSide, (int)(center.y + apothem));
        var left = new Point(center.x - side, center.y);
        var right = new Point(center.x + side, center.y);

        _s = new Square(side, (int)(apothem * 2), color, center);
        _a = new Triangle(bottomLeft, topLeft, left, color);
        _b = new Triangle(bottomRight, topRight, right, color);
    }

    public new static Hexagon FromJson(JsonElement el)
    {
        if (el.ValueKind != JsonValueKind.Object)
            throw new InvalidOperationException("el is not an object");

        var side = el.GetInt("side");
        var centerEl = el.GetObject("center");
        var center = Point.FromJson(centerEl);
        var colorEl = el.GetObject("color");
        var color = Color.FromJson(colorEl);

        return new Hexagon(side, color, center);
    }

    public override bool Intersect(in Point p) =>
        _s.Intersect(p) || _a.Intersect(p) || _b.Intersect(p);
}