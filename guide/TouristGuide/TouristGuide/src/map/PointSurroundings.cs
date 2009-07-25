using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Collections;

namespace TouristGuide.map
{

    public class PointSurroundings : Hashtable
    {
        public PointSurroundings()
        {
            //this.Add("hello", "world");
            //this.Add("bill", "laimbeer");
            //this.Add("bill", "lumbergh");
            this["TOP"] = new Point(0, 1);
            this["BOTTOM"] = new Point(0, -1);
            this["RIGHT"] = new Point(1, 0);
            this["LEFT"] = new Point(-1, 0);
            this["TOP_RIGHT"] = new Point(1, 1);
            this["TOP_LEFT"] = new Point(-1, 1);
            this["BOTTOM_RIGHT"] = new Point(1, -1);
            this["BOTTOM_LEFT"] = new Point(-1, -1);
        }
    }

}
