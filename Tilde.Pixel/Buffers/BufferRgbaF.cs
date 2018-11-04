// Copyright (c) Tilde Love Project. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

using System;
using System.Drawing;
using Tilde.Pixel.Led;

namespace Tilde.Pixel
{
    public class BufferRgbaF : ColorBuffer<ColorRgbaF>
    {
        public override void Add(ColorRgbaF color)
        {
            for (int i = 0; i < Buffer.Length; i++)
            {
                Buffer[i] += color;
            }
        }

        public override void Clamp(ColorRgbaF min, ColorRgbaF max)
        {
            for (int i = 0; i < Buffer.Length; i++)
            {
                Buffer[i] += ColorRgbaF.Clamp(Buffer[i], ref min, ref max);
            }
        }

        public override void Clear()
        {
            Array.Clear(Buffer, 0, Buffer.Length);
        }

        public override void Clear(ColorRgbaF color)
        {
            for (int i = 0; i < Buffer.Length; i++)
            {
                Buffer[i] = color;
            }
        }

        public override void CopyTo(ColorBuffer<ColorRgbaF> otherBuffer)
        {
            Buffer.CopyTo(otherBuffer.Buffer, 0);
        }

        /// <inheritdoc />
        public override ColorRgbaF GetPixelValue(Pixel pixel)
        {
            ColorRgbaF total = ColorRgbaF.Black;

            if (pixel.Index0 >= 0 && pixel.Index0 < Buffer.Length) total += Buffer[pixel.Index0] * pixel.Weight0;
            if (pixel.Index1 >= 0 && pixel.Index1 < Buffer.Length) total += Buffer[pixel.Index1] * pixel.Weight1;
            if (pixel.Index2 >= 0 && pixel.Index2 < Buffer.Length) total += Buffer[pixel.Index2] * pixel.Weight2;
            if (pixel.Index3 >= 0 && pixel.Index3 < Buffer.Length) total += Buffer[pixel.Index3] * pixel.Weight3;

            return total;
        }

        public override void Max(ColorRgbaF color)
        {
            for (int i = 0; i < Buffer.Length; i++)
            {
                Buffer[i] += ColorRgbaF.Max(Buffer[i], ref color);
            }
        }

        public override void Min(ColorRgbaF color)
        {
            for (int i = 0; i < Buffer.Length; i++)
            {
                Buffer[i] += ColorRgbaF.Min(Buffer[i], ref color);
            }
        }

        public override void Multiply(float amount)
        {
            for (int i = 0; i < Buffer.Length; i++)
            {
                Buffer[i] *= amount;
            }
        }

        public override void Multiply(ColorRgbaF color)
        {
            for (int i = 0; i < Buffer.Length; i++)
            {
                Buffer[i] *= color;
            }
        }

        public void Save(string filepath)
        {
//            string actualPath = Helper.ResolvePath(filepath);
//            Helper.EnsurePathExists(actualPath);
//
//            using (Bitmap bitmap = new Bitmap(CurrentWidth, CurrentHeight, PixelFormat.Format32bppArgb))
//            {
//                //Create a BitmapData and Lock all pixels to be written
//                BitmapData bmpData = bitmap.LockBits(
//                        new Rectangle(0, 0, bitmap.Width, bitmap.Height),
//                        ImageLockMode.WriteOnly, bitmap.PixelFormat);
//
//                ColorRgba[] buffer = new ColorRgba[Buffer.Length];
//
//                CopyTo(buffer); 
//
//                //Copy the data from the byte array into BitmapData.Scan0
//                ColorUtils.CopyToPointer(buffer, buffer.Length, bmpData.Scan0);
//
//                //Unlock the pixels
//                bitmap.UnlockBits(bmpData);
//
//                bitmap.Save(actualPath);
//            }
        }

        /// <inheritdoc />
        public override void CopyTo(ColorRgba[] buffer)
        {
            ColorUtils.Copy(Buffer, buffer);
        }

        /// <inheritdoc />
        public override void CopyTo(ColorRgb[] buffer)
        {
            ColorUtils.Copy(Buffer, buffer);
        }

        /// <inheritdoc />
        public override void CopyTo(ColorRgbaF[] buffer)
        {
            ColorUtils.Copy(Buffer, buffer);
        }

        /// <inheritdoc />
        public override void CopyTo(ColorRgbF[] buffer)
        {
            ColorUtils.Copy(Buffer, buffer);
        }
    }
}