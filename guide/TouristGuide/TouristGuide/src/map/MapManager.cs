using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Diagnostics;
using System.Drawing;

using TouristGuide.map.repository;
using TouristGuide.map.obj;
using TouristGuide.map.exception;
using Gps;

namespace TouristGuide.map
{
    public class MapManager
    {
        private MapPackage currentMapPkg;
        private GpsLocation currentGpsLocation;
        private int currentZoom;
        private ArrayList orderingPoints;

        // hash table with point surroundings
        private Hashtable pointSurroundings;      

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
            this.currentZoom = 0;
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

            this.pointSurroundings = new Hashtable();
            this.pointSurroundings["TOP"] = new Point(0, 1);
            this.pointSurroundings["BOTTOM"] = new Point(0, -1);
            this.pointSurroundings["RIGHT"] = new Point(1, 0);
            this.pointSurroundings["LEFT"] = new Point(-1, 0);
            this.pointSurroundings["TOP_RIGHT"] = new Point(1, 1);
            this.pointSurroundings["TOP_LEFT"] = new Point(-1, 1);
            this.pointSurroundings["BOTTOM_RIGHT"] = new Point(1, -1);
            this.pointSurroundings["BOTTOM_LEFT"] = new Point(-1, -1);
        }

        public void newPosition(GpsLocation gpsLocation)
        {
            this.currentGpsLocation = gpsLocation;
            // DEBUG
            string coordinates = this.currentGpsLocation.getLatitudeString() + " " + 
                                 this.currentGpsLocation.getLongitudeString();
            Debug.WriteLine("new position: " + coordinates, this.ToString());
            // when gps location is known (valid) try to get map for region and create map view
            if (gpsLocation.isValid())
            {
                try
                {
                    // get map package
                    this.currentMapPkg = this.mapPkgRepository.getMapPkg(this.currentGpsLocation.getLatitude(),
                                                            this.currentGpsLocation.getLongitude(), this.currentZoom);
                }
                catch (MapNotFoundException e)
                {
                    // no map for gps location
                    return;
                }
                // create current view when map pkg was found
                MapView mv = createCurrentView();
                // display map view
                this.mapDisplayer.displayView(mv);
            }
        }

        private MapView createCurrentView()
        {
            Hashtable viewParts = new Hashtable();
            // current gps coordinates
            double latitude = this.currentGpsLocation.getLatitude();
            double longitude = this.currentGpsLocation.getLongitude();
            Debug.WriteLine("latitude: " + latitude + ", longitude: " + longitude, this.ToString());
            // get point which indicates part image
            Point partPoint = this.currentMapPkg.getPartPoint(latitude, longitude);
            // get location inside part image
            Point insidePartPosition = this.currentMapPkg.getInsidePartPosition(latitude, longitude);
            Debug.WriteLine("insidePartPosition: " + insidePartPosition, this.ToString());
            // add center MapView point
            Point centerViewPoint = new Point(1, 1);
            viewParts[centerViewPoint] = this.currentMapPkg.getPart(partPoint);
            // add neighbours of center point
            //int x_view = -1;
            //int y_view = -1;
            foreach (DictionaryEntry entry in this.pointSurroundings)
            {
                Point viewNeighbour = addPoints(centerViewPoint, (Point)entry.Value);
                Point mapNeighbour = addPoints(partPoint, (Point)entry.Value);
                try
                {
                    viewParts[viewNeighbour] = this.currentMapPkg.getPart(mapNeighbour);
                    //if (entry.Key == "LEFT")
                    //    x_view = ((Image)viewParts[viewNeighbour]).Width + insidePartPosition.X;
                    //if (entry.Key == "TOP")
                    //    y_view = ((Image)viewParts[viewNeighbour]).Height + insidePartPosition.Y;
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Can't get neighbour(" + entry.Key + ") !", this.ToString());
                }
            }
            //// location in map view
            //if (x_view == -1)
            //    x_view = ((Image)viewParts[centerViewPoint]).Width + insidePartPosition.X;
            //if (y_view == -1)
            //    y_view = ((Image)viewParts[centerViewPoint]).Height + insidePartPosition.Y;
            //Point mapViewLocation = new Point(x_view, y_view);
            // create map view
            MapView mapView = new MapView(this.currentGpsLocation, insidePartPosition, viewParts, this.orderingPoints);
            return mapView;
        }

        private Point addPoints(Point p1, Point p2)
        {
            return new Point(p1.X + p2.X, p1.Y + p2.Y);
        }
       
    }
}
