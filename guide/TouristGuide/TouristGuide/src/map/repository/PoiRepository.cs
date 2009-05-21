using System;
using System.Collections.Generic;
using System.Text;
using TouristGuide.map.obj;
using TouristGuide.map.repository.exception;
using System.Collections;

namespace TouristGuide.map.repository
{
    public class PoiRepository
    {
        private bool runtimeDownload;

        public PoiRepository()
        {
            this.runtimeDownload = false;
        }

        internal PoiSourceMem PoiSourceMem
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        internal PoiSourceHdd PoiSourceHdd
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        private PoiSourceWeb poiSourceWeb;
        internal PoiSourceWeb PoiSourceWeb
        {
            set
            {
                this.poiSourceWeb = value;
            }
        }

        public List<Poi> getPois(double topLeftLatitude, double topLeftLongitude,
                            double bottomRightLatitude, double bottomRightLongitude)
        {
            if (!poisAvailable())
            {
                throw new NoPoiOnAreaException("There are no pois for current area.");
            }
            List<Poi> pois = new List<Poi>();
            // get pois
            return pois;
        }

        private bool poisAvailable()
        {
            // TODO:
            return false;
        }

        public void setRuntimeDownload()
        {
            this.runtimeDownload = true;
        }


        // TODO: should download pois for specified area
        public void downloadAreaPois(Area area)
        {
            // download pois from the web server
            List<Poi> pois = this.poiSourceWeb.findPois(area);
            // save downloaded pois
            //this.PoiSourceMem.putPois(pois);
            //this.PoiSourceHdd.putPois(pois);
        }

        public Hashtable getAreas()
        {
            return this.poiSourceWeb.findAreas();
        }
    }
}
