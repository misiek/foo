using System;
using System.Collections.Generic;
using System.Text;

namespace TouristGuide.map.repository.exception
{
    /*
    * Thrown when pois there is no 
    */
    class PoiSourceException : Exception
    {
        public PoiSourceException(string message, Exception e)
            : base(message, e)
        { }

        public PoiSourceException(string message)
            : base(message)
        { }
    }
}
