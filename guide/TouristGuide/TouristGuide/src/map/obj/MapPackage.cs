using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Collections;
using System.Diagnostics;

namespace TouristGuide.map.obj
{
    public class MapPackage
    {
        // Latitude eg. 50.05994539169559 N
        private double topLeftLatitude;
        // Longitude eg. 19.941730499267578 E
        private double topLeftLongitude;
        private double bottomRightLatitude;
        private double bottomRightLongitude;

        private int zoom;

        // package name - directory name
        private string name;
        // package description
        private string descr;
        // parts image format
        private string partsFormat;
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
            this.partsFormat = "";
            this.zoom = -1;
        }

        /// <summary>
        /// Check if given coordinates matches this map package.
        /// </summary>
        /// latitude=50.057 longitude=19.933
        /// this.topLeftLatitude=50.097523 this.topLeftLongitude=19.944005
        /// this.bottomRightLatitude=50.090446 this.bottomRightLongitude=19.95502
        public bool coordinatesMatches(double latitude, double longitude)
        {
            //Debug.WriteLine("MapPackage: coordinatesMatches: latitude=" + latitude +
            //                " longitude=" + longitude);
            //Debug.WriteLine("MapPackage: coordinatesMatches: this.topLeftLatitude=" + this.topLeftLatitude +
            //                " this.topLeftLongitude=" + this.topLeftLongitude +
            //                " this.bottomRightLatitude=" + this.bottomRightLatitude +
            //                " this.bottomRightLongitude=" + this.bottomRightLongitude);
            return latitude <= this.topLeftLatitude
                && latitude >= this.bottomRightLatitude
                && longitude >= this.topLeftLongitude
                && longitude <= this.bottomRightLongitude;
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

        public string getPartsFormat()
        {
            return this.partsFormat;
        }

        public void setPartsFormat(string partsFormat)
        {
            this.partsFormat = partsFormat;
        }

        public string getName()
        {
            return this.name;
        }

        public void setZoom(int zoom)
        {
            if (this.zoom == -1)
                this.zoom = zoom;
        }

        public int getZoom() {
            return this.zoom;
        }

        override public string ToString()
        {
            return "MapPackage[" + this.name + "]";
        }

    }
}
