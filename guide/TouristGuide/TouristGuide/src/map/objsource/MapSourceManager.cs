using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

using TouristGuide.map.obj;
using TouristGuide.map.exceptions;

namespace TouristGuide.map.source
{
    public class MapSourceManager
    {
        // when loading map takes long time (source web ?or hdd),
        // inform map displayer.
        private MapDisplayer mapDisplayer;
        public MapDisplayer MapDisplayer
        {
            set
            {
                this.mapDisplayer = value;
            }
        }

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
                    this.mapSourceMem.putMapPkg(pkg);
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
    }
}
