﻿namespace Tracer2D
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
                Console.Error.WriteLine("input does not exists");
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
    }
}
