using System;
using System.Collections.Generic;
using System.Text;

namespace Gps
{
    public interface GpsDevice
    {
        event LocationChangedDelegate locationChanged;

        event SatellitesChangedDelegate satellitesChanged;

        void start();

        bool isStarted();

        void stop();
    }
}
