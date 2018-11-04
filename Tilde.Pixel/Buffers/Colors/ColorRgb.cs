// Copyright (c) Tilde Love Project. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

using System;
using System.Runtime.InteropServices;

namespace Tilde.Pixel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1), Serializable]
    public struct ColorRgb
    {
        public static readonly ColorRgb Black = new ColorRgb(0, 0, 0);
        public static readonly ColorRgb White = new ColorRgb(255, 255, 255);
        public static readonly int SizeOf = Marshal.SizeOf(typeof(ColorRgb)); 

        public byte R;
        public byte G;
        public byte B;

        public ColorRgb(byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
        }

        public void ReverseRB()
        {
            byte t = R;

            R = B;

            B = t;
        }

        public override string ToString()
        {
            return $"{R},{G},{B}";
        }

        public static implicit operator ColorRgba(ColorRgb color) => new ColorRgba(color.R, color.G, color.B, 255);

        public static implicit operator ColorRgbF(ColorRgb color) => new ColorRgbF(color.R * ColorRgbF.ValueScale, color.G * ColorRgbF.ValueScale, color.B * ColorRgbF.ValueScale);

        public static implicit operator ColorRgbaF(ColorRgb color) => new ColorRgbaF(color.R * ColorRgbaF.ValueScale, color.G * ColorRgbaF.ValueScale, color.B * ColorRgbaF.ValueScale, 1f);

        public static ColorRgb Lerp(ColorRgb p1, ColorRgb p2, float factor)
        {
            return p1 + ((p2 - p1) * Math.Max(Math.Min(factor, 255), 0));
        }

        public static ColorRgb operator *(ColorRgb p1, ColorRgb p2)
        {
            return new ColorRgb((byte)(p1.R * p2.R), (byte)(p1.G * p2.G), (byte)(p1.B * p2.B));
        }

        public static ColorRgb operator *(ColorRgb p1, float scalar)
        {
            return new ColorRgb((byte)(p1.R * scalar), (byte)(p1.G * scalar), (byte)(p1.B * scalar));
        }

        public static ColorRgb operator /(ColorRgb p1, ColorRgb p2)
        {
            return new ColorRgb((byte)(p1.R / p2.R), (byte)(p1.G / p2.G), (byte)(p1.B / p2.B));
        }

        public static ColorRgb operator /(ColorRgb p1, float scalar)
        {
            return new ColorRgb((byte)(p1.R / scalar), (byte)(p1.G / scalar), (byte)(p1.B / scalar));
        }

        public static ColorRgb operator +(ColorRgb p1, ColorRgb p2)
        {
            return new ColorRgb((byte)(p1.R + p2.R), (byte)(p1.G + p2.G), (byte)(p1.B + p2.B));
        }

        public static ColorRgb operator +(ColorRgb p1, float scalar)
        {
            return new ColorRgb((byte)(p1.R + scalar), (byte)(p1.G + scalar), (byte)(p1.B + scalar));
        }

        public static ColorRgb operator -(ColorRgb p1, ColorRgb p2)
        {
            return new ColorRgb((byte)(p1.R - p2.R), (byte)(p1.G - p2.G), (byte)(p1.B - p2.B));
        }

        public static ColorRgb operator -(ColorRgb p1, float scalar)
        {
            return new ColorRgb((byte)(p1.R - scalar), (byte)(p1.G - scalar), (byte)(p1.B - scalar));
        }

        public static ColorRgb Clamp(ColorRgb color, ref ColorRgb min, ref ColorRgb max)
        {
            byte r = color.R, g = color.G, b = color.B;

            r = r < min.R ? min.R : r > max.R ? max.R : r;
            g = g < min.G ? min.G : g > max.G ? max.G : g;
            b = b < min.B ? min.B : b > max.B ? max.B : b;

            return new ColorRgb(r, g, b);
        }

        public static ColorRgb Max(ColorRgb color, ref ColorRgb max)
        {
            byte r = color.R, g = color.G, b = color.B;

            r = r > max.R ? max.R : r;
            g = g > max.G ? max.G : g;
            b = b > max.B ? max.B : b;

            return new ColorRgb(r, g, b);
        }

        public static ColorRgb Min(ColorRgb color, ref ColorRgb min)
        {
            byte r = color.R, g = color.G, b = color.B;

            r = r < min.R ? min.R : r;
            g = g < min.G ? min.G : g;
            b = b < min.B ? min.B : b;

            return new ColorRgb(r, g, b);
        }
    }
}