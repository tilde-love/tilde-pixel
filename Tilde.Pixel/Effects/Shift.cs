// Copyright (c) Tilde Love Project. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace Tilde.Pixel.Effects
{
    public class Shift : IRgbEffectRgbF
    {
        /// <inheritdoc />
        public bool Enabled { get; set; }
        
        public void Run(ColorBuffer<ColorRgbF> buffer, float timeDelta)
        {
            for (int y = 0; y < buffer.Height; y++)
            {
                ColorRgbF color = ColorRgbF.Black;
                ColorRgbF temp;

                for (int x = 0; x < buffer.Width; x++)
                {
                    temp = buffer.Buffer[y * buffer.Width + x];
                    buffer.Buffer[y * buffer.Width + x] = color;
                    color = temp;
                }
            }
        }
    }
}