using System;
using System.Collections.Generic;
using System.Text;

namespace TouristGuide
{
    public class MapSourceMem : MapSource
    {
        #region MapSource Members

        /// <summary>
        /// Find map package by coordinates in the source.
        /// </summary>
        /// <returns>MapPackage instance.</returns>
        public MapPackage findMapPkg()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// Put map package to the source.
        /// </summary>
        public void putMapPkg()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
