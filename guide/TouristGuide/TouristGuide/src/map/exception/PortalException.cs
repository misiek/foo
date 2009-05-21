using System;
using System.Collections.Generic;
using System.Text;

namespace TouristGuide.map.exception
{
    /*
    * Thrown when there is exception when requesting web server
    */
    class PortalException : Exception
    {
        public PortalException(string message, Exception e)
            : base(message, e)
        { }

        public PortalException(string message)
            : base(message)
        { }
    }
}
