using System;
using System.Runtime.CompilerServices;

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

        public unsafe ReadOnlySpan<byte> ToSpan() => new ReadOnlySpan<byte>(Unsafe.AsPointer(ref r), 3);
    }
}
