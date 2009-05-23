using System;
using System.Collections.Generic;
using System.Text;

namespace TouristGuide.map.obj
{
    public class NamedArea : Area
    {
        private string name;

        public NamedArea(string name, double topLeftLatitude, double topLeftLongitude,
                         double bottomRightLatitude, double bottomRightLongitude)
            : base(topLeftLatitude, topLeftLongitude, bottomRightLatitude, bottomRightLongitude)
        {
            this.name = name;
        }

        public string getName()
        {
            return this.name;
        }

    }
}
