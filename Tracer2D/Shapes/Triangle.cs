using System.Text.Json;

namespace Tracer2D.Shapes
{
    public class Triangle : Shape
    {
        readonly Point _v0, _v1, _v2;
        readonly float _area;

        public Triangle(in Point v0, in Point v1, in Point v2, in Color color)
            : base(color)
        {
            _v0 = v0;
            _v1 = v1;
            _v2 = v2;
            _area = Area(v0, v1, v2);
        }

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

        static float Area(in Point p1, in Point p2, in Point p3) =>
                MathF.Abs((p1.x * (p2.y - p3.y) + p2.x * (p3.y - p1.y) + p3.x * (p1.y - p2.y)) / 2f);
    }
}