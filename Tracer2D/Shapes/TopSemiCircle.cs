using System.Text.Json;

namespace Tracer2D.Shapes
{
    public class TopSemiCircle : Circle
    {
        public TopSemiCircle(int radius, in Color color, in Point center)
            : base(radius, color, center)
        {
        }

        public new static TopSemiCircle FromJson(JsonElement el)
        {
            if (el.ValueKind != JsonValueKind.Object)
                throw new InvalidOperationException("el is not an object");

            var radius = el.GetInt("radius");
            var centerEl = el.GetObject("center");
            var center = Point.FromJson(centerEl);
            var colorEl = el.GetObject("color");
            var color = Color.FromJson(colorEl);

            return new TopSemiCircle(radius, color, center);
        }

        public override bool Intersect(in Point p)
        {
            return p.y <= Center.y && base.Intersect(p);
        }
    }
}