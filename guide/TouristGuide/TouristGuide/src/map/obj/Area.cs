using System;
using System.Collections.Generic;
using System.Text;

namespace TouristGuide.map.obj
{
    public class Area
    {
        private string name;
        private double topLeftLatitude;
        private double topLeftLongitude;
        private double bottomRightLatitude;
        private double bottomRightLongitude;


        public Area(double topLeftLatitude, double topLeftLongitude,
                    double bottomRightLatitude, double bottomRightLongitude)
        {
            this.topLeftLatitude = topLeftLatitude;
            this.topLeftLongitude = topLeftLongitude;
            this.bottomRightLatitude = bottomRightLatitude;
            this.bottomRightLongitude = bottomRightLongitude;
        }

        public Area(string name, double topLeftLatitude, double topLeftLongitude,
                    double bottomRightLatitude, double bottomRightLongitude)
        {
            this.name = name;
            this.topLeftLatitude = topLeftLatitude;
            this.topLeftLongitude = topLeftLongitude;
            this.bottomRightLatitude = bottomRightLatitude;
            this.bottomRightLongitude = bottomRightLongitude;
        }

        public string getName()
        {
            return this.name;
        }

        public double getTopLeftLatitude()
        {
            return this.topLeftLatitude;
        }

        public double getTopLeftLongitude()
        {
            return this.topLeftLongitude;
        }

        public double getBottomRightLatitude()
        {
            return this.bottomRightLatitude;
        }

        public double getBottomRightLongitude()
        {
            return this.bottomRightLongitude;
        }
    }
}
