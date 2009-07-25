using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Diagnostics;
using System.Drawing;

using TouristGuide.map.repository;
using TouristGuide.map.obj;
using TouristGuide.map.exception;
using Gps;
using TouristGuide.map.repository.exception;
using TouristGuide.util;

namespace TouristGuide.map
{
    public class MapManager
    {
        private MapPackage currentMapPkg;
        private GpsLocation currentGpsLocation;
        private int currentZoom = 0;
        
        private string loadingEventMsg = "";
        private NamedArea currentNamedArea;

        private MapDisplayer mapDisplayer;
        public MapDisplayer MapDisplayer
        {
            set
            {
                this.mapDisplayer = value;
            }
        }

        private MapPkgRepository mapPkgRepository;
        public MapPkgRepository MapPkgRepository
        {
            set
            {
                this.mapPkgRepository = value;
            }
        }
        
       
        private PoiRepository poiRepository;
        public PoiRepository PoiRepository
        {
            set
            {
                this.poiRepository = value;
            }
        }

        private GpsDataAnalyzer gpsDataAnalyzer;
        public GpsDataAnalyzer GpsDataAnalyzer
        {
            set
            {
                this.gpsDataAnalyzer = value;
            }
        }

        private MapViewBuilder mapViewBuilder;
        public MapViewBuilder MapViewBuilder
        {
            set
            {
                this.mapViewBuilder = value;
            }
        }


        public void newPosition(GpsLocation gpsLocation)
        {
            // when gps location isn't known (invalid) skip processing
            if (!gpsLocation.isValid())
                return;
            // when delta location is to small skip processing
            if (!this.gpsDataAnalyzer.isLocationSignificant(gpsLocation))
                return;
            // add location to gps data analyzer
            this.gpsDataAnalyzer.addGpsLocation(gpsLocation);
            // set current gps location
            this.currentGpsLocation = gpsLocation;
            // update current map package
            updateCurrentMapPkg();
            // when there is no map for location stop processing
            if (this.currentMapPkg == null)
                return;
            // update map view builder
            this.mapViewBuilder.update(currentGpsLocation, currentMapPkg, currentZoom);
            // update current map view or create new one if current doesn't match gps location
            if (mapViewBuilder.getResult() != null
                && mapViewBuilder.getResult().isValidForGpsLocation(this.currentGpsLocation))
            {
                // when current map view match gps location update it
                mapViewBuilder.adjustCurrent();
            }
            else
            {
                // create new map view when current doesn't match gps location
                mapViewBuilder.createNew();
                if (this.loadingEventMsg != "")
                    mapLoaded();
                // find current named area for pois by current gps location
                findCurrentArea();
                // find and set pois on newly created map view
                mapViewBuilder.loadPois();
            }
            // update current map view estimated course
            mapViewBuilder.loadCourse();
            // update current map view target
            mapViewBuilder.loadTarget();

            if (mapViewBuilder.getResult() != null)
            {
                // display map view
                this.mapDisplayer.displayView(mapViewBuilder.getResult());
            }
        }

        public void downloadPois()
        {
            string msg = "Donwloading pois...";
            this.mapDisplayer.displayMessage(msg);
            Hashtable areas = this.poiRepository.getAreas();

            this.poiRepository.downloadAreaPois((NamedArea)areas["Kraków"]);
            this.mapDisplayer.hideMessage(msg);
        }

        /// <summary>
        /// Displays loading message in map panel.
        /// </summary>
        public void loadingMap(string msg)
        {
            this.loadingEventMsg = msg;
            this.mapDisplayer.displayMessage(msg);
        }

        private void mapLoaded()
        {
            this.mapDisplayer.hideMessage(this.loadingEventMsg);
            this.loadingEventMsg = "";
        }

        // TODO: should find area by current gps location,
        // but not there is only one area: Kraków
        private void findCurrentArea()
        {
            if (this.currentNamedArea == null)
            {
                Hashtable areas = this.poiRepository.getAreas();
                this.currentNamedArea = (NamedArea)areas["Kraków"];
                this.poiRepository.setCurrentArea(this.currentNamedArea);
            }
        }

        private void updateCurrentMapPkg()
        {
            double latitude = this.currentGpsLocation.getLatitude();
            double longitude = this.currentGpsLocation.getLongitude();
            // when current map pkg doesn't match, get pkg from repository
            if (!(this.currentMapPkg != null
                && this.currentMapPkg.coordinatesMatches(latitude, longitude)
                && this.currentMapPkg.getZoom() == this.currentZoom))
            {
                try
                {
                    // get map package from repository
                    this.currentMapPkg = this.mapPkgRepository.getMapPkg(latitude, longitude, this.currentZoom);
                }
                catch (MapNotFoundException e)
                {
                    this.currentMapPkg = null;
                    // map pkg for current location doesn't exist in repository
                    this.mapDisplayer.displayMessage("No map for location.", 2000, Color.Red);
                    return;
                }
            }
        }
       
    }
}
