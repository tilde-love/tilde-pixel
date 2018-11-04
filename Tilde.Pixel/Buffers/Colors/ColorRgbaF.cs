// Copyright (c) Tilde Love Project. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

using System;
using System.Runtime.InteropServices;

namespace Tilde.Pixel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ColorRgbaF
    {
        public static readonly ColorRgbaF Black = new ColorRgbaF(0, 0, 0, 1);
        public static readonly ColorRgbaF White = new ColorRgbaF(1, 1, 1, 1);
        public static readonly ColorRgbaF Transparent = new ColorRgbaF(0, 0, 0, 0);
        public static readonly int SizeOf = Marshal.SizeOf(typeof(ColorRgbaF));

        public const float ValueScale = 1f / 255f;
        public const float MaxValue = 255f;

        public float R;
        public float G;
        public float B;
        public float A;

        public ColorRgbaF ClampedValue => new ColorRgbaF(Clamp(R), Clamp(G), Clamp(B), Clamp(A));

        public float Luminance => (0.299f * R + 0.587f * G + 0.114f * B) * A;

        public override string ToString()
        {
            return $"{R},{G},{B},{A}";
        }

        private static float Clamp(float value)
        {
            return Math.Max(Math.Min(value, 1f), 0f);
        }

        private static byte ClampValue(float value)
        {
            return (byte)Math.Max(Math.Min(value * 255f, 255f), 0f);
        }

        public ColorRgbaF(float r, float g, float b, float a)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public static implicit operator ColorRgba(ColorRgbaF color) => new ColorRgba(ClampValue(color.R), ClampValue(color.G), ClampValue(color.B), ClampValue(color.A));

        public static implicit operator ColorRgb(ColorRgbaF color) => new ColorRgb(ClampValue(color.R), ClampValue(color.G), ClampValue(color.B));

        public static implicit operator ColorRgbF(ColorRgbaF color) => new ColorRgbF(color.R, color.G, color.B);

        public static ColorRgbaF Lerp(ColorRgbaF p1, ColorRgbaF p2, float factor)
        {
            return p1 + ((p2 - p1) * Math.Max(Math.Min(factor, 1f), 0f));
        }

        public static ColorRgbaF operator *(ColorRgbaF p1, ColorRgbaF p2)
        {
            return new ColorRgbaF(p1.R * p2.R, p1.G * p2.G, p1.B * p2.B, p1.A * p2.A);
        }

        public static ColorRgbaF operator *(ColorRgbaF p1, float scalar)
        {
            return new ColorRgbaF(p1.R * scalar, p1.G * scalar, p1.B * scalar, p1.A * scalar);
        }

        public static ColorRgbaF operator /(ColorRgbaF p1, ColorRgbaF p2)
        {
            return new ColorRgbaF(p1.R / p2.R, p1.G / p2.G, p1.B / p2.B, p1.A / p2.A);
        }

        public static ColorRgbaF operator /(ColorRgbaF p1, float scalar)
        {
            return new ColorRgbaF(p1.R / scalar, p1.G / scalar, p1.B / scalar, p1.A / scalar);
        }

        public static ColorRgbaF operator +(ColorRgbaF p1, ColorRgbaF p2)
        {
            return new ColorRgbaF(p1.R + p2.R, p1.G + p2.G, p1.B + p2.B, p1.A + p2.A);
        }

        public static ColorRgbaF operator +(ColorRgbaF p1, float scalar)
        {
            return new ColorRgbaF(p1.R + scalar, p1.G + scalar, p1.B + scalar, p1.A + scalar);
        }

        public static ColorRgbaF operator -(ColorRgbaF p1, ColorRgbaF p2)
        {
            return new ColorRgbaF(p1.R - p2.R, p1.G - p2.G, p1.B - p2.B, p1.A - p2.A);
        }

        public static ColorRgbaF operator -(ColorRgbaF p1, float scalar)
        {
            return new ColorRgbaF(p1.R - scalar, p1.G - scalar, p1.B - scalar, p1.A - scalar);
        }

        public static ColorRgbaF Clamp(ColorRgbaF color, ref ColorRgbaF min, ref ColorRgbaF max)
        {
            float r = color.R, g = color.G, b = color.B, a = color.A;

            r = r < min.R ? min.R : r > max.R ? max.R : r;
            g = g < min.G ? min.G : g > max.G ? max.G : g;
            b = b < min.B ? min.B : b > max.B ? max.B : b;
            a = a < min.A ? min.A : a > max.A ? max.A : a;

            return new ColorRgbaF(r, g, b, a);
        }

        public static ColorRgbaF Max(ColorRgbaF color, ref ColorRgbaF max)
        {
            float r = color.R, g = color.G, b = color.B, a = color.A;

            r = r > max.R ? max.R : r;
            g = g > max.G ? max.G : g;
            b = b > max.B ? max.B : b;
            a = a > max.A ? max.A : a;

            return new ColorRgbaF(r, g, b, a);
        }

        public static ColorRgbaF Min(ColorRgbaF color, ref ColorRgbaF min)
        {
            float r = color.R, g = color.G, b = color.B, a = color.A;

            r = r < min.R ? min.R : r;
            g = g < min.G ? min.G : g;
            b = b < min.B ? min.B : b;
            a = a < min.A ? min.A : a;

            return new ColorRgbaF(r, g, b, a);
        }
    }
}