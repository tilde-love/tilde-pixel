// Copyright (c) Tilde Love Project. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

using System;
using System.Numerics;

namespace Tilde.Pixel.Effects
{
    public class Sampler : IRgbEffectRgbF
    {
        /// <inheritdoc />
        public bool Enabled { get; set; }

        public ColorBuffer<ColorRgbF> Field { get; set; }

        public ColorBuffer<ColorRgbF> Source0 { get; set; }

        public ColorBuffer<ColorRgbF> Source1 { get; set; }

        public ColorBuffer<ColorRgbF> Source2 { get; set; }

        public bool ReverseSource0 { get; set; }

        public bool ReverseSource1 { get; set; }

        public bool ReverseSource2 { get; set; }

        public Sampler()
        {
        }

        public void Run(ColorBuffer<ColorRgbF> buffer, float timeDelta)
        {
            ColorBuffer<ColorRgbF> field = Field;
            ColorBuffer<ColorRgbF> source0 = Source0;
            ColorBuffer<ColorRgbF> source1 = Source1;
            ColorBuffer<ColorRgbF> source2 = Source2;

            if (field == null
                || source0 == null
                || source1 == null
                || source2 == null)
            {
                return;
            }

            float maximum = new Vector2(1, 1).Length();

            float source0Scale = ((source0.Width + 2) / maximum) * (ReverseSource0 ? -1f : 1f);
            float source1Scale = ((source1.Width + 2) / maximum) * (ReverseSource1 ? -1f : 1f);
            float source2Scale = ((source2.Width + 2) / maximum) * (ReverseSource2 ? -1f : 1f);

            float source0Offset = (ReverseSource0 ? -source0Scale : 0f);
            float source1Offset = (ReverseSource1 ? -source1Scale : 0f);
            float source2Offset = (ReverseSource2 ? -source2Scale : 0f);

            if (buffer.Width * buffer.Height != field.Buffer.Length)
            {
                throw new Exception($"Buffer dimensions ({buffer.Width},{buffer.Height}) do not match array length ({field.Buffer.Length})");
            }

            int index = 0;
            for (int y = 0; y < buffer.Height; y++)
            {
                for (int x = 0; x < buffer.Width; x++)
                {
                    ColorRgbF sample = field.Buffer[index];

                    ColorRgbF result = ColorRgbF.Black;

                    result += source0.Buffer[(int)((sample.R * source0Scale) + source0Offset)];
                    result += source1.Buffer[(int)((sample.G * source1Scale) + source1Offset)];
                    result += source2.Buffer[(int)((sample.B * source2Scale) + source2Offset)];

                    buffer.Buffer[index++] += result;
                }
            }
        }
    }
}