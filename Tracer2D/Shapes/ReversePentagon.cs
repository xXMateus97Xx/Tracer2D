using System.Text.Json;

namespace Tracer2D.Shapes
{
    public class ReversePentagon : Shape
    {
        readonly Triangle _a, _b, _c;

        public ReversePentagon(int side, in Color color, in Point center)
        {
            Color = color;

            var height = side * 3.0776834f / 2;
            var halfHeight = height / 2;
            var diagonal = side * 3.236068f / 2;
            var halfDiagonal = diagonal / 2;
            var halfSide = side / 2;

            var bottom = new Point(center.x, (int)(center.y + halfHeight));
            var topLeft = new Point(center.x - halfSide, (int)(center.y - halfHeight));
            var topRight = new Point(center.x + halfSide, (int)(center.y - halfHeight));
            var sideCornerY = (int)(bottom.y - MathF.Sqrt((side * side) - (halfDiagonal * halfDiagonal)));

            _a = new Triangle(bottom, topLeft, topRight, color);

            _b = new Triangle(bottom, topLeft,
                new Point((int)(center.x - halfDiagonal), sideCornerY),
                color);

            _c = new Triangle(bottom, topRight,
                new Point((int)(center.x + halfDiagonal), sideCornerY),
                color);
        }

        public static new ReversePentagon FromJson(JsonElement el)
        {
            if (el.ValueKind != JsonValueKind.Object)
                throw new InvalidOperationException("el is not an object");

            var side = el.GetInt("side");
            var centerEl = el.GetObject("center");
            var center = Point.FromJson(centerEl);
            var colorEl = el.GetObject("color");
            var color = Color.FromJson(colorEl);

            return new ReversePentagon(side, color, center);
        }

        public override bool Intersect(in Point p) => _a.Intersect(p) || _b.Intersect(p) || _c.Intersect(p);
    }
}