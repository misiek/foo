using System;
using System.Collections.Generic;
using System.Text;

namespace TouristGuide.map.exceptions
{
    /*
     * Thrown when error in MapSource
     */
    class MapSourceException : Exception
    {
        public MapSourceException(string message, Exception e) : base(message, e)
        {
        }
    }
}
