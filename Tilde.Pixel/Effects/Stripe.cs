// Copyright (c) Tilde Love Project. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace Tilde.Pixel.Effects
{
    public enum StripeDirection
    {
        Vertical,
        Horizontal
    }

    public class Stripe : IRgbEffectRgbF
    {
        private float position; 
        
        /// <inheritdoc />
        public bool Enabled { get; set; }
        
        public float Speed { get; set; }

        public float Amount { get; set; }

        public ColorRgbF Color { get; set; }

        public StripeDirection Orientation { get; set; }

        /// <inheritdoc />
        public void Run(ColorBuffer<ColorRgbF> buffer, float timeDelta)
        {
            position += timeDelta * Speed;

            position = (position % 1.0f + 1.0f) % 1.0f;

            if (Orientation == StripeDirection.Horizontal)
            {
                float yPos = position * (float)buffer.Width;

                int lower = (int)yPos;
                int upper = ((int)yPos + 1) % buffer.Width;

                float propInLower = yPos - lower;

                for (int x = 0; x < buffer.Height; x++)
                {
                    buffer.Buffer[(x * buffer.Width) + lower] += (Color * Amount * (1f - propInLower));
                    buffer.Buffer[(x * buffer.Width) + upper] += (Color * Amount * (propInLower));
                }
            }
            else
            {
                float yPos = position * (float)buffer.Height;

                int lower = (int)yPos;
                int upper = ((int)yPos + 1) % buffer.Height;

                float propInLower = yPos - lower;

                for (int x = 0; x < buffer.Width; x++)
                {
                    buffer.Buffer[(lower * buffer.Width) + x] += (Color * Amount * (1f - propInLower));
                    buffer.Buffer[(upper * buffer.Width) + x] += (Color * Amount * (propInLower));
                }
            }
        }
    }
}
