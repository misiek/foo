using System;
using System.Collections.Generic;
using System.Collections;
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
        private Point centerImgLocation;
        // table with points which are keys in viewParts, determine order of displaying
        private ArrayList orderingPoints;
        // images - hashtable (Point => Bitmap)
        private Hashtable viewParts;
        // TODO: change type to more suitable
        private object[] pois;

        // images - hashtable (Point => Bitmap)
        public MapView(GpsLocation gpsLocation, Point centerImgLocation, Hashtable viewParts, ArrayList orderingPoints)
        {
            this.gpsLocation = gpsLocation;
            this.centerImgLocation = centerImgLocation;
            this.viewParts = viewParts;
            this.orderingPoints = orderingPoints;
        }

        public ArrayList getOrderingPoints()
        {
            return this.orderingPoints;
        }

        public Image getImgByPoint(Point p)
        {
            return (Image)this.viewParts[p];
        }

        public Point getCenterImgLocation()
        {
            return this.centerImgLocation;
        }

        public GpsLocation getGpsLocation()
        {
            return this.gpsLocation;
        }


    }
}
