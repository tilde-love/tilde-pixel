// Copyright (c) Tilde Love Project. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

using System;

namespace Tilde.Pixel.Effects
{
    public class Gradient : IRgbEffectRgbF
    {        
        /// <inheritdoc />
        public bool Enabled { get; set; }

        public float Amount { get; set; }

        public ColorRgbF End { get; set; }

        public ColorRgbF Start { get; set; }

        public float XEnd { get; set; } = 1f;

        public float XStart { get; set; } = 0f;

        public void Run(ColorBuffer<ColorRgbF> buffer, float timeDelta)
        {
            //Vector3 start = new Vector3(Start.R, Start.G, Start.B);
            //Vector3 end = new Vector3(End.R, End.G, End.B);

            int index = 0;
            float xScale = 1f / (float)buffer.Width;

            float xRangeScale = 1f / (XEnd - XStart);

            for (int y = 0; y < buffer.Height; y++)
            {
                for (int x = 0; x < buffer.Width; x++)
                {
                    buffer.Buffer[index++] = ColorRgbF.Lerp(Start, End, Math.Min(Math.Max(((float)x * xScale - XStart) * xRangeScale, 0f), 1f)) * Amount;
                }
            }
        }
    }
}