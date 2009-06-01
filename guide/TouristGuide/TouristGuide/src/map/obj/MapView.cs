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
        // pois list
        private List<Poi> pois;
        // current view area
        private Area area;

        // images - hashtable (Point => Bitmap)
        public MapView(GpsLocation gpsLocation, Point centerImgLocation, Hashtable viewParts, ArrayList orderingPoints)
        {
            this.gpsLocation = gpsLocation;
            this.centerImgLocation = centerImgLocation;
            this.orderingPoints = orderingPoints;
            // check if view parts contains center image
            if (viewParts[new Point(1, 1)] == null)
                throw new Exception("Can't instantiate map view without center image!");
            this.viewParts = viewParts;
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

        public List<Poi> getPois()
        {
            return this.pois;
        }

        public void setPois(List<Poi> pois)
        {
            this.pois = pois;
        }

        public Area getArea()
        {
            return this.area;
        }

        public void setArea(Area area)
        {
            this.area = area;
        }

    }
}
