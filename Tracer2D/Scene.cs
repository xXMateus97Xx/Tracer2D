using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Text.Json;

namespace Tracer2D;

public readonly struct Scene(Color background, Shape[] shapes, int width, int height)
{
    public readonly Color Background = background;
    public readonly Shape[] Shapes = shapes;
    public readonly int Width = width, Height = height;

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

    public static async Task<Scene> FromJsonAsync(Stream json)
    {
        var parsedJson = await JsonDocument.ParseAsync(json);
        return FromJson(parsedJson);
    }

    public static Scene FromJson(JsonDocument json)
    {
        var root = json.RootElement;

        if (root.ValueKind != JsonValueKind.Object)
            throw new InvalidOperationException("Root json was not an object");

        var backgroundObj = root.GetObject("background");
        var background = Color.FromJson(backgroundObj);

        var width = root.GetInt("width");
        var height = root.GetInt("height");

        var shapesObj = root.GetArray("shapes");
        var shapes = Shape.ArrayFromJson(shapesObj);

        return new Scene(background, shapes, width, height);
    }

    public void Render(Stream stream)
    {
        WriteHeader(stream);

        if (Vector256.IsHardwareAccelerated)
        {
            RenderFast(stream);
            return;
        }

        var p = new Point();
        Color finalColor;
        //1023 é multiplo de 3, e RGB tem 3 bytes
        Span<byte> buffer = stackalloc byte[1023];
        var bufPos = 0;

        var shapes = Shapes;

        for (; p.y < Height; p.y++)
        {
            for (p.x = 0; p.x < Width; p.x++)
            {
                ref readonly var c = ref Background;

                for (var i = shapes.Length - 1; i >= 0; i--)
                {
                    var shape = shapes[i];
                    if (shape.Intersect(p))
                    {
                        ref readonly var shapeColor = ref shape.Color;
                        finalColor = c + shapeColor;
                        c = ref finalColor;
                        break;
                    }
                }

                c.ToSpan(buffer[bufPos..]);
                bufPos += 3;
                if (bufPos == buffer.Length)
                {
                    stream.Write(buffer);
                    bufPos = 0;
                }
            }
        }

        if (bufPos > 0)
            stream.Write(buffer[..bufPos]);
    }

    private void RenderFast(Stream stream)
    {
        Span<byte> buffer = stackalloc byte[1024];
        Span<Color> colors = stackalloc Color[Vector256<int>.Count];
        var bufPos = 0;

        var inc = Vector256<float>.Indices;

        var shapes = Shapes;

        for (var y = 0; y < Height; y++)
        {
            var yVec = Vector256.Create((float)y);

            for (var x = 0; x < Width; x += Vector256<float>.Count)
            {
                var xVec = Vector256.Create((float)x) + inc;

                colors.Fill(Background);

                for (var i = shapes.Length - 1; i >= 0; i--)
                {
                    var shape = shapes[i];
                    var intersect = shape.Intersect(xVec, yVec);
                    var mask = Vector256.ExtractMostSignificantBits(intersect);

                    if (mask == 0)
                        continue;

                    for (var j = Vector256<int>.Count - 1; j >= 0; j--)
                    {
                        if (((mask >> j) & 1) == 1)
                            colors[j] += shape.Color;
                    }
                }

                for (var i = 0; i < colors.Length && x + i < Width; i++)
                {
                    colors[i].ToSpan(buffer[bufPos..]);
                    bufPos += 3;
                    if (buffer.Length - bufPos < 3)
                    {
                        stream.Write(buffer[..bufPos]);
                        bufPos = 0;
                    }
                }
            }
        }

        if (bufPos > 0)
            stream.Write(buffer[..bufPos]);
    }

    public async Task RenderAsync(string outputPath)
    {
        await using var file = File.OpenWrite(outputPath);
        Render(file);
    }

    private void WriteHeader(Stream file)
    {
        static int Itoa(int val, Span<byte> result)
        {
            var size = ((int)Math.Floor(Math.Log10(val)));

            ref var dstBuf = ref MemoryMarshal.GetReference(result);
            for (var i = size; val > 0 && i >= 0; i--, val /= 10)
                Unsafe.Add(ref dstBuf, i) = (byte)((val % 10) + '0');

            return size + 1;
        }

        var formatHeader = "P6\n"u8;
        var colorHeader = "\n255\n"u8;

        var headerLength = formatHeader.Length + colorHeader.Length + 21;
        var headerPos = 0;
        Span<byte> header = stackalloc byte[headerLength];

        formatHeader.CopyTo(header);
        headerPos += formatHeader.Length;
        headerPos += Itoa(Width, header[headerPos..]);
        header[headerPos++] = (byte)' ';
        headerPos += Itoa(Height, header[headerPos..]);
        colorHeader.CopyTo(header[headerPos..]);
        headerPos += colorHeader.Length;

        file.Write(header[..headerPos]);
    }
}