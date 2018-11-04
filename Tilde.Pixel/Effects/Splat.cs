// Copyright (c) Tilde Love Project. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

using System;
using System.Numerics;

namespace Tilde.Pixel.Effects
{
    public class Splat : IRgbEffectRgbF
    {
        private Random random = new Random(); 

        private ColorRgbF currentColor;
        private int centerX;
        private bool triggered;
        
        /// <inheritdoc />
        public bool Enabled { get; set; }
        
        public float Amount { get; set; }

        public float Trigger { get; set; }

        public ColorRgbF Color { get; set; }

        public float Size { get; set; }

        public Vector2 CenterRange { get; set; } = new Vector2(0, 1); 

        /// <inheritdoc />
        public void Run(ColorBuffer<ColorRgbF> buffer, float timeDelta)
        {
            if (triggered == true && Amount < Trigger)
            {
                triggered = false; 
            }

            if (triggered == false && Amount >= Trigger)
            {
                int cMin = (int)(CenterRange.X * buffer.Width);
                int cMax = (int)(CenterRange.Y * buffer.Width);

                centerX = random.Next(cMin, cMax);
                triggered = true; 
            }

            if (triggered == true)
            {
                int number = (int)(Size * Amount * 0.5f);
                int start = centerX - number;

                if (start < 0)
                {
                    number += -start;
                    start = 0; 
                }

                for (int i = start, ie = Math.Min(centerX + number, buffer.Width); i < ie; i++)
                {
                    buffer.Buffer[i] = Color;
                }
            }            
        }
    }
}