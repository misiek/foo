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

        // checks if given area is sub area of current area instance
        public bool contains(Area area)
        {
            return this.topLeftLatitude >= area.getTopLeftLatitude() &&
                   this.topLeftLongitude <= area.getTopLeftLongitude() &&
                   this.bottomRightLatitude <= area.getBottomRightLatitude() &&
                   this.bottomRightLongitude >= area.getBottomRightLongitude();
        }

        // check if given poi is inside area
        public bool contains(Poi poi)
        {
            return this.topLeftLatitude >= poi.getLatitude() &&
                   this.topLeftLongitude <= poi.getLongitude() &&
                   this.bottomRightLatitude <= poi.getLatitude() &&
                   this.bottomRightLongitude >= poi.getLongitude();
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
