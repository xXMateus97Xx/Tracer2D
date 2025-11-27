using System.Numerics;
using System.Runtime.Intrinsics;
using System.Text.Json;

namespace Tracer2D;

public readonly struct Color
{
    public readonly byte r, g, b;
    public readonly float a;

    public Color(byte r, byte g, byte b)
    {
        this.r = r;
        this.g = g;
        this.b = b;
        this.a = 1;
    }

    public Color(byte r, byte g, byte b, float a)
    {
        this.r = r;
        this.g = g;
        this.b = b;
        this.a = a;
    }

    public static Color FromJson(JsonElement el)
    {
        if (el.ValueKind != JsonValueKind.Object)
            throw new ArgumentException("Element is not an object", nameof(el));

        var a = 1f;
        if (el.TryGetProperty("a", out _))
            a = el.GetFloat("a");

        var r = el.GetByte("r");
        var g = el.GetByte("g");
        var b = el.GetByte("b");

        var color = new Color(r, g, b, a);

        return color;
    }

    public void ToSpan(Span<byte> buffer)
    {
        buffer[2] = b;
        buffer[1] = g;
        buffer[0] = r;
    }

    public static Color operator +(in Color a, in Color b)
    {
        var aVec = new Vector3(a.r, a.g, a.b);
        var bVec = new Vector3(b.r, b.g, b.b);

        var alpha = b.a;

        var r = (1 - alpha) * aVec + alpha * bVec;

        var v = Vector128.ConvertToInt32(r.AsVector128()).AsByte();

        var result = new Color(v[0], v[4], v[8]);

        return result;
    }
}
