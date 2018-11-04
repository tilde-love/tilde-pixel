// Copyright (c) Tilde Love Project. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

using System.Collections.Generic;
using System.Numerics;

namespace Tilde.Pixel.Led
{
    public class LedChain : ILedPlacement
    {
        public Vector2 Start { get; set; }

        public Vector2 End { get; set; }

        public int Count { get; set; }

        public List<Pixel> Map(BufferRgbF buffer)
        {
            List<Pixel> pixels = new List<Pixel>(Count);

            if (Count == 0)
            {
                return pixels;
            }

            float blend = 1f / (float)Count;
            for (int i = 0; i < Count; i++)
            {

                pixels.Add(LedSingle.CalculatePixel(buffer, Vector2.Lerp(Start, End, blend * (float)i)));
            }

            return pixels;
        }

        public void CalculateMinMax(ref Vector2 min, ref Vector2 max)
        {
            min = Vector2.Min(min, Start);
            min = Vector2.Min(min, End);

            max = Vector2.Max(max, Start);
            max = Vector2.Max(max, End);
        }

        public void Rescale(Vector2 offset, Vector2 scale)
        {
            Start += offset;
            Start *= scale;

            End += offset;
            End *= scale;
        }
    }

}
