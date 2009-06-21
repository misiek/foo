using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace TouristGuide.util
{
    class PointUtil
    {
        public static string pointStr(Point p)
        {
            return "(" + p.X + "; " + p.Y + ")";
        }

        public static string pointsStr(Point[] points)
        {
            string str = "";
            foreach (Point p in points)
            {
                str += pointStr(p) + " ";
            }
            return str;
        }
    }
}
