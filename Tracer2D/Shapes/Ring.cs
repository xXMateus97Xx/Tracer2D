using System.Text.Json;

namespace Tracer2D.Shapes
{
    public class Ring : Shape
    {
        public readonly int Radius, Thickness;
        public readonly Point Center;

        public Ring(int radius, int thickness, in Color color, in Point center)
        {
            Color = color;
            Radius = radius;
            Thickness = thickness;
            Center = center;
        }

        public static new Ring FromJson(JsonElement el)
        {
            if (el.ValueKind != JsonValueKind.Object)
                throw new InvalidOperationException("el is not an object");

            var radius = el.GetInt("radius");
            var thickness = el.GetInt("thickness");
            var centerEl = el.GetObject("center");
            var center = Point.FromJson(centerEl);
            var colorEl = el.GetObject("color");
            var color = Color.FromJson(colorEl);

            return new Ring(radius, thickness, color, center);
        }

        public override bool Intersect(in Point p)
        {
            var distance = MathF.Sqrt(((p.x - Center.x) * (p.x - Center.x)) + ((p.y - Center.y) * (p.y - Center.y)));
            return distance <= Radius && distance > Thickness;
        }
    }
}