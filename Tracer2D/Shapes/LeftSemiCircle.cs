using System;
using System.Text.Json;

namespace Tracer2D.Shapes
{
    public class LeftSemiCircle : Circle
    {
        public LeftSemiCircle(int radius, in Color color, in Point center)
            : base(radius, color, center)
        {
        }

        public static new LeftSemiCircle FromJson(JsonElement el)
        {
            if (el.ValueKind != JsonValueKind.Object)
                throw new InvalidOperationException("el is not an object");
            
            var radius = el.GetInt("radius");
            var centerEl = el.GetObject("center");
            var center = Point.FromJson(centerEl);
            var colorEl = el.GetObject("color");
            var color = Color.FromJson(colorEl);

            return new LeftSemiCircle(radius, color, center);
        }

        public override bool Intersect(in Point p)
        {
            if (p.x > Center.x)
                return false;

            return base.Intersect(p);
        }
    }
}