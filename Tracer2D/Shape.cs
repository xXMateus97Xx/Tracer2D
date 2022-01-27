using System.Text.Json;
using Tracer2D.Shapes;

namespace Tracer2D
{
    public enum ShapeKind
    {
        Square,
        Circle,
        Triangle,
        Diamond,
        Pentagon,
        Hexagon,
        ReversePentagon,
        Ellipse,
        Trapeze,
        ReverseTrapeze,
        Ring,
        TopSemiCircle,
        RightSemiCircle,
        BottomSemiCircle,
        LefSemiCircle
    }

    public abstract class Shape
    {
        public Color Color;

        public abstract bool Intersect(in Point p);

        public static Shape[] ArrayFromJson(JsonElement array)
        {
            if (array.ValueKind != JsonValueKind.Array)
                throw new ArgumentException("Element is not an array", nameof(array));

            var shapes = new Shape[array.GetArrayLength()];

            var i = 0;
            foreach (var item in array.EnumerateArray())
                shapes[i++] = FromJson(item);

            return shapes;
        }

        public static Shape FromJson(JsonElement item)
        {
            var kind = item.GetEnum<ShapeKind>("kind");
            switch (kind)
            {
                case ShapeKind.Square:
                    return Square.FromJson(item);
                case ShapeKind.Circle:
                    return Circle.FromJson(item);
                case ShapeKind.Triangle:
                    return Triangle.FromJson(item);
                case ShapeKind.Diamond:
                    return Diamond.FromJson(item);
                case ShapeKind.Pentagon:
                    return Pentagon.FromJson(item);
                case ShapeKind.Hexagon:
                    return Hexagon.FromJson(item);
                case ShapeKind.ReversePentagon:
                    return ReversePentagon.FromJson(item);
                case ShapeKind.Ellipse:
                    return Ellipse.FromJson(item);
                case ShapeKind.Trapeze:
                    return Trapeze.FromJson(item);
                case ShapeKind.ReverseTrapeze:
                    return ReverseTrapeze.FromJson(item);
                case ShapeKind.Ring:
                    return Ring.FromJson(item);
                case ShapeKind.TopSemiCircle:
                    return TopSemiCircle.FromJson(item);
                case ShapeKind.RightSemiCircle:
                    return RightSemiCircle.FromJson(item);
                case ShapeKind.BottomSemiCircle:
                    return BottomSemiCircle.FromJson(item);
                case ShapeKind.LefSemiCircle:
                    return LeftSemiCircle.FromJson(item);
                default:
                    throw new InvalidOperationException($"Unknown shape kind {kind}");
            }
        }
    }
}
