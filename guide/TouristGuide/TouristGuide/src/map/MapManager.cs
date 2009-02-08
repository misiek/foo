using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Diagnostics;
using System.Drawing;

using TouristGuide.map.repository;
using TouristGuide.map.obj;
using Gps;

namespace TouristGuide.map
{
    public class MapManager
    {
        private MapPackage currentMapPkg;
        private GpsLocation currentGpsLocation;
        private ArrayList orderingPoints;

        private MapDisplayer mapDisplayer;
        public MapDisplayer MapDisplayer
        {
            set
            {
                this.mapDisplayer = value;
            }
        }

        private MapPkgRepository mapPkgRepository;
        public MapPkgRepository MapPkgRepository
        {
            set
            {
                this.mapPkgRepository = value;
            }
        }
        
       
        private PoiRepository poiRepository;
        public PoiRepository PoiRepository
        {
            set
            {
                this.poiRepository = value;
            }
        }

        public MapManager()
        {
            // 8  1  2
            // 7  0  3
            // 6  5  4
            this.orderingPoints = new ArrayList();
            this.orderingPoints.Add(new Point(1, 1));
            this.orderingPoints.Add(new Point(0, 1));
            this.orderingPoints.Add(new Point(0, 2));
            this.orderingPoints.Add(new Point(1, 2));
            this.orderingPoints.Add(new Point(2, 2));
            this.orderingPoints.Add(new Point(2, 1));
            this.orderingPoints.Add(new Point(2, 0));
            this.orderingPoints.Add(new Point(1, 0));
            this.orderingPoints.Add(new Point(0, 0));
        }

        public void newPosition(GpsLocation gpsLocation)
        {
            this.currentGpsLocation = gpsLocation;
            // DEBUG
            string coordinates = this.currentGpsLocation.getLatitudeString() + " " + 
                                 this.currentGpsLocation.getLongitudeString();
            Debug.WriteLine("new position: " + coordinates, this.ToString());
            // create current view
            MapView mv = createCurrentView();
            // display map view
            this.mapDisplayer.displayView(mv);
        }

        private MapView createCurrentView()
        {
            // TODO:
            Point imgLocation = new Point();
            Hashtable viewParts = new Hashtable();
            // create map view
            MapView mapView = new MapView(this.currentGpsLocation, imgLocation, viewParts, this.orderingPoints);
            return mapView;
        }



       
    }
}
