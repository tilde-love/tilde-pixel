// Copyright (c) Tilde Love Project. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Xml.Linq;

namespace Tilde.Pixel.Effects
{
    public class DistanceFieldCenter
    {
        private static readonly Random Random = new Random();

        public Vector2 Position { get; set; }

        public int Channel { get; set; }

        public bool Randomise { get; set; }

        public void Evaluate()
        {
            if (Randomise == false)
            {
                return;
            }

            Position = new Vector2((float)Random.NextDouble(), (float)Random.NextDouble());
        }
    }

    public class DistanceField : IRgbEffectRgbF
    {
        /// <inheritdoc />
        public bool Enabled { get; set; }
        
        public List<DistanceFieldCenter> Centers { get; }

        public DistanceField()
        {
            Centers = new List<DistanceFieldCenter>();
        }

        /// <inheritdoc />
        public void Run(ColorBuffer<ColorRgbF> buffer, float timeDelta)
        {
            List<Vector2> positionsR = new List<Vector2>();
            List<Vector2> positionsG = new List<Vector2>();
            List<Vector2> positionsB = new List<Vector2>();

            foreach (DistanceFieldCenter center in Centers)
            {
                center.Evaluate();

                switch (center.Channel)
                {
                    case 0:
                        positionsR.Add(center.Position);
                        break;
                    case 1:
                        positionsG.Add(center.Position);
                        break;
                    case 2:
                        positionsB.Add(center.Position);
                        break;
                    default:
                        break;
                }
            }

            float widthScale = 1f / (float)buffer.Width;
            float heightScale = 1f / (float)buffer.Height;

            for (int y = 0; y < buffer.Height; y++)
            {
                for (int x = 0; x < buffer.Width; x++)
                {
                    float distR = float.MaxValue, distG = float.MaxValue, distB = float.MaxValue;

                    Vector2 pixelLocation = new Vector2(
                        (float)x * widthScale,
                        (float)y * heightScale
                        );

                    distR = positionsR.Select(pos => (pixelLocation - pos).LengthFast).Concat(new[] { distR }).Min();

                    distG = positionsG.Select(pos => (pixelLocation - pos).LengthFast).Concat(new[] { distG }).Min();

                    distB = positionsB.Select(pos => (pixelLocation - pos).LengthFast).Concat(new[] { distB }).Min();

                    buffer.Buffer[y * buffer.Width + x] = new ColorRgbF(distR, distG, distB);
                }
            }
        }
    }
}