using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace TouristGuide.map
{
    class PointUtil
    {
        public static string pointStr(Point p)
        {
            return "(" + p.X + "; " + p.Y + ")";
        }
    }
}
