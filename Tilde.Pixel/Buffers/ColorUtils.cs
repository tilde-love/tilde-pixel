// Copyright (c) Tilde Love Project. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

using System;
using System.Runtime.InteropServices;

namespace Tilde.Pixel
{
    public static class ColorUtils
    {
        public static void Copy(ColorRgb[] source, ColorRgba[] destination)
        {
            if (destination.Length != source.Length)
            {
                throw new Exception($"Array lengths do not match souce {source.Length} destination {destination.Length}");
            }

            for (int i = 0; i < source.Length; i++)
            {
                destination[i] = source[i];
            }
        }

        public static void Copy(ColorRgb[] source, ColorRgb[] destination)
        {
            if (destination.Length != source.Length)
            {
                throw new Exception($"Array lengths do not match souce {source.Length} destination {destination.Length}");
            }

            source.CopyTo(destination, 0);
        }

        public static void Copy(ColorRgba[] source, ColorRgba[] destination)
        {
            if (destination.Length != source.Length)
            {
                throw new Exception($"Array lengths do not match souce {source.Length} destination {destination.Length}");
            }

            source.CopyTo(destination, 0);
        }

        public static void Copy(ColorRgba[] source, ColorRgb[] destination)
        {
            if (destination.Length != source.Length)
            {
                throw new Exception($"Array lengths do not match souce {source.Length} destination {destination.Length}");
            }

            for (int i = 0; i < source.Length; i++)
            {
                destination[i] = source[i];
            }
        }

        public static void Copy(ColorRgbaF[] source, ColorRgba[] destination)
        {
            if (destination.Length != source.Length)
            {
                throw new Exception($"Array lengths do not match souce {source.Length} destination {destination.Length}");
            }

            for (int i = 0; i < source.Length; i++)
            {
                destination[i] = source[i];
            }
        }

        public static void Copy(ColorRgbaF[] source, ColorRgb[] destination)
        {
            if (destination.Length != source.Length)
            {
                throw new Exception($"Array lengths do not match souce {source.Length} destination {destination.Length}");
            }

            for (int i = 0; i < source.Length; i++)
            {
                destination[i] = source[i];
            }
        }

        public static void Copy(ColorRgbF[] source, ColorRgba[] destination)
        {
            if (destination.Length != source.Length)
            {
                throw new Exception($"Array lengths do not match souce {source.Length} destination {destination.Length}");
            }

            for (int i = 0; i < source.Length; i++)
            {
                destination[i] = source[i];
            }
        }

        public static void Copy(ColorRgbF[] source, ColorRgb[] destination)
        {
            if (destination.Length != source.Length)
            {
                throw new Exception($"Array lengths do not match souce {source.Length} destination {destination.Length}");
            }

            for (int i = 0; i < source.Length; i++)
            {
                destination[i] = source[i];
            }
        }

        public static void Copy(ColorRgb[] source, ColorRgbaF[] destination)
        {
            if (destination.Length != source.Length)
            {
                throw new Exception($"Array lengths do not match souce {source.Length} destination {destination.Length}");
            }

            for (int i = 0; i < source.Length; i++)
            {
                destination[i] = source[i];
            }
        }

        public static void Copy(ColorRgb[] source, ColorRgbF[] destination)
        {
            if (destination.Length != source.Length)
            {
                throw new Exception($"Array lengths do not match souce {source.Length} destination {destination.Length}");
            }

            for (int i = 0; i < source.Length; i++)
            {
                destination[i] = source[i];
            }
        }

        public static void Copy(ColorRgba[] source, ColorRgbaF[] destination)
        {
            if (destination.Length != source.Length)
            {
                throw new Exception($"Array lengths do not match souce {source.Length} destination {destination.Length}");
            }

            for (int i = 0; i < source.Length; i++)
            {
                destination[i] = source[i];
            }
        }

        public static void Copy(ColorRgbaF[] source, ColorRgbaF[] destination)
        {
            if (destination.Length != source.Length)
            {
                throw new Exception($"Array lengths do not match souce {source.Length} destination {destination.Length}");
            }

            for (int i = 0; i < source.Length; i++)
            {
                destination[i] = source[i];
            }
        }

        public static void Copy(ColorRgbaF[] source, ColorRgbF[] destination)
        {
            if (destination.Length != source.Length)
            {
                throw new Exception($"Array lengths do not match souce {source.Length} destination {destination.Length}");
            }

            for (int i = 0; i < source.Length; i++)
            {
                destination[i] = source[i];
            }
        }

        public static void Copy(ColorRgba[] source, ColorRgbF[] destination)
        {
            if (destination.Length != source.Length)
            {
                throw new Exception($"Array lengths do not match souce {source.Length} destination {destination.Length}");
            }

            for (int i = 0; i < source.Length; i++)
            {
                destination[i] = source[i];
            }
        }

        public static void Copy(ColorRgbF[] source, ColorRgbaF[] destination)
        {
            if (destination.Length != source.Length)
            {
                throw new Exception($"Array lengths do not match souce {source.Length} destination {destination.Length}");
            }

            for (int i = 0; i < source.Length; i++)
            {
                destination[i] = source[i];
            }
        }

        public static void Copy(ColorRgbF[] source, ColorRgbF[] destination)
        {
            if (destination.Length != source.Length)
            {
                throw new Exception($"Array lengths do not match souce {source.Length} destination {destination.Length}");
            }

            for (int i = 0; i < source.Length; i++)
            {
                destination[i] = source[i];
            }
        }

        [DllImport("kernel32.dll", EntryPoint = "CopyMemory", SetLastError = false)]
        public static extern void CopyMemory(IntPtr dest, IntPtr src, uint count);

        public static void CopyToByteArray<T>(T[] array, int count, byte[] bytes, ref int index)
        {
            int size = Marshal.SizeOf(typeof(T));
            int totalSize = size * count;

            GCHandle handle = GCHandle.Alloc(array, GCHandleType.Pinned);

            try
            {
                Marshal.Copy(handle.AddrOfPinnedObject(), bytes, index, totalSize);
            }
            finally
            {
                handle.Free();
            }

            index += totalSize;
        }

        public static void CopyToPointer<T>(T[] array, int count, IntPtr destination)
        {
            int size = Marshal.SizeOf(typeof(T));
            int totalSize = size * count;

            GCHandle handle = GCHandle.Alloc(array, GCHandleType.Pinned);

            try
            {
                CopyMemory(destination, handle.AddrOfPinnedObject(), (uint)totalSize);
            }
            finally
            {
                handle.Free();
            }
        }
    }
}