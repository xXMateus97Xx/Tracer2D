using System;
using System.IO;

namespace Tracer2D
{
    public struct Scene
    {
        public Color Background;
        public Shape[] Shapes;
        public int Width, Height;

        public void Render(Stream stream)
        {
            WriteHeader(stream);

            var p = new Point();

            for (int y = 0; y < Height; y++)
            {
                p.y = y;
                for (int x = 0; x < Width; x++)
                {
                    p.x = x;
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

            file.WriteByte((byte)'P');
            file.WriteByte((byte)'6');
            file.WriteByte((byte)'\n');

            Span<byte> numberBuffer = stackalloc byte[31];
            Span<byte> copy = numberBuffer;
            Itoa(Width, ref copy);
            file.Write(copy);

            numberBuffer.Fill(0);
            file.WriteByte((byte)' ');

            copy = numberBuffer;
            Itoa(Height, ref copy);
            file.Write(copy);

            file.WriteByte((byte)'\n');
            file.WriteByte((byte)'2');
            file.WriteByte((byte)'5');
            file.WriteByte((byte)'5');
            file.WriteByte((byte)'\n');
        }
    }
}
