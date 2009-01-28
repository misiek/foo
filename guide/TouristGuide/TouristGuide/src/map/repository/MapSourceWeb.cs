using System;
using System.Collections.Generic;
using System.Text;

using TouristGuide.map.obj;

namespace TouristGuide.map.repository
{
    public class MapSourceWeb : MapSource
    {
        #region MapSource Members

        /// <summary>
        /// Find map package by coordinates in the source.
        /// </summary>
        /// <returns>MapPackage instance.</returns>
        public MapPackage findMapPkg(double latitude, double longitude, int zoom)
        {
            return null;
        }

        /// <summary>
        /// Put map package to the source.
        /// </summary>
        public void putMapPkg(MapPackage mapPkg)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
