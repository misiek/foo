using System;
using System.Collections.Generic;
using System.Text;

using TouristGuide.map.obj;

namespace TouristGuide.map.source
{
    public interface MapSource
    {
        /// <summary>
        /// Find map package by coordinates in the source.
        /// </summary>
        /// <returns>MapPackage instance.</returns>
        MapPackage findMapPkg(double latitude, double longitude, int zoom);

        /// <summary>
        /// Put map package to the source.
        /// </summary>
        void putMapPkg(MapPackage mapPkg);
    }
}
