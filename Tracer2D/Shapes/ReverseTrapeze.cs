using System.Text.Json;

namespace Tracer2D.Shapes
{
    public class ReverseTrapeze : Shape
    {
        readonly Square _square;
        readonly Triangle _left, _right;

        public ReverseTrapeze(int height, int topWidth, int bottomWidth, in Color color, in Point center)
            : base(color)
        {
            _square = new Square(bottomWidth, height, color, center);

            var halfHeight = height / 2;
            var leftX = center.x - bottomWidth / 2;
            var rightX = center.x + bottomWidth / 2;
            var topBottomDiff = (topWidth - bottomWidth) / 2;
            var topY = center.y - halfHeight;
            var bottomY = center.y + halfHeight;

            var topLeft = new Point(leftX, topY);
            var bottomLeft = new Point(leftX, bottomY);
            var deepestLeft = new Point(leftX - topBottomDiff, topLeft.y);

            var topRight = new Point(rightX, topY);
            var bottomRight = new Point(rightX, bottomY);
            var deepestRight = new Point(rightX + topBottomDiff, topRight.y);

            _left = new Triangle(topLeft, bottomLeft, deepestLeft, color);
            _right = new Triangle(topRight, bottomRight, deepestRight, color);
        }

        public static new ReverseTrapeze FromJson(JsonElement el)
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

            return new ReverseTrapeze(height, topWidth, bottomWidth, color, center);
        }

        public override bool Intersect(in Point p) => _square.Intersect(p) || _left.Intersect(p) || _right.Intersect(p);
    }
}