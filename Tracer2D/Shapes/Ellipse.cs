using System.Text.Json;

namespace Tracer2D.Shapes
{
    public class Ellipse : Shape
    {
        public readonly Point Center, Radius;

        public Ellipse(in Point radius, in Color color, in Point center)
        {
            Color = color;
            Radius = radius;
            Center = center;
        }

        public static new Ellipse FromJson(JsonElement el)
        {
            if (el.ValueKind != JsonValueKind.Object)
                throw new InvalidOperationException("el is not an object");

            var radiusEl = el.GetObject("radius");
            var radius = Point.FromJson(radiusEl);
            var centerEl = el.GetObject("center");
            var center = Point.FromJson(centerEl);
            var colorEl = el.GetObject("color");
            var color = Color.FromJson(colorEl);

            return new Ellipse(radius, color, center);
        }

        public override bool Intersect(in Point p) =>
            ((p.x - Center.x) * (p.x - Center.x) / Radius.x) + ((p.y - Center.y) * (p.y - Center.y) / Radius.y) <= Radius.x;
    }
}