using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

using TouristGuide.map.obj;

namespace TouristGuide.map.repository
{
    /*
     * Keeps only those map packages
     * which are completly loaded (with loaded images).
     */
    public class MapSourceMem : MapSource
    {

        private static int CACHE_SIZE = 6;

        private List<MapPackage> recentlyUsedMapPkgs;

        public MapSourceMem()
        {
            this.recentlyUsedMapPkgs = new List<MapPackage>();
        }

        #region MapSource Members

        /// <summary>
        /// Find map package by coordinates in the source.
        /// </summary>
        /// <returns>MapPackage instance.</returns>
        public MapPackage findMapPkg(double latitude, double longitude, int zoom)
        {
            //foreach (MapPackage mapPkg in this.recentlyUsedMapPkgs)
            // iterate in reverse to start from the newest
            for(int i = this.recentlyUsedMapPkgs.Count - 1; i >= 0; i--)
            {
                MapPackage mapPkg = this.recentlyUsedMapPkgs[i];
                if (mapPkg.getZoom()==zoom && mapPkg.coordinatesMatches(latitude, longitude))
                {
                    //Debug.WriteLine("MapSourceMem: findMapPkg: found map pkg: " + mapPkg);
                    return mapPkg;
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
            if (this.recentlyUsedMapPkgs.Count >= CACHE_SIZE)
            {
                this.recentlyUsedMapPkgs[0].freeParts();
                this.recentlyUsedMapPkgs.RemoveAt(0);
            }
            this.recentlyUsedMapPkgs.Add(mapPkg);
        }

        #endregion

    }
}
