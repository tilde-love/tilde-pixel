// Copyright (c) Tilde Love Project. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace Tilde.Pixel.Effects
{
    public class Multiply : IRgbEffectRgbF
    {        
        /// <inheritdoc />
        public bool Enabled { get; set; }

        public float Time { get; set; }

        public float Amount { get; set; }

        public ColorRgbF Color { get; set; }

        /// <inheritdoc />
        public void Run(ColorBuffer<ColorRgbF> buffer, float timeDelta)
        {
            if (Time <= 0)
            {
                buffer.Multiply(Color * Amount);
            }
            else
            {
                buffer.Multiply(ColorRgbF.Lerp(ColorRgbF.White, Color * Amount, timeDelta / Time));
            }          
        }
    }
}
