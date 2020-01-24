namespace Tracer2D
{
    class Program
    {
        static void Main(string[] args)
        {
            JapanFlag();
            ItalyFlag();
            FranceFlag();
            BrazilFlag();
            Chessboard();
        }

        static void JapanFlag()
        {
            var scene = new Scene
            {
                Background = new Color(255, 255, 255),
                Height = 768,
                Width = 1366,
                Shapes = new Shape[]
                {
                    new Circle(300, new Color(255, 0, 0), new Point(683, 384))
                }
            };

            scene.Render("japan.ppm");
        }

        static void ItalyFlag()
        {
            var scene = new Scene
            {
                Background = new Color(255, 255, 255),
                Height = 768,
                Width = 1366,
                Shapes = new Shape[]
                {
                    new Square(455, 768, new Color(255, 0, 0), new Point(1138, 384)),
                    new Square(455, 768, new Color(0, 255, 0), new Point(227, 384)),
                }
            };

            scene.Render("italy.ppm");
        }

        static void FranceFlag()
        {
            var scene = new Scene
            {
                Background = new Color(255, 255, 255),
                Height = 768,
                Width = 1366,
                Shapes = new Shape[]
                {
                    new Square(455, 768, new Color(255, 0, 0), new Point(1138, 384)),
                    new Square(455, 768, new Color(0, 0, 255), new Point(227, 384)),
                }
            };

            scene.Render("france.ppm");
        }

        static void BrazilFlag()
        {
            var scene = new Scene
            {
                Background = new Color(0, 255, 0),
                Height = 768,
                Width = 1366,
                Shapes = new Shape[]
                {
                    new Diamond(1100, 700, new Color(255, 255, 0), new Point(683, 384)),
                    new Circle(200, new Color(0, 0, 255), new Point(683, 384)),
                }
            };

            scene.Render("brazil.ppm");
        }

        static void Chessboard()
        {
            const int cellSize = 200;
            var black = new Color();

            var scene = new Scene
            {
                Background = new Color(255, 255, 255),
                Height = 1600,
                Width = 1600,
                Shapes = new Shape[]
                {
                    new Square(cellSize, cellSize, black, new Point(1300, 1500)),
                    new Square(cellSize, cellSize, black, new Point(900, 1500)),
                    new Square(cellSize, cellSize, black, new Point(500, 1500)),
                    new Square(cellSize, cellSize, black, new Point(100, 1500)),
                    new Square(cellSize, cellSize, black, new Point(1500, 1300)),
                    new Square(cellSize, cellSize, black, new Point(1100, 1300)),
                    new Square(cellSize, cellSize, black, new Point(700, 1300)),
                    new Square(cellSize, cellSize, black, new Point(300, 1300)),
                    new Square(cellSize, cellSize, black, new Point(1300, 1100)),
                    new Square(cellSize, cellSize, black, new Point(900, 1100)),
                    new Square(cellSize, cellSize, black, new Point(500, 1100)),
                    new Square(cellSize, cellSize, black, new Point(100, 1100)),
                    new Square(cellSize, cellSize, black, new Point(1500, 900)),
                    new Square(cellSize, cellSize, black, new Point(1100, 900)),
                    new Square(cellSize, cellSize, black, new Point(700, 900)),
                    new Square(cellSize, cellSize, black, new Point(300, 900)),
                    new Square(cellSize, cellSize, black, new Point(1300, 700)),
                    new Square(cellSize, cellSize, black, new Point(900, 700)),
                    new Square(cellSize, cellSize, black, new Point(500, 700)),
                    new Square(cellSize, cellSize, black, new Point(100, 700)),
                    new Square(cellSize, cellSize, black, new Point(1500, 500)),
                    new Square(cellSize, cellSize, black, new Point(1100, 500)),
                    new Square(cellSize, cellSize, black, new Point(700, 500)),
                    new Square(cellSize, cellSize, black, new Point(300, 500)),
                    new Square(cellSize, cellSize, black, new Point(1300, 300)),
                    new Square(cellSize, cellSize, black, new Point(900, 300)),
                    new Square(cellSize, cellSize, black, new Point(500, 300)),
                    new Square(cellSize, cellSize, black, new Point(100, 300)),
                    new Square(cellSize, cellSize, black, new Point(1500, 100)),
                    new Square(cellSize, cellSize, black, new Point(1100, 100)),
                    new Square(cellSize, cellSize, black, new Point(700, 100)),
                    new Square(cellSize, cellSize, black, new Point(300, 100))
                }
            };

            scene.Render("chess.ppm");
        }
    }
}
