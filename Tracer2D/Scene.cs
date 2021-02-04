using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Tracer2D
{
    public struct Scene
    {
        static readonly byte[] _formatHeader = new byte[]{ (byte)'P', (byte)'6', (byte)'\n' };
        static readonly byte[] _colorHeader = new byte[]{ (byte)'\n', (byte)'2', (byte)'5', (byte)'5', (byte)'\n' };

        public Color Background;
        public Shape[] Shapes;
        public int Width, Height;

        public static Scene FromJson(string json)
        {
            var parsedJson = JsonDocument.Parse(json);
            return FromJson(parsedJson);
        }

        public static Scene FromJson(byte[] json)
        {
            var parsedJson = JsonDocument.Parse(json);
            return FromJson(parsedJson);
        }

        public static async ValueTask<Scene> FromJsonAsync(Stream json)
        {
            var parsedJson = await JsonDocument.ParseAsync(json);
            return FromJson(parsedJson);
        }

        public static Scene FromJson(JsonDocument json)
        {
            var root = json.RootElement;

            if (root.ValueKind != JsonValueKind.Object)
                throw new InvalidOperationException("Root json was not an object");

            var scene = new Scene();

            var background = root.GetObject("background");
            scene.Background = Color.FromJson(background);

            scene.Width = root.GetInt("width");
            scene.Height = root.GetInt("height");

            var shapes = root.GetArray("shapes");
            scene.Shapes = Shape.ArrayFromJson(shapes);

            return scene;
        }

        public void Render(Stream stream)
        {
            WriteHeader(stream);

            var p = new Point();

            for (; p.y < Height; p.y++)
            {
                for (p.x = 0; p.x < Width; p.x++)
                {
                    ref Color c = ref Background;
                    for (int i = Shapes.Length - 1; i >= 0; i--)
                    {
                        var shape = Shapes[i];
                        if (shape.Intersect(p))
                        {
                            c = ref shape.Color;
                            break;
                        }
                    }

                    stream.Write(c.ToSpan());
                }
            }
        }

        public void Render(string outputPath)
        {
            using var file = File.OpenWrite(outputPath);
            Render(file);
        }

        private void WriteHeader(Stream file)
        {
            static void Itoa(int val, ref Span<byte> result)
            {
                for (int i = 30; val > 0 && i > 0; i--, val /= 10)
                    result[i] = (byte)"0123456789"[val % 10];

                result = result.Trim((byte)0);
            }

            file.Write(_formatHeader);

            Span<byte> numberBuffer = stackalloc byte[31];
            Span<byte> copy = numberBuffer;
            Itoa(Width, ref copy);
            file.Write(copy);

            numberBuffer.Fill(0);
            file.WriteByte((byte)' ');

            copy = numberBuffer;
            Itoa(Height, ref copy);
            file.Write(copy);

            file.Write(_colorHeader);
        }
    }
}
