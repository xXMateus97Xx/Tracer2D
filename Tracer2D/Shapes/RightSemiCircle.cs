using System.Text.Json;

namespace Tracer2D.Shapes
{
    public class RightSemiCircle : Circle
    {
        public RightSemiCircle(int radius, in Color color, in Point center)
            : base(radius, color, center)
        {
        }

        public new static RightSemiCircle FromJson(JsonElement el)
        {
            if (el.ValueKind != JsonValueKind.Object)
                throw new InvalidOperationException("el is not an object");

            var radius = el.GetInt("radius");
            var centerEl = el.GetObject("center");
            var center = Point.FromJson(centerEl);
            var colorEl = el.GetObject("color");
            var color = Color.FromJson(colorEl);

            return new RightSemiCircle(radius, color, center);
        }

        public override bool Intersect(in Point p)
        {
            return p.x >= Center.x && base.Intersect(p);
        }
    }
}