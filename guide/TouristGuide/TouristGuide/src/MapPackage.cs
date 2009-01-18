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

        // package name - directory name
        private string name;
        // package description
        private string descr;
        // map parts
        private Hashtable parts;

        public MapPackage(string name,
                          double topLeftLatitude,
                          double topLeftLongitude,
                          double bottomRightLatitude,
                          double bottomRightLongitude)
        {
            this.name = name;
            this.topLeftLatitude = topLeftLatitude;
            this.topLeftLongitude = topLeftLongitude;
            this.bottomRightLatitude = bottomRightLatitude;
            this.bottomRightLongitude = bottomRightLongitude;
            this.parts = new Hashtable();
            this.descr = "";
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
            if (this.parts.ContainsKey(p))
                throw new Exception("Part exists: " + p);
            this.parts.Add(p, image);
        }

        public Image getPart(Point p)
        {
            return (Image)this.parts[p];
        }

        /*
         * Free memory by loosing parts.
         * When map package is not used.
         */
        public void freeParts()
        {
            this.parts = new Hashtable();
        }

        public bool isPartsFree()
        {
            if (this.parts.Count == 0)
                return true;
            return false;
        }

        public string getDescription()
        {
            return this.descr;
        }

        public void setDescription(string descr)
        {
            this.descr = descr;
        }
    }
}
