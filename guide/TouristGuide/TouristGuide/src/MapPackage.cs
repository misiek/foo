using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Collections;

namespace TouristGuide
{
    public class MapPackage
    {
        private double topLeftLatitude;
        private double topLeftLongitude;
        private double bottomRightLatitude;
        private double bottomRightLongitude;

        // package name
        private string name;
        // package description
        private string descr;
        // map parts
        private Hashtable parts;

        public MapPackage(double topLeftLatitude,
                          double topLeftLongitude,
                          double bottomRightLatitude,
                          double bottomRightLongitude)
        {
            this.topLeftLatitude = topLeftLatitude;
            this.topLeftLongitude = topLeftLongitude;
            this.bottomRightLatitude = bottomRightLatitude;
            this.bottomRightLongitude = bottomRightLongitude;
            parts = new Hashtable();
        }

        /// <summary>
        /// Check if given coordinates matches this map package.
        /// </summary>
        public bool coordinatesMatches(double latitude, double longitude)
        {
            return latitude >= this.topLeftLatitude
                && latitude <= this.bottomRightLatitude
                && longitude <= this.topLeftLongitude
                && longitude >= this.bottomRightLongitude;
        }

        public void setPart(Point p, Image image)
        {
            if (parts.ContainsKey(p))
                throw new Exception("Part exists: " + p);
            parts.Add(p, image);
        }

        public Image getPart(Point p)
        {
            return (Image)parts[p];
        }
    }
}
