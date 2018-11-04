// Copyright (c) Tilde Love Project. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Reflection;

namespace Tilde.Pixel.Inputs
{
    public class AnalogInput8
    {
        public float[] Data { get; private set; }

        public bool Broadcast { get; set; }

        public string[] Messages { get; set; } = new string[8];

        public float CompressionSpeed { get; set; }

        public float Gain { get; set; }

        public float Threshold { get; set; }

        private float peak;

        private float scale = 1f;

        //public ObjectBinding<IPulse> TimerSource { get; private set; }

        public AnalogInput8()
        {
            Data = new float[8];

            //TimerSource = new ObjectBinding<IPulse>(this); 
        }
        /* 
                /// <inheritdoc />
                public override void Bind()
                {
                    TimingSource.Bind();

                    if (TimingSource.Value != null)
                    {
                        TimingSource.Value.Pulse += Run;
                    }
                }

                /// <inheritdoc />
                public override void Unbind()
                {
                    if (TimingSource.Value != null)
                    {
                        TimingSource.Value.Pulse -= Run;
                    }

                    TimingSource.Unbind();
                }
         */
        //[]
        //private void Audio(float a0, float a1, float a2, float a3, float a4, float a5, float a6, float a7)
        //{
        //    OscConnection.Send(OscType.GetMemberAddress(GetType(), Name.OscAddress, MethodBase.GetCurrentMethod().Name), pattern);
        //}

        public void SetMessage(int index, string message)
        {
            Messages[index] = message;
        }
        
        public void Audio(float a0, float a1, float a2, float a3, float a4, float a5, float a6, float a7)
        {
            float rescale = 1f / (1f - Threshold); 

            Data[0] = Math.Max(a0 - Threshold, 0) * rescale;
            Data[1] = Math.Max(a1 - Threshold, 0) * rescale;
            Data[2] = Math.Max(a2 - Threshold, 0) * rescale;
            Data[3] = Math.Max(a3 - Threshold, 0) * rescale;
            Data[4] = Math.Max(a4 - Threshold, 0) * rescale;
            Data[5] = Math.Max(a5 - Threshold, 0) * rescale;
            Data[6] = Math.Max(a6 - Threshold, 0) * rescale;
            Data[7] = Math.Max(a7 - Threshold, 0) * rescale;

            if (CompressionSpeed > 0)
            {
                float max = 0;

                foreach (float value in Data)
                {
                    max = Math.Max(value, max);
                }

                for (int i = 0; i < Data.Length; i++)
                {
                    Data[i] *= scale;
                }

                peak = Vector2.Lerp(new Vector2(peak), new Vector2(max), CompressionSpeed).X;

                if (peak == 0)
                {
                    scale = 1f;
                }
                else
                {
                    scale = 1f / peak;
                }
            }

            for (int i = 0; i < Data.Length; i++)
            {
                Data[i] *= Gain;
            }

//            if (Broadcast == true)
//            {
//                OscConnection.Send(OscType.GetMemberAddress(GetType(), Name.OscAddress, MethodBase.GetCurrentMethod().Name),
//                    Data[0], Data[1], Data[2], Data[3], Data[4], Data[5], Data[6], Data[7]);
//            }

            if (Messages == null)
            {
                return;
            }

//            for (int i = 0; i < 8; i++)
//            {
//                if (i > Messages.Length)
//                {
//                    return;
//                }
//
//                if (string.IsNullOrEmpty(Messages[i]) || OscAddress.IsValidAddressPattern(Messages[i]) == false)
//                {
//                    continue;
//                }
//
//                OscConnection.OscAddressManager.Invoke(new OscMessage(Messages[i], Data[i]));
//            }
        }
    }

    public static class MathExt
    {
        public static float Clamp(this float value, float min, float max)
        {
            return value < min ? min : value > max ? max : value; 
        }
    }

    public class ValueDriver
    {
        public string Message { get; set; }

        public Vector2 InputRange { get; set; }

        public Vector2 OutputRange { get; set; }

        public void Update(float value)
        {
            float actualValue = value.Clamp(InputRange.X, InputRange.Y);
            
            actualValue -= InputRange.X;
            
            actualValue *= 1f / (InputRange.Y - InputRange.X);

            actualValue = Vector2.Lerp(new Vector2(OutputRange.X), new Vector2(OutputRange.Y), actualValue).X;

            //OscConnection.OscAddressManager.Invoke(new OscMessage(Message, actualValue));
        }
    }

    public class HueDriver
    {
        public string Message { get; set; }

        public Vector2 InputRange { get; set; }

        public Vector2 OutputRange { get; set; }

        public float Saturation { get; set; }

        public float Lightness { get; set; }

        public void Update(float value)
        {
            float actualValue = value.Clamp(InputRange.X, InputRange.Y);
            
            actualValue -= InputRange.X;
            
            actualValue *= 1f / (InputRange.Y - InputRange.X);

            actualValue = Vector2.Lerp(new Vector2(OutputRange.X), new Vector2(OutputRange.Y), actualValue).X;

            ColorRgbF color = ColorFromHSV(actualValue, Saturation, Lightness);

            // OscConnection.OscAddressManager.Invoke(new OscMessage(Message, color.R, color.G, color.B));
        }

        public static void ColorToHSV(ColorRgbF colorF, out float hue, out float saturation, out float value)
        {
            ColorRgb color24 = colorF;
            Color color = Color.FromArgb(255, color24.R, color24.G, color24.B);

            int max = Math.Max(color.R, Math.Max(color.G, color.B));
            int min = Math.Min(color.R, Math.Min(color.G, color.B));

            hue = color.GetHue();
            saturation = (max == 0) ? 0 : 1f - (1f * min / max);
            value = max / 255f;
        }

        public static ColorRgbF ColorFromHSV(float hue, float saturation, float value)
        {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value = value * 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - f * saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

            ColorRgb color24;

            switch (hi)
            {
                case 0: color24 = new ColorRgb((byte)v, (byte)t, (byte)p);
                    break;
                case 1: color24 = new ColorRgb((byte)q, (byte)v, (byte)p);
                    break;
                case 2: color24 = new ColorRgb((byte)p, (byte)v, (byte)t);
                    break;
                case 3: color24 = new ColorRgb((byte)p, (byte)q, (byte)v);
                    break;
                case 4: color24 = new ColorRgb((byte)t, (byte)p, (byte)v);
                    break;
                default: color24 = new ColorRgb((byte)v, (byte)p, (byte)q);
                    break;
            }

            return color24;
        }
    }
}
