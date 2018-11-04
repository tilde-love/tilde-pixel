// Copyright (c) Tilde Love Project. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

using System;
using System.Runtime.InteropServices;

namespace Tilde.Pixel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1), Serializable]
    public struct ColorRgba
    {
        public static readonly ColorRgba Black = new ColorRgba(0, 0, 0, 255);
        public static readonly ColorRgba White = new ColorRgba(255, 255, 255, 255);
        public static readonly ColorRgba Transparent = new ColorRgba(0, 0, 0, 0);
        public static readonly int SizeOf = Marshal.SizeOf(typeof(ColorRgba));

        public byte R;
        public byte G;
        public byte B;
        public byte A;

        public ColorRgba(byte r, byte g, byte b, byte a)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public override string ToString()
        {
            return $"{R},{G},{B},{A}";
        }

        public void ReverseRB()
        {
            byte t = R;

            R = B;

            B = t;
        }

        public static implicit operator ColorRgb(ColorRgba color) => new ColorRgb(color.R, color.G, color.B);

        public static implicit operator ColorRgbF(ColorRgba color) => new ColorRgbF(color.R * 255f, color.G * 255f, color.B * 255f);

        public static implicit operator ColorRgbaF(ColorRgba color) => new ColorRgbaF(color.R, color.G, color.B, 1f);

        public static ColorRgba Lerp(ColorRgba p1, ColorRgba p2, float factor)
        {
            return p1 + ((p2 - p1) * Math.Max(Math.Min(factor, 1f), 0f));
        }

        public static ColorRgba operator *(ColorRgba p1, ColorRgba p2)
        {
            return new ColorRgba(
                (byte)(p1.R * p2.R),
                (byte)(p1.G * p2.G),
                (byte)(p1.B * p2.B),
                (byte)(p1.A * p2.A)
            );
        }

        public static ColorRgba operator *(ColorRgba p1, float scalar)
        {
            return new ColorRgba(
                (byte)(p1.R * scalar),
                (byte)(p1.G * scalar),
                (byte)(p1.B * scalar),
                (byte)(p1.A * scalar)
            );
        }

        public static ColorRgba operator /(ColorRgba p1, ColorRgba p2)
        {
            return new ColorRgba(
                (byte)(p1.R / p2.R),
                (byte)(p1.G / p2.G),
                (byte)(p1.B / p2.B),
                (byte)(p1.A / p2.A)
            );
        }

        public static ColorRgba operator /(ColorRgba p1, float scalar)
        {
            return new ColorRgba(
                (byte)(p1.R / scalar),
                (byte)(p1.G / scalar),
                (byte)(p1.B / scalar),
                (byte)(p1.A / scalar)
            );
        }

        public static ColorRgba operator +(ColorRgba p1, ColorRgba p2)
        {
            return new ColorRgba(
                (byte)(p1.R + p2.R),
                (byte)(p1.G + p2.G),
                (byte)(p1.B + p2.B),
                (byte)(p1.A + p2.A)
            );
        }

        public static ColorRgba operator +(ColorRgba p1, float scalar)
        {
            return new ColorRgba(
                (byte)(p1.R + scalar),
                (byte)(p1.G + scalar),
                (byte)(p1.B + scalar),
                (byte)(p1.A + scalar)
            );
        }

        public static ColorRgba operator -(ColorRgba p1, ColorRgba p2)
        {
            return new ColorRgba(
                (byte)(p1.R - p2.R),
                (byte)(p1.G - p2.G),
                (byte)(p1.B - p2.B),
                (byte)(p1.A - p2.A)
            );
        }

        public static ColorRgba operator -(ColorRgba p1, float scalar)
        {
            return new ColorRgba(
                (byte)(p1.R - scalar),
                (byte)(p1.G - scalar),
                (byte)(p1.B - scalar),
                (byte)(p1.A - scalar)
            );
        }

        public static ColorRgba Clamp(ColorRgba color, ref ColorRgba min, ref ColorRgba max)
        {
            byte r = color.R, g = color.G, b = color.B, a = color.A;

            r = r < min.R ? min.R : r > max.R ? max.R : r;
            g = g < min.G ? min.G : g > max.G ? max.G : g;
            b = b < min.B ? min.B : b > max.B ? max.B : b;
            a = a < min.A ? min.A : a > max.A ? max.A : a;

            return new ColorRgba(r, g, b, a);
        }

        public static ColorRgba Max(ColorRgba color, ref ColorRgba max)
        {
            byte r = color.R, g = color.G, b = color.B, a = color.A;

            r = r > max.R ? max.R : r;
            g = g > max.G ? max.G : g;
            b = b > max.B ? max.B : b;
            a = a > max.A ? max.A : a;

            return new ColorRgba(r, g, b, a);
        }

        public static ColorRgba Min(ColorRgba color, ref ColorRgba min)
        {
            byte r = color.R, g = color.G, b = color.B, a = color.A;

            r = r < min.R ? min.R : r;
            g = g < min.G ? min.G : g;
            b = b < min.B ? min.B : b;
            a = a < min.A ? min.A : a;

            return new ColorRgba(r, g, b, a);
        }
    }
}