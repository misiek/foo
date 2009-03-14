using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Diagnostics;

using TouristGuide.map.obj;
using TouristGuide.map.exception;

namespace TouristGuide.map.repository
{
    public class MapPkgRepository
    {
        private MapSourceMem mapSourceMem;
        public MapSourceMem MapSourceMem
        {
            set
            {
                this.mapSourceMem = value;
            }
        }

        private MapSourceHdd mapSourceHdd;
        public MapSourceHdd MapSourceHdd
        {
            set
            {
                this.mapSourceHdd = value;
                // subscribe to map source loading map pkg event
                this.mapSourceHdd.loadingMapPkgEvent += new MapSourceHdd.LoadingMapPkg(loadingMap);
            }
        }

        private MapSourceWeb mapSourceWeb;
        public MapSourceWeb MapSourceWeb
        {
            set
            {
                this.mapSourceWeb = value;
            }
        }

        /// <summary>
        /// When loading map takes long time (source web or hdd),
        /// set this event (eg. map displayer can show loading box)
        /// </summary>
        public event LoadingMap loadingMapEvent;
        public delegate void LoadingMap(string msg);

        // set loading map event with msg
        private void loadingMap(string msg)
        {
            if (loadingMapEvent != null)
                loadingMapEvent(msg);
        }

        /// <summary>
        /// Get map package by coordinates.
        /// </summary>
        /// <returns>MapPackage instance</returns>
        public MapPackage getMapPkg(double latitude, double longitude, int zoom)
        {
            MapPackage pkg = this.mapSourceMem.findMapPkg(latitude, longitude, zoom);
            if (pkg == null)
            {
                pkg = this.mapSourceHdd.findMapPkg(latitude, longitude, zoom);
                if (pkg == null)
                {
                    pkg = this.mapSourceWeb.findMapPkg(latitude, longitude, zoom);
                }
                if (pkg != null)
                {
                    this.mapSourceMem.putMapPkg(pkg);
                }
                else
                {
                    string msg = "Map package can't be found for location: ("
                        + latitude + "; " + longitude + "), zoom: " + zoom;
                    Debug.WriteLine("MapSourceManager: getMapPkg: " + msg);
                    throw new MapNotFoundException(msg);
                }
            }
            return pkg;
        }

        // Get map package which is neighbour for given in specified direction.
        public MapPackage getNeighbourMapPkg(MapPackage pkg, Point direction, int zoom)
        {
            if (direction.X != -1 && direction.X != 0 && direction.X != 1 ||
                direction.Y != -1 && direction.Y != 0 && direction.Y != 1)
            {
                throw new MapNotFoundException("Bad direction!");
            }

            double topLeftLongitude = pkg.getTopLeftLongitude();
            double topLeftLatitude = pkg.getTopLeftLatitude();
            double bottomRightLongitude = pkg.getBottomRightLongitude();
            double bottomRightLatitude = pkg.getBottomRightLatitude();

            double longitude = (topLeftLongitude + bottomRightLongitude) / 2;
            double latitude = (topLeftLatitude + bottomRightLatitude) / 2;

            switch (direction.X)
            {
                case 1:
                    longitude = bottomRightLongitude + 0.000001;
                    break;
                case -1:
                    longitude = topLeftLongitude - 0.000001;
                    break;
            }
            // NOTE: y has inverted direction in map context
            switch (direction.Y)
            {
                case 1:
                    latitude = bottomRightLatitude - 0.000001;
                    break;
                case -1:
                    latitude = topLeftLatitude + 0.000001;
                    break;
            }

            return getMapPkg(latitude, longitude, zoom);
        }
    }
}
