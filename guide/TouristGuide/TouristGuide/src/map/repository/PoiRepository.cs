using System;
using System.Collections.Generic;
using System.Text;
using TouristGuide.map.obj;
using TouristGuide.map.repository.exception;
using System.Collections;
using System.Diagnostics;

namespace TouristGuide.map.repository
{
    public class PoiRepository
    {
        private bool runtimeDownload;
        // influances size of cache area
        private double cacheFactor;

        public PoiRepository()
        {
            this.runtimeDownload = false;
            this.cacheFactor = 0.5;
        }

        private PoiSourceMem poiSourceMem;
        internal PoiSourceMem PoiSourceMem
        {
            set
            {
                this.poiSourceMem = value;
            }
        }

        private PoiSourceHdd poiSourceHdd;
        internal PoiSourceHdd PoiSourceHdd
        {
            set
            {
                this.poiSourceHdd = value;
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

        public List<Poi> getPois(Area area)
        {
            List<Poi> pois = null;
            // get pois
            pois = this.poiSourceMem.findPois(area);
            if (pois == null)
            {
                Area cacheArea = newCacheArea(area);
                pois = this.poiSourceHdd.findPois(cacheArea);
                if (pois != null)
                {
                    this.poiSourceMem.setCacheArea(cacheArea);
                    this.poiSourceMem.putPois(pois);
                }
                else
                {
                    string msg = "No pois for area: " + area;
                    Debug.WriteLine("getPois: " + msg, ToString());
                    throw new NoPoiOnAreaException(msg);
                }

            }

            return pois;
        }

        // create bigger area for cache
        private Area newCacheArea(Area area)
        {
            double deltaLatitude = Math.Abs(area.getTopLeftLatitude() - area.getBottomRightLatitude());
            double deltaLongitude = Math.Abs(area.getTopLeftLongitude() - area.getBottomRightLongitude());
            double topLeftLatitude = area.getTopLeftLatitude() + this.cacheFactor * deltaLatitude;
            double topLeftLongitude = area.getTopLeftLongitude() - this.cacheFactor * deltaLongitude;
            double bottomRightLatitude = area.getBottomRightLatitude() - this.cacheFactor * deltaLatitude;
            double bottomRightLongitude = area.getBottomRightLongitude() + this.cacheFactor * deltaLongitude;
            return new Area(topLeftLatitude, topLeftLongitude, bottomRightLatitude, bottomRightLongitude);
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
