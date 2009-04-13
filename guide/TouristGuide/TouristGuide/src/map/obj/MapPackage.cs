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

        private int width;
        private int height;

        private int max_x;
        private int max_y;

        private Hashtable tmpWidthCounting;
        private Hashtable tmpHeightCounting;

        private double heightLatitude;
        private double widthLongitude;

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
            this.width = 0;
            this.height = 0;
            this.tmpWidthCounting = new Hashtable();
            this.tmpHeightCounting = new Hashtable();
            this.max_x = -1;
            this.max_y = -1;
            this.heightLatitude = (double)Math.Abs(this.bottomRightLatitude - this.topLeftLatitude);
            this.widthLongitude = (double)Math.Abs(this.bottomRightLongitude - this.topLeftLongitude);
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
            if (!this.tmpWidthCounting.ContainsKey(p.X))
            {
                this.tmpWidthCounting[p.X] = 1;
                this.width += image.Width;
                this.max_x++;
            }
            if (!this.tmpHeightCounting.ContainsKey(p.Y))
            {
                this.tmpHeightCounting[p.Y] = 1;
                this.height += image.Height;
                this.max_y++;
            }
            this.parts.Add(p, image);
        }

        public int getMaxX()
        {
            return this.max_x;
        }

        public int getMaxY()
        {
            return this.max_y;
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
            this.width = 0;
            this.height = 0;
            this.tmpWidthCounting = new Hashtable();
            this.tmpHeightCounting = new Hashtable();
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

        public Point getPartPoint(double latitude, double longitude)
        {
            Point pixelCoordinates = getPixelCoordinates(latitude, longitude);
            // find x coordinate
            int x_width = 0;
            int x = 0;
            while (x_width < pixelCoordinates.X)
            {
                int imgWidth = getPart(new Point(x, 0)).Width;
                if (x_width + imgWidth >= pixelCoordinates.X)
                    break;
                x_width += imgWidth;
                x++;
            }
            // find y coordinate
            int y_height = 0;
            int y = 0;
            while (y_height < pixelCoordinates.Y)
            {
                int imgHeight = getPart(new Point(0, y)).Height;
                if (y_height + imgHeight >= pixelCoordinates.Y)
                    break;
                y_height += imgHeight;
                y++;
            }
            return new Point(x, y);
        }

        public Point getInsidePartPosition(double latitude, double longitude)
        {
            Point partPoint = getPartPoint(latitude, longitude);
            Point pixelCoordinates = getPixelCoordinates(latitude, longitude);
            int x_px = pixelCoordinates.X;
            for (int x = 0; x < partPoint.X; x++)
                x_px -= getPart(new Point(x, 0)).Width;
            int y_px = pixelCoordinates.Y;
            for (int y = 0; y < partPoint.Y; y++)
                y_px -= getPart(new Point(0, y)).Height;
            return new Point(x_px, y_px);
        }

        private Point getPixelCoordinates(double latitude, double longitude)
        {
            // how many degrees from left edge
            double relativeLongitude = (double)Math.Abs(longitude - this.topLeftLongitude);
            // how many deegres from bottom edge
            double relativeLatidude = (double)Math.Abs(latitude - this.bottomRightLatitude);
            // transform relative angles inside map package into pixels
            int x_px = (int)(relativeLongitude * this.width / this.widthLongitude);
            int y_px = this.height - (int)(relativeLatidude * this.height / this.heightLatitude);
            return new Point(x_px, y_px);
        }

    }
}
