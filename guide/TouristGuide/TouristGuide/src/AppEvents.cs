using System;
using System.Collections.Generic;
using System.Text;
using Gps;
using TouristGuide.map;

namespace TouristGuide
{
    public class AppEvents
    {
        // gps evants delegates for MainWindow
        private GpsDevice.LocationChangedEventHandler locationChangedMainWindow;
        private GpsDevice.SatellitesChangedEventHandler satellitesChangedMainWindow;
        // gps evants delegates for MapManager
        private GpsDevice.LocationChangedEventHandler locationChangedMapManager;

        public AppEvents(MainWindow mainWindow, MapManager mapManager)
        {
            // initialize gps events delegates for MainWindow
            this.locationChangedMainWindow = new GpsDevice.LocationChangedEventHandler(mainWindow.locationChanged);
            this.satellitesChangedMainWindow = new GpsDevice.SatellitesChangedEventHandler(mainWindow.satellitesChanged);
            // initialize gps events delegates for MapManager
            this.locationChangedMapManager = new GpsDevice.LocationChangedEventHandler(mapManager.newPosition);
        }

        public void subscribeToGps(GpsDevice gps)
        {
            // remove events to be sure that they will be added once
            gps.locationChanged -= this.locationChangedMainWindow;
            gps.locationChanged -= this.locationChangedMapManager;
            gps.satellitesChanged -= this.satellitesChangedMainWindow;
            // add events
            gps.locationChanged += this.locationChangedMainWindow;
            gps.locationChanged += this.locationChangedMapManager;
            gps.satellitesChanged += this.satellitesChangedMainWindow;
        }
    }
}
