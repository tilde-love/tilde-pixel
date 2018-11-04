// Copyright (c) Tilde Love Project. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace Tilde.Pixel.Effects
{
    public class Block : IRgbEffectRgbF
    {
        /// <inheritdoc />
        public bool Enabled { get; set; }

        public float Amount { get; set; }

        public ColorRgbF Color { get; set; }

        public float Time { get; set; }

        public float Position { get; set; } 

        /// <inheritdoc />
        public void Run(ColorBuffer<ColorRgbF> buffer, float timeDelta)
        {
            int index = (int)(Position * buffer.Width);
            if (Time <= 0)
            {
                buffer.Buffer[index] += Color * Amount;
            }
            else
            {
                buffer.Buffer[index] += ColorRgbF.Lerp(ColorRgbF.Black, Color * Amount, timeDelta / Time);
            }
        }
    }
}