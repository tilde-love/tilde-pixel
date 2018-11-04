// Copyright (c) Tilde Love Project. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Numerics;
using System.Reflection;
using System.Xml.Linq;

namespace Tilde.Pixel.Led
{
    public class LedMapping
    {
        public BufferRgbF Buffer { get; set; }

        public bool AutoScale { get; set; }

        public List<ILedPlacement> LedPlacements { get; }

        public Pixel[] Pixels { get; private set; }

        public LedMapping()
        {
            LedPlacements = new List<ILedPlacement>();
        }

        public void CalculateMapping()
        {
            if (Buffer == null)
            {
                return;
            }

            List<Pixel> pixels = new List<Pixel>();

            foreach (ILedPlacement placement in LedPlacements)
            {
                pixels.AddRange(placement.Map(Buffer));
            }

            Pixels = pixels.ToArray();
        }

        public void Fit()
        {
            if (LedPlacements.Count < 2)
            {
                return;
            }

            Vector2 min = new Vector2(float.MaxValue, float.MaxValue);
            Vector2 max = new Vector2(float.MinValue, float.MinValue);

            foreach (ILedPlacement placement in LedPlacements)
            {
                placement.CalculateMinMax(ref min, ref max);
            }

            Vector2 size = max - min;
            Vector2 scale = new Vector2(1f / size.X, 1f / size.Y);

            foreach (ILedPlacement placement in LedPlacements)
            {
                placement.Rescale(-min, scale);
            }

            CalculateMapping();
        }

        public void Rescale(Vector2 min, Vector2 max)
        {
            Vector2 size = max - min;
            Vector2 scale = new Vector2(1f / size.X, 1f / size.Y);

            foreach (ILedPlacement placment in LedPlacements)
            {
                placment.Rescale(-min, scale);
            }

            CalculateMapping();
        }
                
        public void Random(int count)
        {
            Random random = new Random();

            LedPlacements.Clear();

            float offsetX, offsetY;

            offsetX = 1f / (float)Buffer.Width;
            offsetY = 1f / (float)Buffer.Height;

            offsetX *= 2f;
            offsetY *= 2f;

            float scaleX = 1f - (offsetX * 2f);
            float scaleY = 1f - (offsetY * 2f);

            for (int i = 0; i < count; i++)
            {
                LedSingle led = new LedSingle
                {
                    Position = new Vector2(
                        (float) random.NextDouble() * scaleX + offsetX,
                        (float) random.NextDouble() * scaleY + offsetY
                    )
                };


                LedPlacements.Add(led);
            }

            CalculateMapping();
        }

//        public void Dump()
//        {
//            foreach (Pixel pixel in Pixels)
//            {
//                PixelLocation(pixel.Position.X, pixel.Position.Y);
//            }
//        }

//        public void PixelLocation(float x, float y)
//        {
//            //OscConnection.Reporter.PrintEmphasized(OscConnection.Current.ToString());
//            OscConnection.Send(OscType.GetMemberAddress(GetType(), Name.OscAddress, MethodBase.GetCurrentMethod().Name), x, y);
//        }

//        public override void Save(LoadContext context, XElement element)
//        {
//            base.Save(context, element);
//
//            Loader.SaveObjects(context, element, LedPlacements);
//        }

//        public override void Unbind()
//        {
//            base.Unbind();
//
//            Buffer.Unbind();
//        }
    }
}