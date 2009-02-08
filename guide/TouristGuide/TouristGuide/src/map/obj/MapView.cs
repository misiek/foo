using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Gps;

namespace TouristGuide.map.obj
{
    public class MapView
    {
        // gps location
        private GpsLocation gpsLocation;
        // relative coordinates inside map view (pixels)
        private Point imgLocation;
        // 0 index - center map image (longitude and latidute are inside this image)
        // the rest as circle from top of center
        //  8  1  2
        //  7  0  3
        //  6  5  4
        private Bitmap[] images;
        // TODO: change type to more suitable
        private object[] pois;

        public MapView(GpsLocation gpsLocation, Point imgLocation, Bitmap[] images)
        {
            this.gpsLocation = gpsLocation;
            this.imgLocation = imgLocation;
            this.images = images;
        }

        public Bitmap[] getImages()
        {
            return this.images;
        }

        public Point getImgLocation()
        {
            return this.imgLocation;
        }

        public GpsLocation getGpsLocation()
        {
            return this.gpsLocation;
        }


    }
}
