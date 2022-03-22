using System.Runtime.InteropServices;
using System.Text.Json;

namespace Tracer2D
{
    public struct Color
    {
        public byte r, g, b;
        public float a;

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

            var color = new Color
            {
                r = el.GetByte("r"),
                g = el.GetByte("g"),
                b = el.GetByte("b"),
                a = a
            };

            return color;
        }

        public ReadOnlySpan<byte> ToSpan() => MemoryMarshal.CreateSpan(ref r, 3);
        public void ToSpan(Span<byte> buffer) => MemoryMarshal.CreateSpan(ref r, 3).CopyTo(buffer);

        public static Color operator +(in Color a, in Color b)
        {
            var alpha = b.a;

            var result = new Color(
                (byte)((1 - alpha) * a.r + alpha * b.r),
                (byte)((1 - alpha) * a.g + alpha * b.g),
                (byte)((1 - alpha) * a.b + alpha * b.b)
            );

            return result;
        }
    }
}
