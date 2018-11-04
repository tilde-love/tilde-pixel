// Copyright (c) Tilde Love Project. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

using System;

namespace Tilde.Pixel.Effects
{
    public class BoxBlur : IRgbEffectRgbF
    {
        /// <inheritdoc />
        public bool Enabled { get; set; }

        public int Radius { get; set; }

        public int Iterations { get; set; }

        /// <inheritdoc />
        public void Run(ColorBuffer<ColorRgbF> buffer, float timeDelta)
        {
            if (Radius < 1)
            {
                return;
            }

            int width = buffer.Width;
            int height = buffer.Height;
            int wm = width - 1;
            int hm = height - 1;
            int wh = width * height;
            float div = (float)Radius + (float)Radius + 1f;

            for (int iteration = 0; iteration < Iterations; iteration++)
            {
                float[] r = new float[wh];
                float[] g = new float[wh];
                float[] b = new float[wh];

                float rsum, gsum, bsum;
                int x, y, i, yp, yi, yw;
                ColorRgbF p, p1, p2;

                int[] vmin = new int[Math.Max(width, height)];
                int[] vmax = new int[Math.Max(width, height)];

                ColorRgbF[] pix = buffer.Buffer;

                yw = yi = 0;

                for (y = 0; y < height; y++)
                {
                    rsum = gsum = bsum = 0;

                    for (i = -Radius; i <= Radius; i++)
                    {
                        p = pix[yi + Math.Min(wm, Math.Max(i, 0))];

                        rsum += p.R;
                        gsum += p.G;
                        bsum += p.B;
                    }

                    for (x = 0; x < width; x++)
                    {
                        r[yi] = rsum / div;
                        g[yi] = gsum / div;
                        b[yi] = bsum / div;

                        if (y == 0)
                        {
                            vmin[x] = Math.Min(x + Radius + 1, wm);
                            vmax[x] = Math.Max(x - Radius, 0);
                        }

                        p1 = pix[yw + vmin[x]];
                        p2 = pix[yw + vmax[x]];

                        rsum += p1.R - p2.R;
                        gsum += p1.G - p2.G;
                        bsum += p1.B - p2.B;

                        yi++;
                    }

                    yw += width;
                }

                for (x = 0; x < width; x++)
                {
                    rsum = gsum = bsum = 0;
                    yp = -Radius * width;

                    for (i = -Radius; i <= Radius; i++)
                    {
                        yi = Math.Max(0, yp) + x;

                        rsum += r[yi];
                        gsum += g[yi];
                        bsum += b[yi];

                        yp += width;
                    }

                    yi = x;

                    for (y = 0; y < height; y++)
                    {
                        pix[yi] = new ColorRgbF()
                        {
                            R = rsum / div,
                            G = gsum / div,
                            B = bsum / div,
                        };

                        if (x == 0)
                        {
                            vmin[y] = Math.Min(y + Radius + 1, hm) * width;
                            vmax[y] = Math.Max(y - Radius, 0) * width;
                        }

                        int t1 = x + vmin[y];
                        int t2 = x + vmax[y];

                        rsum += r[t1] - r[t2];
                        gsum += g[t1] - g[t2];
                        bsum += b[t1] - b[t2];

                        yi += width;
                    }
                }
            }
        }
    }
}
