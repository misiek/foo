using System;
using System.Collections.Generic;
using System.Text;
using Gps;
using TouristGuide.gui;
using TouristGuide.map;
using TouristGuide.map.repository;

namespace TouristGuide
{
    public class AppEvents
    {
        // gps evants delegates for MainWindow
        private GpsDevice.LocationChangedEventHandler locationChangedMainWindow;
        private GpsDevice.SatellitesChangedEventHandler satellitesChangedMainWindow;
        // gps evants delegates for MapManager
        private GpsDevice.LocationChangedEventHandler locationChangedMapManager;
        // map pkg repository events for MapManager
        private MapPkgRepository.LoadingMap loadingMapToMapManager;

        public AppEvents(MainWindow mainWindow, MapManager mapManager, MapPkgRepository mapPkgRepo)
        {
            // initialize gps events delegates for MainWindow
            this.locationChangedMainWindow = new GpsDevice.LocationChangedEventHandler(mainWindow.locationChanged);
            this.satellitesChangedMainWindow = new GpsDevice.SatellitesChangedEventHandler(mainWindow.satellitesChanged);
            // initialize gps events delegates for MapManager
            this.locationChangedMapManager = new GpsDevice.LocationChangedEventHandler(mapManager.newPosition);
            // initialize map pkg repository event delegate for MapManager
            this.loadingMapToMapManager = new MapPkgRepository.LoadingMap(mapManager.loadingMap);
            // subscribe to map pkg repository loading event
            mapPkgRepo.loadingMapEvent += this.loadingMapToMapManager;
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
