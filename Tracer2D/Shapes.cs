using System;

namespace Tracer2D
{
    public abstract class Shape
    {
        public Color Color;

        public abstract bool Intersect(in Point p);
    }

    public class Circle : Shape
    {
        public readonly int Radius;
        public readonly Point Center;

        public Circle(int radius, in Color color, in Point center)
        {
            Radius = radius;
            Color = color;
            Center = center;
        }

        public override bool Intersect(in Point p)
        {
            var distance = MathF.Sqrt(((p.x - Center.x) * (p.x - Center.x)) + ((p.y - Center.y) * (p.y - Center.y)));
            return distance <= Radius;
        }
    }

    public class Square : Shape
    {
        readonly int _topY, _bottomY, _leftX, _rightX;

        public Square(int width, int height, in Color color, in Point center)
        {
            var halfHeight = height / 2;
            var halfWidth = width / 2;
            _topY = center.y - halfHeight;
            _bottomY = center.y + halfHeight;
            _leftX = center.x - halfWidth;
            _rightX = center.x + halfWidth;

            Color = color;
        }

        public override bool Intersect(in Point p) =>
            p.x >= _leftX && p.x <= _rightX && p.y >= _topY && p.y <= _bottomY;
    }

    public class Triangle : Shape
    {
        public readonly Point v0, v1, v2;
        readonly float _area;

        public Triangle(in Point v0, in Point v1, in Point v2, in Color color)
        {
            this.v0 = v0;
            this.v1 = v1;
            this.v2 = v2;
            Color = color;
            _area = Area(v0, v1, v2);
        }

        static float Area(in Point p1, in Point p2, in Point p3) =>
                MathF.Abs((p1.x * (p2.y - p3.y) + p2.x * (p3.y - p1.y) + p3.x * (p1.y - p2.y)) / 2f);

        public override bool Intersect(in Point p)
        {
            var a1 = Area(p, v1, v2);
            var a2 = Area(v0, p, v2);
            var a3 = Area(v0, v1, p);

            return _area == a1 + a2 + a3;
        }
    }

    public class Diamond : Shape
    {
        readonly Triangle _a, _b;

        public Diamond(int width, int height, in Color color, in Point center)
        {
            Color = color;
            var halfWidth = width / 2;
            var halfHeight = height / 2;
            var left = new Point(center.x - halfWidth, center.y);
            var right = new Point(center.x + halfWidth, center.y);
            var top = new Point(center.x, center.y - halfHeight);
            var bottom = new Point(center.x, center.y + halfHeight);

            _a = new Triangle(left, right, top, color);
            _b = new Triangle(left, right, bottom, color);
        }

        public override bool Intersect(in Point p) => _a.Intersect(p) || _b.Intersect(p);
    }

    public class Pentagon : Shape
    {
        public readonly int Side;
        readonly Triangle _a, _b, _c;

        public Pentagon(int side, in Color color, in Point center)
        {
            Color = color;

            var height = side * 3.0776834f / 2;
            var halfHeight = height / 2;
            var diagonal = side * 3.236068f / 2;
            var halfDiagonal = diagonal / 2;
            var halfSide = side / 2;

            var top = new Point(center.x, (int)(center.y - halfHeight));
            var baseLeft = new Point(center.x - halfSide, (int)(center.y + halfHeight));
            var baseRight = new Point(center.x + halfSide, (int)(center.y + halfHeight));
            var sideCornerY = (int)(top.y + MathF.Sqrt((side * side) - (halfDiagonal * halfDiagonal)));

            _a = new Triangle(top, baseLeft, baseRight, color);

            _b = new Triangle(top, baseLeft,
                new Point((int)(center.x - halfDiagonal), sideCornerY),
                color);

            _c = new Triangle(top, baseRight,
                new Point((int)(center.x + halfDiagonal), sideCornerY),
                color);
        }

        public override bool Intersect(in Point p) => _a.Intersect(p) || _b.Intersect(p) || _c.Intersect(p);
    }

    public class ReversePentagon : Shape
    {
        public readonly int Side;
        readonly Triangle _a, _b, _c;

        public ReversePentagon(int side, in Color color, in Point center)
        {
            Color = color;

            var height = side * 3.0776834f / 2;
            var halfHeight = height / 2;
            var diagonal = side * 3.236068f / 2;
            var halfDiagonal = diagonal / 2;
            var halfSide = side / 2;

            var bottom = new Point(center.x, (int)(center.y + halfHeight));
            var topLeft = new Point(center.x - halfSide, (int)(center.y - halfHeight));
            var topRight = new Point(center.x + halfSide, (int)(center.y - halfHeight));
            var sideCornerY = (int)(bottom.y - MathF.Sqrt((side * side) - (halfDiagonal * halfDiagonal)));

            _a = new Triangle(bottom, topLeft, topRight, color);

            _b = new Triangle(bottom, topLeft,
                new Point((int)(center.x - halfDiagonal), sideCornerY),
                color);

            _c = new Triangle(bottom, topRight,
                new Point((int)(center.x + halfDiagonal), sideCornerY),
                color);
        }

        public override bool Intersect(in Point p) => _a.Intersect(p) || _b.Intersect(p) || _c.Intersect(p);
    }

    public class Hexagon : Shape
    {
        readonly Triangle _a, _b;
        readonly Square _s;

        public Hexagon(int side, in Color color, in Point center)
        {
            Color = color;

            var halfSide = side / 2;
            var apothem = MathF.Sqrt((side * side) - (halfSide * halfSide));

            var topLeft = new Point(center.x - halfSide, (int)(center.y - apothem));
            var topRight = new Point(center.x + halfSide, (int)(center.y - apothem));
            var bottomLeft = new Point(center.x - halfSide, (int)(center.y + apothem));
            var bottomRight = new Point(center.x + halfSide, (int)(center.y + apothem));
            var left = new Point(center.x - side, center.y);
            var right = new Point(center.x + side, center.y);

            _s = new Square(side, (int)(apothem * 2), color, center);
            _a = new Triangle(bottomLeft, topLeft, left, color);
            _b = new Triangle(bottomRight, topRight, right, color);
        }

        public override bool Intersect(in Point p) =>
            _s.Intersect(p) || _a.Intersect(p) || _b.Intersect(p);
    }

    public class Ellipse : Shape
    {
        public readonly Point Center, Radius;

        public Ellipse(in Point radius, in Color color, in Point center)
        {
            Color = color;
            Radius = radius;
            Center = center;
        }

        public override bool Intersect(in Point p) =>
            ((p.x - Center.x) * (p.x - Center.x) / Radius.x) + ((p.y - Center.y) * (p.y - Center.y) / Radius.y) <= Radius.x;
    }
}
