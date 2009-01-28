using System;
using System.Collections.Generic;
using System.Text;

namespace TouristGuide.map.exception
{
    /*
     * Thrown when map package con't be found.
     */
    class MapNotFoundException : Exception
    {
        public MapNotFoundException(string message, Exception e)
            : base(message, e)
        { }

        public MapNotFoundException(string message) : base(message)
        { }
    }
}
