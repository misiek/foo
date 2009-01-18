using System;
using System.Collections.Generic;
using System.Text;

namespace TouristGuide
{
    public interface MapSource
    {
        /// <summary>
        /// Find map package by coordinates in the source.
        /// </summary>
        /// <returns>MapPackage instance.</returns>
        TouristGuide.MapPackage findMapPkg(double latitude, double longitude);

        /// <summary>
        /// Put map package to the source.
        /// </summary>
        void putMapPkg(MapPackage mapPkg);
    }
}
