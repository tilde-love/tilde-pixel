// Copyright (c) Tilde Love Project. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

using System.Collections.Generic;
using System.Numerics;

namespace Tilde.Pixel.Led
{
    public struct Pixel
    {
        public Vector2 Position; 

        public int Index0, Index1, Index2, Index3; 

        public float Weight0,Weight1,Weight2, Weight3;
    }

    public interface ILedPlacement
    {
        List<Pixel> Map(BufferRgbF buffer);

        void CalculateMinMax(ref Vector2 min, ref Vector2 max); 

        void Rescale(Vector2 offset, Vector2 scale);
    }
}