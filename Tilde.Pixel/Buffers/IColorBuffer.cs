// Copyright (c) Tilde Love Project. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tilde.Pixel
{
    public interface IColorBuffer
    {
        Type ColorType { get; }

        int BufferSize { get; }

        int CurrentHeight { get; }
        
        int CurrentWidth { get; }

        int Width { get; set; } 
        
        int Height { get; set; }

        void CopyTo(ColorRgb[] colorRgb);
        
        void CopyTo(ColorRgba[] colorRgba);
        
        void CopyTo(ColorRgbF[] colorRgbF);
        
        void CopyTo(ColorRgbaF[] colorRgbaF);
    }
}
