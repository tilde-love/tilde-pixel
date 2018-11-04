// Copyright (c) Tilde Love Project. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace Tilde.Pixel.Effects
{
    public class Add :  IRgbEffectRgbF
    {
        public float Time { get; set; }

        public float Amount { get; set; }

        public ColorRgbF Color { get; set; }

        /// <inheritdoc />
        public bool Enabled { get; set; }

        /// <inheritdoc />
        public void Run(ColorBuffer<ColorRgbF> buffer, float timeDelta)
        {
            if (Time <= 0)
            {
                buffer.Add(Color * Amount);
            }
            else
            {
                buffer.Add(ColorRgbF.Lerp(ColorRgbF.Black, Color * Amount, timeDelta / Time));
            }
        }

    }

    public class Set : IRgbEffectRgbF
    {
        public float Amount { get; set; }

        public ColorRgbF Color { get; set; }
        
        /// <inheritdoc />
        public bool Enabled { get; set; }

        /// <inheritdoc />
        public void Run(ColorBuffer<ColorRgbF> buffer, float timeDelta)
        {
            buffer.Clear(Color * Amount);

        }
    }
}
