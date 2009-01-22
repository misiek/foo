using System;
using System.Collections.Generic;
using System.Text;

namespace TouristGuide.exceptions
{
    /*
     * Thrown when error in MapPkgRepository
     */
    class MapPkgRepositoryException : Exception
    {
        public MapPkgRepositoryException(string message, Exception e) : base(message, e)
        {
        }
    }
}
