// Copyright (c) Tilde Love Project. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace Tilde.Pixel.Effects
{
    public class Clamp : IRgbEffectRgbF
    {
        /// <inheritdoc />
        public bool Enabled { get; set; }

        public ColorRgbF Min { get; set; }

        public ColorRgbF Max { get; set; }

        /// <inheritdoc />
        public void Run(ColorBuffer<ColorRgbF> buffer, float timeDelta)
        {
            buffer.Clamp(Min, Max);
        }
    }
}
