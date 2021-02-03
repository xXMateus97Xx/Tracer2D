using System;
using System.IO;
using System.Threading.Tasks;
using Tracer2D.Shapes;

namespace Tracer2D
{
    class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.Error.WriteLine("Invalid arguments");
                Environment.Exit(-1);
            }

            var input = args[0];
            var output = args[1];

            if (!File.Exists(input))
            {
                Console.Error.WriteLine("input doest not exists");
                Environment.Exit(-1);
            }

            try
            {
                using var inputFile = File.OpenRead(input);
                var scene = await Scene.FromJsonAsync(inputFile);
                scene.Render(output);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                Environment.Exit(-1);
            }
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
