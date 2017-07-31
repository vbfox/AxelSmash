// Code adapted from Spectrum library
// https://github.com/nigel-sampson/spectrum/blob/master/Spectrum/Color.RGB.cs
//
// By Nigel Sampson, under Apache 2.0 license
// https://www.nuget.org/packages/Spectrum/

using System;

namespace AxelSmash.Colors
{
    static class ColorConversion
    {
        public static Hsl ToHsl(this Rgb rgb)
        {
            var r = rgb.R / 255.0;
            var g = rgb.G / 255.0;
            var b = rgb.B / 255.0;

            var min = (rgb.R < rgb.G ? (rgb.R < rgb.B ? rgb.R : rgb.B) : (rgb.G < rgb.B ? rgb.G : rgb.B)) / 255.0;
            var max = (rgb.R > rgb.G ? (rgb.R > rgb.B ? rgb.R : rgb.B) : (rgb.G > rgb.B ? rgb.G : rgb.B)) / 255.0;

            var delta = max - min;

            var l = (max + min) / 2.0d;
            double s;
            double h;

            if (max > 0.0d)
            {
                if (l < 0.5d)
                    s = delta / (max + min);
                else
                    s = delta / (2 - max - min);
            }
            else
            {
                s = 0;
            }

            if (Math.Abs(r - max) < 0.01)
            {
                h = (g - b) / delta;
            }
            else if (Math.Abs(g - max) < 0.01)
            {
                h = 2 + (b - r) / delta;
            }
            else
            {
                h = 4 + (r - g) / delta;
            }

            h *= 60;

            if (h < 0.0d)
                h += 360.0d;

            h = double.IsNaN(h) ? 0.0d : h;
            s = double.IsNaN(s) ? 0.0d : s;

            return new Hsl(h, s, l);
        }

        public static Rgb ToRgb(this Hsl hsl)
        {
            double v;
            double r, g, b;

            r = hsl.L;   // default to gray
            g = hsl.L;
            b = hsl.L;
            v = (hsl.L <= 0.5) ? (hsl.L * (1.0 + hsl.S)) : (hsl.L + hsl.S - hsl.L * hsl.S);

            if (v > 0)
            {
                var m = hsl.L + hsl.L - v;
                var sv = (v - m) / v;
                var hue = (hsl.H / 360.0) * 6.0;
                var sextant = (int)hue;
                var fract = hue - sextant;
                var vsf = v * sv * fract;
                var mid1 = m + vsf;
                var mid2 = v - vsf;

                switch (sextant)
                {
                    case 0:
                        r = v;
                        g = mid1;
                        b = m;
                        break;
                    case 1:
                        r = mid2;
                        g = v;
                        b = m;
                        break;
                    case 2:
                        r = m;
                        g = v;
                        b = mid1;
                        break;
                    case 3:
                        r = m;
                        g = mid2;
                        b = v;
                        break;
                    case 4:
                        r = mid1;
                        g = m;
                        b = v;
                        break;
                    case 5:
                        r = v;
                        g = m;
                        b = mid2;
                        break;
                }
            }

            return new Rgb(
                (byte)(r * 255.0),
                (byte)(g * 255.0),
                (byte)(b * 255.0));
        }
    }
}