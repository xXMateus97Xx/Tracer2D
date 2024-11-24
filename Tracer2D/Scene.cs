﻿using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace Tracer2D
{
    public struct Scene
    {
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
            Color finalColor;
            //1023 é multiplo de 3, e RGB tem 3 bytes
            Span<byte> buffer = stackalloc byte[1023];
            var bufPos = 0;

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
                            ref var shapeColor = ref shape.Color;
                            finalColor = c + shapeColor;
                            c = ref finalColor;
                            break;
                        }
                    }

                    c.ToSpan(buffer.Slice(bufPos));
                    bufPos += 3;
                    if (bufPos == buffer.Length)
                    {
                        stream.Write(buffer);
                        bufPos = 0;
                    }
                }
            }

            if (bufPos > 0)
                stream.Write(buffer.Slice(0, bufPos));
        }

        public async Task RenderAsync(string outputPath)
        {
            using var file = File.OpenWrite(outputPath);
            Render(file);
        }

        private void WriteHeader(Stream file)
        {
            static int Itoa(int val, Span<byte> result)
            {
                ref char numbers = ref MemoryMarshal.GetReference("0123456789".AsSpan());

                Span<byte> tempResult = stackalloc byte[31];
                ref byte dstBuf = ref MemoryMarshal.GetReference(tempResult);

                int i, j;
                for (i = 30, j = 0; val > 0 && i > 0; i--, val /= 10, j++)
                {
                    var number = Unsafe.Add(ref numbers, val % 10);
                    Unsafe.Add(ref dstBuf, i) = (byte)number;
                }

                tempResult.TrimStart((byte)0).CopyTo(result);

                return j;
            }

            var formatHeader = "P6\n"u8;
            var colorHeader = "\n255\n"u8;

            var headerLength = formatHeader.Length + colorHeader.Length + 21;
            var headerPos = 0;
            Span<byte> header = stackalloc byte[headerLength];

            formatHeader.CopyTo(header);
            headerPos += formatHeader.Length;
            headerPos += Itoa(Width, header.Slice(headerPos));
            header[headerPos++] = (byte)' ';
            headerPos += Itoa(Height, header.Slice(headerPos));
            colorHeader.CopyTo(header.Slice(headerPos));
            headerPos += colorHeader.Length;

            file.Write(header.Slice(0, headerPos));
        }
    }
}
