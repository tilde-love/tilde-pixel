// Copyright (c) Tilde Love Project. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Numerics;

namespace Tilde.Pixel.Led
{
    public class LedSingle : ILedPlacement
    {
        public static Pixel CalculatePixel(BufferRgbF buffer, Vector2 position)
        {
            Vector2 scaledPosition = position * new Vector2((float)buffer.Width, (float)buffer.Height);

            int floorX = (int)Math.Floor(scaledPosition.X);
            int floorY = (int)Math.Floor(scaledPosition.Y);

            float weightX = scaledPosition.X - floorX;
            float weightY = scaledPosition.Y - floorY;

            int index0, index1, index2, index3;
            float weight0, weight1, weight2, weight3;

            CalculateIndex(floorX, floorY, buffer.Width, buffer.Height, 1f - weightX, 1f - weightY, out index0, out weight0);
            CalculateIndex(floorX + 1, floorY, buffer.Width, buffer.Height, weightX, 1f - weightY, out index1, out weight1);
            CalculateIndex(floorX, floorY + 1, buffer.Width, buffer.Height, 1f - weightX, weightY, out index2, out weight2);
            CalculateIndex(floorX + 1, floorY + 1, buffer.Width, buffer.Height, weightX, weightY, out index3, out weight3);

            Pixel pixel = new Pixel
            {
                Position = position, 

                Index0 = index0,
                Index1 = index1,
                Index2 = index2,
                Index3 = index3,

                Weight0 = weight0,
                Weight1 = weight1,
                Weight2 = weight2,
                Weight3 = weight3,
            };

            return pixel;
        }

        private static void CalculateIndex(int x, int y, int width, int height,
                                            float weightX, float weightY,
                                            out int index0, out float weight0)
        {
            if (x < 0 || x >= width || y < 0 || y >= height)
            {
                // OscConnection.Reporter.PrintNormal($"Pixel clips buffer space {x},{y}");
                index0 = -1;
                weight0 = 0;
            }

            index0 = y * width + x;

            weight0 = (weightX + weightY) * 0.5f;
        }

        public Vector2 Position { get; set; }

        public List<Pixel> Map(BufferRgbF buffer)
        {
            List<Pixel> pixels = new List<Pixel> { CalculatePixel(buffer, Position) };

            return pixels;
        }

        public void CalculateMinMax(ref Vector2 min, ref Vector2 max)
        {
            min = Vector2.Min(min, Position);
            max = Vector2.Max(max, Position);
        }

        public void Rescale(Vector2 offset, Vector2 scale)
        {
            Position += offset;
            Position *= scale;
        }
    }
}
