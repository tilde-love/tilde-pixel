// Copyright (c) Tilde Love Project. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

using System;
using System.Runtime.InteropServices;

namespace Tilde.Pixel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ColorRgbF
    {
        public static readonly ColorRgbF Black = new ColorRgbF(0, 0, 0);
        public static readonly ColorRgbF White = new ColorRgbF(1, 1, 1);
        public static readonly int SizeOf = Marshal.SizeOf(typeof(ColorRgbF));

        public const float ValueScale = 1f / 255f;
        public const float MaxValue = 255f;

        public float R;
        public float G;
        public float B;

        public override string ToString()
        {
            return $"{R},{G},{B}";
        }

        public ColorRgbF ClampedValue => new ColorRgbF(Clamp(R), Clamp(G), Clamp(B));

        public float Luminance => 0.299f * R + 0.587f * G + 0.114f * B;

        private static float Clamp(float value)
        {
            return Math.Max(Math.Min(value, 1f), 0f);
        }

        private static byte ClampValue(float value)
        {
            return (byte)Math.Max(Math.Min(value * 255f, 255f), 0f);
        }

        public ColorRgbF(float r, float g, float b)
        {
            R = r;
            G = g;
            B = b;
        }

        public static implicit operator ColorRgba(ColorRgbF color) => new ColorRgba(ClampValue(color.R), ClampValue(color.G), ClampValue(color.B), 255);

        public static implicit operator ColorRgb(ColorRgbF color) => new ColorRgb(ClampValue(color.R), ClampValue(color.G), ClampValue(color.B));

        public static implicit operator ColorRgbaF(ColorRgbF color) => new ColorRgbaF(color.R, color.G, color.B, 1f);

        public static ColorRgbF Lerp(ColorRgbF p1, ColorRgbF p2, float factor)
        {
            return p1 + ((p2 - p1) * Math.Max(Math.Min(factor, 1f), 0f));
        }

        public static ColorRgbF operator *(ColorRgbF p1, ColorRgbF p2)
        {
            return new ColorRgbF(p1.R * p2.R, p1.G * p2.G, p1.B * p2.B);
        }

        public static ColorRgbF operator *(ColorRgbF p1, float scalar)
        {
            return new ColorRgbF(p1.R * scalar, p1.G * scalar, p1.B * scalar);
        }

        public static ColorRgbF operator /(ColorRgbF p1, ColorRgbF p2)
        {
            return new ColorRgbF(p1.R / p2.R, p1.G / p2.G, p1.B / p2.B);
        }

        public static ColorRgbF operator /(ColorRgbF p1, float scalar)
        {
            return new ColorRgbF(p1.R / scalar, p1.G / scalar, p1.B / scalar);
        }

        public static ColorRgbF operator +(ColorRgbF p1, ColorRgbF p2)
        {
            return new ColorRgbF(p1.R + p2.R, p1.G + p2.G, p1.B + p2.B);
        }

        public static ColorRgbF operator +(ColorRgbF p1, float scalar)
        {
            return new ColorRgbF(p1.R + scalar, p1.G + scalar, p1.B + scalar);
        }

        public static ColorRgbF operator -(ColorRgbF p1, ColorRgbF p2)
        {
            return new ColorRgbF(p1.R - p2.R, p1.G - p2.G, p1.B - p2.B);
        }

        public static ColorRgbF operator -(ColorRgbF p1, float scalar)
        {
            return new ColorRgbF(p1.R - scalar, p1.G - scalar, p1.B - scalar);
        }

        public static ColorRgbF Clamp(ColorRgbF color, ref ColorRgbF min, ref ColorRgbF max)
        {
            float r = color.R, g = color.G, b = color.B;

            r = r < min.R ? min.R : r > max.R ? max.R : r;
            g = g < min.G ? min.G : g > max.G ? max.G : g;
            b = b < min.B ? min.B : b > max.B ? max.B : b;

            return new ColorRgbF(r, g, b);
        }

        public static ColorRgbF Max(ColorRgbF color, ref ColorRgbF max)
        {
            float r = color.R, g = color.G, b = color.B;

            r = r > max.R ? max.R : r;
            g = g > max.G ? max.G : g;
            b = b > max.B ? max.B : b;

            return new ColorRgbF(r, g, b);
        }

        public static ColorRgbF Min(ColorRgbF color, ref ColorRgbF min)
        {
            float r = color.R, g = color.G, b = color.B;

            r = r < min.R ? min.R : r;
            g = g < min.G ? min.G : g;
            b = b < min.B ? min.B : b;

            return new ColorRgbF(r, g, b);
        }
    }
}