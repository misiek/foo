using System;
using System.Collections.Generic;
using System.Text;

namespace TouristGuide.map.repository.exception
{
    /*
    * Thrown when pois there is no 
    */
    class NoPoiOnAreaException : Exception
    {
        public NoPoiOnAreaException(string message, Exception e)
            : base(message, e)
        { }

        public NoPoiOnAreaException(string message)
            : base(message)
        { }
    }
}
