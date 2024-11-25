using System.Text.Json;

namespace Tracer2D.Shapes
{
    public class Pentagon : Shape
    {
        readonly Triangle _a, _b, _c;

        public Pentagon(int side, in Color color, in Point center)
            : base(color)
        {
            var height = side * 3.0776834f / 2;
            var halfHeight = height / 2;
            var diagonal = side * 3.236068f / 2;
            var halfDiagonal = diagonal / 2;
            var halfSide = side / 2;

            var top = new Point(center.x, (int)(center.y - halfHeight));
            var baseLeft = new Point(center.x - halfSide, (int)(center.y + halfHeight));
            var baseRight = new Point(center.x + halfSide, (int)(center.y + halfHeight));
            var sideCornerY = (int)(top.y + MathF.Sqrt(side * side - halfDiagonal * halfDiagonal));

            _a = new Triangle(top, baseLeft, baseRight, color);

            _b = new Triangle(top, baseLeft,
                new Point((int)(center.x - halfDiagonal), sideCornerY),
                color);

            _c = new Triangle(top, baseRight,
                new Point((int)(center.x + halfDiagonal), sideCornerY),
                color);
        }

        public new static Pentagon FromJson(JsonElement el)
        {
            if (el.ValueKind != JsonValueKind.Object)
                throw new InvalidOperationException("el is not an object");

            var side = el.GetInt("side");
            var centerEl = el.GetObject("center");
            var center = Point.FromJson(centerEl);
            var colorEl = el.GetObject("color");
            var color = Color.FromJson(colorEl);

            return new Pentagon(side, color, center);
        }

        public override bool Intersect(in Point p) => _a.Intersect(p) || _b.Intersect(p) || _c.Intersect(p);
    }
}