using System;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace Tracer2D
{
    public struct Color
    {
        public byte r, g, b;

        public Color(byte r, byte g, byte b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
        }

        public static Color FromJson(JsonElement el)
        {
            if (el.ValueKind != JsonValueKind.Object)
                throw new ArgumentException("Element is not an object", nameof(el));

            var color = new Color
            {
                r = el.GetByte("r"),
                g = el.GetByte("g"),
                b = el.GetByte("b")
            };

            return color;
        }

        public unsafe ReadOnlySpan<byte> ToSpan() => new ReadOnlySpan<byte>(Unsafe.AsPointer(ref r), 3);
    }
}
