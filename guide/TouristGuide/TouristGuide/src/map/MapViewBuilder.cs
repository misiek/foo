using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

using Gps;
using TouristGuide.map.obj;
using TouristGuide.map.repository;
using System.Drawing;
using TouristGuide.util;
using TouristGuide.map.exception;
using TouristGuide.map.repository.exception;

namespace TouristGuide.map
{
    public class MapViewBuilder
    {
        private GpsLocation currentGpsLocation;
        private MapPackage currentMapPkg;
        private MapView currentMapVeiw;
        private int currentZoom;

        // hash table with point surroundings
        private PointSurroundings pointSurroundings = new PointSurroundings();
        // 8  1  2
        // 7  0  3
        // 6  5  4
        private Point[] orderingPoints = new Point[] {
            new Point(1, 1), new Point(0, 1), new Point(0, 2),
            new Point(1, 2), new Point(2, 2), new Point(2, 1),
            new Point(2, 0), new Point(1, 0), new Point(0, 0)
        };

        

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

        private Targets targets;
        public Targets Targets
        {
            set
            {
                this.targets = value;
            }
        }

        private GpsDataAnalyzer gpsDataAnalyzer;
        public GpsDataAnalyzer GpsDataAnalyzer
        {
            set
            {
                this.gpsDataAnalyzer = value;
            }
        }


        public void update(GpsLocation currentGpsLocation, MapPackage currentMapPkg, int currentZoom)
        {
            this.currentGpsLocation = currentGpsLocation;
            this.currentMapPkg = currentMapPkg;
            this.currentZoom = currentZoom;
        }

        public MapView getResult()
        {
            return this.currentMapVeiw;
        }

        public void loadPois()
        {
            // get pois
            try
            {
                List<Poi> pois = this.poiRepository.getPois(this.currentMapVeiw.getArea());
                this.currentMapVeiw.setPois(pois);
            }
            catch (NoPoiOnAreaException e)
            {
                Debug.WriteLine("No pois for view, reason: " + e.Message, this.ToString());
            }
        }

        public void loadTarget()
        {
            double latitude = this.currentGpsLocation.getLatitude();
            double longitude = this.currentGpsLocation.getLongitude();
            Poi currentTarget = this.targets.getCurrent(latitude, longitude);
            Debug.WriteLine("updateCurrentTarget: " + currentTarget, this.ToString());
            this.currentMapVeiw.setTarget(currentTarget);
        }

        public void loadCourse()
        {
            // get estimated course
            double estimatedCourse = this.gpsDataAnalyzer.estimateCourse();
            this.currentMapVeiw.setEstimatedCourse(estimatedCourse);
        }

        public void adjustCurrent()
        {
            // get current gps coordinates
            double latitude = this.currentGpsLocation.getLatitude();
            double longitude = this.currentGpsLocation.getLongitude();
            // get location (pixel coordinates) inside part image
            Point insidePartPosition = this.currentMapPkg.getInsidePartPosition(latitude, longitude);
            Debug.WriteLine("updateCurrentView *****: " + PointUtil.pointStr(insidePartPosition), ToString());
            this.currentMapVeiw.setCenterImgPosition(insidePartPosition);
            this.currentMapVeiw.setGpsLocation(this.currentGpsLocation);
        }

        // TODO: this method is too big
        public void createNew()
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
            Debug.WriteLine("partPoint: " + PointUtil.pointStr(partPoint), ToString());
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
                this.currentMapVeiw = null;
                return;
            }
            // add neighbours of center point
            foreach (DictionaryEntry entry in this.pointSurroundings)
            {
                Point directionPoint = (Point)entry.Value;
                Debug.WriteLine("directionPoint: " + PointUtil.pointStr(directionPoint), ToString());
                Point viewNeighbour = PointMath.addPoints(centerViewPoint, directionPoint);
                Point mapNeighbour = PointMath.addPoints(partPoint, directionPoint);
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
                            Debug.WriteLine(" trying to get neighbour pkg for neighbour point: " + PointUtil.pointStr(neighbourDirection), ToString());
                            // get neighbour map package
                            neighbourPkg = this.mapPkgRepository.getNeighbourMapPkg(this.currentMapPkg,
                                                neighbourDirection, this.currentZoom);
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
                            Debug.WriteLine(" getting map part from neighbour pkg for point: " + PointUtil.pointStr(mapNeighbour), this.ToString());
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
            // create map view
            MapView mapView = new MapView(this.currentGpsLocation, insidePartPosition,
                viewParts, this.orderingPoints);
            mapView.setLatitudePerPixel(this.currentMapPkg.getLatitudePerPixel());
            mapView.setLongitudePerPixel(this.currentMapPkg.getLongitudePerPixel());
            Area mapViewArea = createMapViewArea(mapView);
            Area centerImgArea = createMapViewAreaForCenterImg(mapView);
            mapView.setArea(mapViewArea);
            mapView.setCenterImgArea(centerImgArea);
            Debug.WriteLine("-----------------------------\n", ToString());
            this.currentMapVeiw = mapView;
        }

        private Area createMapViewArea(MapView mapView)
        {
            Area afci = createMapViewAreaForCenterImg(mapView);

            Image topLeftPart = mapView.getImgByPoint(new Point(0, 0));
            double latitudeTopEdgeFix = topLeftPart.Size.Height * this.currentMapPkg.getLatitudePerPixel();
            double longitudeLeftEdgeFix = topLeftPart.Size.Width * this.currentMapPkg.getLongitudePerPixel();

            Image bottomRightPart = mapView.getImgByPoint(new Point(2, 2));
            double latitudeBottomEdgeFix = bottomRightPart.Height * this.currentMapPkg.getLatitudePerPixel();
            double longitudeRghtEdgeFix = bottomRightPart.Width * this.currentMapPkg.getLongitudePerPixel();

            return new Area(afci.getTopLeftLatitude() + latitudeTopEdgeFix,
                            afci.getTopLeftLongitude() - longitudeLeftEdgeFix,
                            afci.getBottomRightLatitude() - latitudeBottomEdgeFix,
                            afci.getBottomRightLongitude() + longitudeRghtEdgeFix);
        }

        private Area createMapViewAreaForCenterImg(MapView mapView)
        {
            Point centerImgLocation = mapView.getCenterImgPosition();
            GpsLocation gpsLoc = mapView.getGpsLocation();

            int pixelsToLeftEdge = centerImgLocation.X;
            int pixelsToTopEdge = centerImgLocation.Y;
            Image centerPart = mapView.getImgByPoint(new Point(1, 1));
            int pixelsToRightEdge = centerPart.Width - centerImgLocation.X;
            int pixelsToBottomEdge = centerPart.Height - centerImgLocation.Y;

            double latitudePerPixel = this.currentMapPkg.getLatitudePerPixel();
            double longitudePerPixel = this.currentMapPkg.getLongitudePerPixel();

            double topLeftLatitude = gpsLoc.getLatitude() + pixelsToTopEdge * latitudePerPixel;
            double topLeftLongitude = gpsLoc.getLongitude() - pixelsToLeftEdge * longitudePerPixel;
            double bottomRightLatitude = gpsLoc.getLatitude() - pixelsToBottomEdge * latitudePerPixel;
            double bottomRightLongitude = gpsLoc.getLongitude() + pixelsToRightEdge * longitudePerPixel;

            return new Area(topLeftLatitude, topLeftLongitude, bottomRightLatitude, bottomRightLongitude);
        }

    }
}
