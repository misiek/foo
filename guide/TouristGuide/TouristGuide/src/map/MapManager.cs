using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

using TouristGuide.map.repository;
using TouristGuide.map.obj;
using Gps;

namespace TouristGuide.map
{
    public class MapManager
    {
        private MapPackage currentMapPkg;
        private GpsLocation currentGpsLocation;

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


        public void newPosition(GpsLocation gpsLocation)
        {
            this.currentGpsLocation = gpsLocation;
            string coordinates = this.currentGpsLocation.getLatitudeString() + " " + 
                                 this.currentGpsLocation.getLongitudeString();
            Debug.WriteLine("new position: " + coordinates, this.ToString());
        }

    



       
    }
}
