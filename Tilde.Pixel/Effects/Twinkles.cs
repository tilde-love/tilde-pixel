// Copyright (c) Tilde Love Project. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

using System;

namespace Tilde.Pixel.Effects
{
    public class Twinkles : IRgbEffectRgbF
    {
        private readonly Random random = new Random();
        private int[] indicies = new int[0];
        private float time = 0;

        /// <inheritdoc />
        public bool Enabled { get; set; }
        
        public float Amount { get; set; }

        public ColorRgbF Color { get; set; }

        public int Count { get; set; }

        public float Speed { get; set; }

        /// <inheritdoc />
        public void Run(ColorBuffer<ColorRgbF> buffer, float timeDelta)
        {
            time += timeDelta * Speed;

            if (time > 1f || indicies.Length != Count)
            {
                time = (time % 1.0f + 1.0f) % 1.0f;

                if (indicies.Length != Count)
                {
                    indicies = new int[Count];
                }

                for (int i = 0; i < Count; i++)
                {
                    int x = random.Next(0, buffer.Width);
                    int y = random.Next(0, buffer.Height);

                    indicies[i] = y * buffer.Width + x;
                }
            }

            for (int i = 0; i < Count; i++)
            {
                buffer.Buffer[indicies[i]] += (Color * Amount);
            }
        }
    }
}