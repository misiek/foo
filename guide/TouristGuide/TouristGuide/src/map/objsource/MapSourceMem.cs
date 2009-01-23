using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

using TouristGuide.map.obj;

namespace TouristGuide.map.source
{
    /*
     * Keeps only those map packages
     * which are completly loaded (with loaded images).
     */
    public class MapSourceMem : MapSource
    {
        #region MapSource Members

        /// <summary>
        /// Find map package by coordinates in the source.
        /// </summary>
        /// <returns>MapPackage instance.</returns>
        public MapPackage findMapPkg(double latitude, double longitude, int zoom)
        {
            foreach (MapPackage pkg in this.recentlyUsedMapPkgs)
            {
                if (pkg.getZoom()==zoom && pkg.coordinatesMatches(latitude, longitude))
                {
                    return pkg;
                }
            }
            Debug.WriteLine("MapSourceMem: findMapPkg: not found pkg for: ("
                + latitude + "; " + longitude + "), zoom: " + zoom);
            return null;
        }

        /// <summary>
        /// Put map package to the source.
        /// </summary>
        public void putMapPkg(MapPackage mapPkg)
        {
            this.recentlyUsedMapPkgs.Push(mapPkg);
        }

        #endregion

        private Stack<MapPackage> recentlyUsedMapPkgs;

        public MapSourceMem()
        {
            this.recentlyUsedMapPkgs = new Stack<MapPackage>(5);
        }


    }
}
