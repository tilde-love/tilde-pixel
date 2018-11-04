// Copyright (c) Tilde Love Project. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace Tilde.Pixel.Effects
{
    public class Min : IRgbEffectRgbF
    {
        /// <inheritdoc />
        public bool Enabled { get; set; }

        public ColorRgbF Color { get; set; }

        /// <inheritdoc />
        public void Run(ColorBuffer<ColorRgbF> buffer, float timeDelta)
        {
            buffer.Min(Color);
        }
    }
}
