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
        // relative coordinates inside center image map part (pixels)
        private Point centerImgPosition;
        // table with points which are keys in viewParts, determine order of displaying
        private ArrayList orderingPoints;
        // images - hashtable (Point => Bitmap)
        private Hashtable viewParts;
        // pois
        private List<Poi> pois;
        // center img area
        private Area centerImgArea;
        // current view area
        private Area area;

        private Poi target;

        private double latitudePerPixel;
        private double longitudePerPixel;

        // images - hashtable (Point => Bitmap)
        public MapView(GpsLocation gpsLocation, Point centerImgPosition, Hashtable viewParts, ArrayList orderingPoints)
        {
            // check if view parts contains center image
            if (viewParts[new Point(1, 1)] == null)
                throw new Exception("Can't instantiate map view without center image!");
            this.gpsLocation = gpsLocation;
            this.viewParts = viewParts;

            this.centerImgPosition = centerImgPosition;

            this.orderingPoints = orderingPoints;
        }

        public bool isValidForGpsLocation(GpsLocation gpsLoc)
        {
            return this.centerImgArea.contains(gpsLoc.getLatitude(), gpsLoc.getLongitude());
        }

        public ArrayList getOrderingPoints()
        {
            return this.orderingPoints;
        }

        public Image getImgByPoint(Point p)
        {
            return (Image)this.viewParts[p];
        }

        public Point getCenterImgPosition()
        {
            return this.centerImgPosition;
        }

        public void setCenterImgPosition(Point centerImgPosition)
        {
            this.centerImgPosition = centerImgPosition;
        }

        public Point getPositionOnImg()
        {
            Image topLeftPart = (Image)viewParts[new Point(0, 0)];
            int positionOnImgX = this.centerImgPosition.X;
            int positionOnImgY = this.centerImgPosition.Y;
            if (topLeftPart != null)
            {
                positionOnImgX += topLeftPart.Width;
                positionOnImgY += topLeftPart.Height;
            }

            return new Point(positionOnImgX, positionOnImgY);
        }

        public GpsLocation getGpsLocation()
        {
            return this.gpsLocation;
        }

        public void setGpsLocation(GpsLocation gpsLocation)
        {
            this.gpsLocation = gpsLocation;
        }

        public List<Poi> getPois()
        {
            return this.pois;
        }

        public void setPois(List<Poi> pois)
        {
            this.pois = pois;
        }

        public Point getPoiPixelCoordinates(Poi p)
        {
            double deltaLongitude = p.getLongitude() - this.area.getTopLeftLongitude();
            double deltaLatitude = this.area.getTopLeftLatitude() - p.getLatitude();

            int x = (int) Math.Floor(deltaLongitude / this.longitudePerPixel);
            int y = (int) Math.Floor(deltaLatitude / this.latitudePerPixel);
            return new Point(x, y);
        }

        public Area getArea()
        {
            return this.area;
        }

        public void setArea(Area area)
        {
            this.area = area;
        }

        public void setLatitudePerPixel(double latitudePerPixel)
        {
            this.latitudePerPixel = latitudePerPixel;
        }

        public void setLongitudePerPixel(double longitudePerPixel)
        {
            this.longitudePerPixel = longitudePerPixel;
        }


        public void setCenterImgArea(Area centerImgArea)
        {
            this.centerImgArea = centerImgArea;
        }

        internal void setTarget(Poi currentTarget)
        {
            this.target = currentTarget;
        }

        public Poi getTarget()
        {
            return this.target;
        }

        public Point getTargetPixelCoordinates()
        {
            return getPoiPixelCoordinates(this.target);
        }
    }
}
