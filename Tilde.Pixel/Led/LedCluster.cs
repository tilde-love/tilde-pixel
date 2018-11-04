// Copyright (c) Tilde Love Project. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Numerics;

namespace Tilde.Pixel.Led
{
    public class LedCluster : ILedPlacement
    {
        public Vector2 Center { get; set; }

        public float Radius { get; set; }

        public int Count { get; set; }

        public int Seed { get; set; }

        public List<Pixel> Map(BufferRgbF buffer)
        {
            List<Pixel> pixels = new List<Pixel>();

            Random random = new Random(Seed);

            for (int i = 0; i < Count; i++)
            {
                float distance = (float)random.NextDouble() * Radius;
                float angle = (float)random.NextDouble() * (float)Math.PI * 2f;
                Vector3 position = new Vector3(distance, 0, 0);

                Quaternion quaternion = Quaternion.CreateFromAxisAngle(Vector3.UnitZ, angle);

                position = Vector3.Transform(position, quaternion);

                Vector2 position2 = new Vector2(position.X, position.Y);

                position2 += Center;

                pixels.Add(LedSingle.CalculatePixel(buffer, position2));
            }

            return pixels;
        }

        public void CalculateMinMax(ref Vector2 min, ref Vector2 max)
        {
            min = Vector2.Min(min, Center - new Vector2(Radius, Radius));
            max = Vector2.Max(max, Center + new Vector2(Radius, Radius));
        }

        public void Rescale(Vector2 offset, Vector2 scale)
        {
            Center += offset;
            Radius *= Math.Min(scale.X, scale.Y);
        }
    }

}
