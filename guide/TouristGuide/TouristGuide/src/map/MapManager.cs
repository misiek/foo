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
using TouristGuide.map.repository.exception;

namespace TouristGuide.map
{
    public class MapManager
    {
        private const double MIN_DELTA_LOCATION = 0.00001;

        private MapPackage currentMapPkg;
        private GpsLocation currentGpsLocation;
        private int currentZoom;
        private ArrayList orderingPoints;
        private string loadingEventMsg;
        private NamedArea currentNamedArea;

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
            this.loadingEventMsg = "";
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

        public void downloadPois()
        {
            string msg = "Donwloading pois...";
            this.mapDisplayer.displayMessage(msg);
            Hashtable areas = this.poiRepository.getAreas();

            this.poiRepository.downloadAreaPois((NamedArea)areas["Kraków"]);
            this.mapDisplayer.hideMessage(msg);
        }

        /// <summary>
        /// Displays loading message in map panel.
        /// </summary>
        public void loadingMap(string msg)
        {
            this.loadingEventMsg = msg;
            this.mapDisplayer.displayMessage(msg);
        }

        private void mapLoaded()
        {
            this.mapDisplayer.hideMessage(this.loadingEventMsg);
            this.loadingEventMsg = "";
        }

        // TODO: should find area by current gps location,
        // but not there is only one area: Kraków
        private void findCurrentArea()
        {
            if (this.currentNamedArea == null)
            {
                Hashtable areas = this.poiRepository.getAreas();
                this.currentNamedArea = (NamedArea)areas["Kraków"];
                this.poiRepository.setCurrentArea(this.currentNamedArea);
            }
        }

        public void newPosition(GpsLocation gpsLocation)
        {
            // when gps location is known (valid) try to get map for region and create map view
            if (gpsLocation.isValid())
            {
                if (this.currentGpsLocation != null){
                    double deltaLocation = getLocationsDistance(this.currentGpsLocation, gpsLocation);
                    // if distance is to small skip processing
                    if (deltaLocation < MIN_DELTA_LOCATION)
                    {
                        Debug.WriteLine("delta location to small.", this.ToString());
                        return;
                    }
                }
                // set current gps location
                this.currentGpsLocation = gpsLocation;
                // find current named area for pois by current gps location
                findCurrentArea();
                double latitude = this.currentGpsLocation.getLatitude();
                double longitude = this.currentGpsLocation.getLongitude();
                // when current map pkg doesn't match get pkg from repository
                if (!(this.currentMapPkg != null 
                    && this.currentMapPkg.coordinatesMatches(latitude, longitude)
                    && this.currentMapPkg.getZoom() == this.currentZoom))
                {
                    try
                    {
                        // get map package from repository
                        this.currentMapPkg = this.mapPkgRepository.getMapPkg(latitude, longitude, this.currentZoom);
                    }
                    catch (MapNotFoundException e)
                    {
                        // map pkg for current location doesn't exist in repository
                        this.mapDisplayer.displayMessage("No map for location.", 2000, Color.Red);
                        return;
                    }
                }
                // create current view when map pkg was found
                MapView mv = createCurrentView();
                // get pois
                try
                {
                    List<Poi> pois = this.poiRepository.getPois(mv.getArea());
                    Debug.WriteLine("Found pois:", this.ToString());
                    foreach (Poi p in pois)
                    {
                        Debug.WriteLine(" *** " + p, this.ToString());
                    }
                    mv.setPois(pois);
                }
                catch (NoPoiOnAreaException e)
                {
                    Debug.WriteLine("No pois for view, reason: " + e.Message, this.ToString());
                }
                if (mv != null)
                {
                    // display map view
                    this.mapDisplayer.displayView(mv);
                }
            }
        }


        private double getLocationsDistance(GpsLocation gl1, GpsLocation gl2)
        {
            // distance between two points
            return Math.Sqrt(Math.Pow(gl2.getLatitude() - gl1.getLatitude(), 2) + 
                                Math.Pow(gl2.getLongitude() - gl1.getLongitude(), 2));
        }

        // for debugging
        private string pointStr(Point p)
        {
            return "(" + p.X + "; " + p.Y + ")";
        }

        private MapView createCurrentView()
        {
            Debug.WriteLine("----- createCurrentView -----", ToString());
            // container to keep map parts which create current map view
            Hashtable viewParts = new Hashtable();
            // get current gps coordinates
            double latitude = this.currentGpsLocation.getLatitude();
            double longitude = this.currentGpsLocation.getLongitude();
            //Debug.WriteLine("latitude: " + latitude + ", longitude: " + longitude, this.ToString());
            // get point which indicates part image for the location
            Point partPoint = this.currentMapPkg.getPartPoint(latitude, longitude);
            Debug.WriteLine("partPoint: " + pointStr(partPoint), ToString());
            // get location (pixel coordinates) inside part image
            Point insidePartPosition = this.currentMapPkg.getInsidePartPosition(latitude, longitude);
            //Debug.WriteLine("insidePartPosition: (" + insidePartPosition.X + "; " + insidePartPosition.Y + ")", this.ToString());
            // add center MapView point
            Point centerViewPoint = new Point(1, 1);
            viewParts[centerViewPoint] = this.currentMapPkg.getPart(partPoint);
            // map view has only sense when there is center map part
            if (viewParts[centerViewPoint] == null)
            {
                Debug.WriteLine("Can't get center part! viewParts[centerViewPoint] is null - skipping.", this.ToString());
                // return when center part is null
                return null;
            }
            // add neighbours of center point
            foreach (DictionaryEntry entry in this.pointSurroundings)
            {
                Point directionPoint = (Point)entry.Value;
                Debug.WriteLine("directionPoint: " + pointStr(directionPoint), ToString());
                Point viewNeighbour = addPoints(centerViewPoint, directionPoint);
                Point mapNeighbour = addPoints(partPoint, directionPoint);
                try
                {
                    viewParts[viewNeighbour] = this.currentMapPkg.getPart(mapNeighbour);
                    if (viewParts[viewNeighbour] == null)
                    {
                        Debug.WriteLine("looking for view neighbour:", this.ToString());
                        MapPackage neighbourPkg = null;
                        int neighbourDirectionX = 0;
                        if (mapNeighbour.X > this.currentMapPkg.getMaxX())
                            neighbourDirectionX = 1;
                        if (mapNeighbour.X < 0)
                            neighbourDirectionX = -1;
                        int neighbourDirectionY = 0;
                        if (mapNeighbour.Y > this.currentMapPkg.getMaxY())
                            neighbourDirectionY = 1;
                        if (mapNeighbour.Y < 0)
                            neighbourDirectionY = -1;
                        Point neighbourDirection = new Point(neighbourDirectionX, neighbourDirectionY);
                        try
                        {
                            Debug.WriteLine(" trying to get neighbour pkg for neighbour point: " + pointStr(neighbourDirection), ToString());
                            // get neighbour map package
                            neighbourPkg = this.mapPkgRepository.getNeighbourMapPkg(this.currentMapPkg,
                                                neighbourDirection , this.currentZoom);
                        }
                        catch (MapNotFoundException e)
                        {
                            Debug.WriteLine(" Can't get neighbour pkg for: " + this.currentMapPkg
                                            + ", dircetion: (" + directionPoint.X + "; " + directionPoint.Y + "), ERROR: " + e.Message, this.ToString());
                        }
                        if (neighbourPkg != null)
                        {
                            if (mapNeighbour.X > this.currentMapPkg.getMaxX())
                                mapNeighbour.X = 0;
                            if (mapNeighbour.X < 0)
                                mapNeighbour.X = neighbourPkg.getMaxX();
                            if (mapNeighbour.Y > this.currentMapPkg.getMaxY())
                                mapNeighbour.Y = 0;
                            if (mapNeighbour.Y < 0)
                                mapNeighbour.Y = neighbourPkg.getMaxY();
                            Debug.WriteLine(" getting map part from neighbour pkg for point: " + pointStr(mapNeighbour), this.ToString());
                            viewParts[viewNeighbour] = neighbourPkg.getPart(mapNeighbour);
                        }
                    }
                    else
                        Debug.WriteLine("view neighbour ok", this.ToString());
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Can't get neighbour(" + entry.Key + ")! ERROR: " + e.Message, this.ToString());
                }
            }
            if (this.loadingEventMsg != "")
                mapLoaded();
            // create map view
            MapView mapView = new MapView(this.currentGpsLocation, insidePartPosition, 
                viewParts, this.orderingPoints);
            Area mapViewArea = countMapViewArea(mapView);
            mapView.setArea(mapViewArea);
            mapView.setLatitudePerPixel(this.currentMapPkg.getLatitudePerPixel());
            mapView.setLongitudePerPixel(this.currentMapPkg.getLongitudePerPixel());
            Debug.WriteLine("-----------------------------\n", ToString());
            return mapView;
        }

        private Area countMapViewArea(MapView mapView)
        {

            Point centerImgLocation = mapView.getPositionOnCenterImgPart();
            GpsLocation gpsLoc = mapView.getGpsLocation();

            Image topLeftPart = mapView.getImgByPoint(new Point(0, 0));
            int pixelsToLeftEdge = topLeftPart.Size.Width + centerImgLocation.X;
            int pixelsToTopEdge = topLeftPart.Size.Height + centerImgLocation.Y;
            Image bottomRightPart = mapView.getImgByPoint(new Point(2, 2));
            Image centerPart = mapView.getImgByPoint(new Point(1, 1));
            int pixelsToRightEdge = bottomRightPart.Width + centerPart.Width - centerImgLocation.X;
            int pixelsToBottomEdge = bottomRightPart.Width + centerPart.Width - centerImgLocation.X;

            double latitudePerPixel = this.currentMapPkg.getLatitudePerPixel();
            double longitudePerPixel = this.currentMapPkg.getLongitudePerPixel();

            double topLeftLatitude = gpsLoc.getLatitude() + pixelsToTopEdge * latitudePerPixel;
            double topLeftLongitude = gpsLoc.getLongitude() - pixelsToLeftEdge * longitudePerPixel;
            double bottomRightLatitude = gpsLoc.getLatitude() - pixelsToBottomEdge * latitudePerPixel;
            double bottomRightLongitude = gpsLoc.getLongitude() + pixelsToRightEdge * longitudePerPixel;

            return new Area(topLeftLatitude, topLeftLongitude, bottomRightLatitude, bottomRightLongitude);
        }

        private Point addPoints(Point p1, Point p2)
        {
            return new Point(p1.X + p2.X, p1.Y + p2.Y);
        }
       
    }
}
