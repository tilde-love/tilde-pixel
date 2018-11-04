// Copyright (c) Tilde Love Project. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Xml.Linq;

namespace Tilde.Pixel.Led
{
    public class LedLinePoint 
    {
        public Vector2 Position { get; set; }
    }
    
    public class LedLine : ILedPlacement
    {
        public List<LedLinePoint> Points { get; }

        public int Count { get; set; }

        public LedLine()
        {
            Points = new List<LedLinePoint>();
        }

        public List<Pixel> Map(BufferRgbF buffer)
        {
            List<Pixel> pixels = new List<Pixel>(Count);

            if (Count == 0)
            {
                return pixels;
            }

            if (Points.Count < 2)
            {
                return pixels;
            }

            float totalLength = 0;

            Vector2 last = Points[0].Position;

            float[] proportions = new float[Points.Count];
            float[] bendProportions = new float[Points.Count];

            proportions[0] = 0;
            bendProportions[0] = 0;

            for (int i = 1; i < Points.Count; i++)
            {
                Vector2 next = Points[i].Position;

                float length = (next - last).Length();

                totalLength += length;

                proportions[i] = length;
                bendProportions[i] = totalLength;

                last = next;
            }

            if (totalLength == 0)
            {
                return pixels;
            }

            for (int i = 0; i < proportions.Length; i++)
            {
                proportions[i] = proportions[i] / totalLength;
                bendProportions[i] = bendProportions[i] / totalLength;
            }

            float blend = 1f / (float)Count;
            float blendOffset = blend * 0.5f; 

            for (int i = 0; i < Count; i++)
            {
                float currentBlend = blendOffset + (blend * (float)i);

                Vector2 prev = Points[0].Position;
                Vector2 next = Points[0].Position;
                float bp0 = 0;
                float bp1 = 0;
                float p = 0;

                for (int j = 1; j < Points.Count; j++)
                {
                    if (bendProportions[j] < currentBlend)
                    {
                        prev = Points[j].Position;
                        bp0 = bendProportions[j];
                    }
                    else
                    {
                        next = Points[j].Position;

                        bp1 = bendProportions[j];
                        p = proportions[j];

                        break; 
                    }
                }

                if (p == 0)
                {
                    continue; 
                }

                pixels.Add(LedSingle.CalculatePixel(buffer, Vector2.Lerp(prev, next, (currentBlend - bp0) / p)));
            }

            return pixels;
        }

        public void CalculateMinMax(ref Vector2 min, ref Vector2 max)
        {
            min = Points.Aggregate(min, (current, point) => Vector2.Min(current, point.Position));
            max = Points.Aggregate(max, (current, point) => Vector2.Max(current, point.Position));
        }

        public void Rescale(Vector2 offset, Vector2 scale)
        {
            foreach (LedLinePoint point in Points)
            {
                point.Position = (point.Position + offset) * scale;
            }
        }
    }
}
