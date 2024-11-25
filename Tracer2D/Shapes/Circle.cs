using System.Text.Json;

namespace Tracer2D.Shapes
{
    public class Circle : Shape
    {
        readonly int _radius;
        protected readonly Point Center;

        public Circle(int radius, in Color color, in Point center)
            : base(color)
        {
            _radius = radius;
            Center = center;
        }

        public new static Circle FromJson(JsonElement el)
        {
            if (el.ValueKind != JsonValueKind.Object)
                throw new InvalidOperationException("el is not an object");

            var radius = el.GetInt("radius");
            var centerEl = el.GetObject("center");
            var center = Point.FromJson(centerEl);
            var colorEl = el.GetObject("color");
            var color = Color.FromJson(colorEl);

            return new Circle(radius, color, center);
        }

        public override bool Intersect(in Point p)
        {
            var distance = MathF.Sqrt((p.x - Center.x) * (p.x - Center.x) + (p.y - Center.y) * (p.y - Center.y));
            return distance <= _radius;
        }
    }
}