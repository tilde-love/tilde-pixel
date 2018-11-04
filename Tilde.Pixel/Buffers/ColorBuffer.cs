// Copyright (c) Tilde Love Project. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

using Tilde.Pixel.Led;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Linq;

namespace Tilde.Pixel
{
    public abstract class ColorBuffer<TColor> : IColorBuffer where TColor : struct
    {
        public TColor[] Buffer;
        private int newHeight = 1, newWidth = 1;

        /// <inheritdoc />
        public int BufferSize => Buffer.Length;

        public Type ColorType => typeof(TColor);

        public int CurrentHeight { get; private set; } = 1;
        public int CurrentWidth { get; private set; } = 1;

        public bool Debug { get; set; } = false;

        public List<IRgbEffect<ColorBuffer<TColor>>> Effects { get; }

        public int Height
        {
            get { return CurrentHeight; }
            set
            {
                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Height must be 1 or greater");
                }

                newHeight = value;
            }
        }

        public int Width
        {
            get { return CurrentWidth; }
            set
            {
                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Width must be 1 or greater");
                }

                newWidth = value;
            }
        }

        static ColorBuffer()
        {
//            OscSerializer.Register(typeof(ColorRgb), new ColorRgbOscSerializer());
//            OscSerializer.Register(typeof(ColorRgba), new ColorRgbaOscSerializer());
//            OscSerializer.Register(typeof(ColorRgbF), new ColorRgbFOscSerializer());
//            OscSerializer.Register(typeof(ColorRgbaF), new ColorRgbaFOscSerializer());
//            OscSerializer.Register(typeof(StripeDirection), new EnumSerializer<StripeDirection>());
//            OscSerializer.Register(typeof(PixelStreamFormat), new EnumSerializer<PixelStreamFormat>()); 
        }

        protected ColorBuffer()
        {
            Effects = new List<IRgbEffect<ColorBuffer<TColor>>>();

            Buffer = new TColor[Width * Height];

//            TimingSource = nIPulse>(this);
        }

        public abstract void Add(TColor color);

        public abstract void Clamp(TColor min, TColor max);

        public abstract void Clear();

        public abstract void Clear(TColor color);

        public virtual void CopyTo(ColorBuffer<TColor> otherBuffer)
        {
            Buffer.CopyTo(otherBuffer.Buffer, 0);
        }

        /// <inheritdoc />
        public abstract void CopyTo(ColorRgba[] colorRgba);

        /// <inheritdoc />
        public abstract void CopyTo(ColorRgb[] colorRgb);

        /// <inheritdoc />
        public abstract void CopyTo(ColorRgbF[] colorRgbF);

        /// <inheritdoc />
        public abstract void CopyTo(ColorRgbaF[] colorRgbaF);

        /// <inheritdoc />
        public abstract TColor GetPixelValue(Pixel pixel);

        public abstract void Max(TColor color);

        public abstract void Min(TColor color);

        public abstract void Multiply(float amount);

        public abstract void Multiply(TColor color);

        public void Run(float timeDelta)
        {
            try
            {
                if (Buffer.Length != newWidth * newHeight)
                {
                    Buffer = new TColor[newWidth * newHeight];
                }

                CurrentWidth = newWidth;
                CurrentHeight = newHeight;

                foreach (IRgbEffect<ColorBuffer<TColor>> effect in Effects)
                {
                    if (effect.Enabled == false)
                    {
                        continue; 
                    }

                    try
                    {
                        effect.Run(this, timeDelta);
                    }
                    catch (Exception ex)
                    {
                        // OscConnection.Send(OscMessages.ObjectError(effect.Name.OscAddress, ex.Message));
                    }
                }
            }
            catch (Exception ex)
            {
                // OscConnection.Send(OscMessages.ObjectError(OscType.GetMemberAddress(this.GetType(), Name.OscAddress, MethodBase.GetCurrentMethod().Name), ex.Message));
            }
        }
    }
}