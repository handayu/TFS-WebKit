using System;
using System.Collections.Generic;
using System.Drawing;

namespace ParserTools
{
    public static class ParserHelper
    {
        public static double Invalidation = UInt32.MaxValue;

        public static Color StringToColor(string str)
        {
            string[] p = str.Split(',');
            if (p.Length == 1)
                return Color.FromName(str);
            else
                if (p.Length == 3)
                {
                    byte r = 0;
                    byte.TryParse(p[0], out r);
                    byte g = 0;
                    byte.TryParse(p[1], out g);
                    byte b = 0;
                    byte.TryParse(p[2], out b);
                    return Color.FromArgb(r, g, b);
                }
                else
                {
                    return Color.White;
                }
        }
    }
}
