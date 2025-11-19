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
        LeftSemiCircle
    }

    public abstract class Shape(in Color color)
    {
        public readonly Color Color = color;

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
            return kind switch
            {
                ShapeKind.Square => Square.FromJson(item),
                ShapeKind.Circle => Circle.FromJson(item),
                ShapeKind.Triangle => Triangle.FromJson(item),
                ShapeKind.Diamond => Diamond.FromJson(item),
                ShapeKind.Pentagon => Pentagon.FromJson(item),
                ShapeKind.Hexagon => Hexagon.FromJson(item),
                ShapeKind.ReversePentagon => ReversePentagon.FromJson(item),
                ShapeKind.Ellipse => Ellipse.FromJson(item),
                ShapeKind.Trapeze => Trapeze.FromJson(item),
                ShapeKind.ReverseTrapeze => ReverseTrapeze.FromJson(item),
                ShapeKind.Ring => Ring.FromJson(item),
                ShapeKind.TopSemiCircle => TopSemiCircle.FromJson(item),
                ShapeKind.RightSemiCircle => RightSemiCircle.FromJson(item),
                ShapeKind.BottomSemiCircle => BottomSemiCircle.FromJson(item),
                ShapeKind.LeftSemiCircle => LeftSemiCircle.FromJson(item),
                _ => throw new InvalidOperationException($"Unknown shape kind {kind}"),
            };
        }
    }
}
