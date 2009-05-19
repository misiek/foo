using System;
using System.Collections.Generic;
using System.Text;
using TouristGuide.map.obj;
using TouristGuide.map.repository.exception;

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

        internal PoiSourceWeb PoiSourceWeb
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public List<Poi> getPois(double topLeftLatitude, double topLeftLongitude,
                            double bottomRightLatitude, double bottomRightLongitude)
        {
            if (!poisAvailable())
            {
                throw new NoPoiOnAreaException("There are not pois for current area.");
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

        public void downloadAreaPois()
        {
            throw new System.NotImplementedException();
        }

        public void setRuntimeDownload()
        {
            this.runtimeDownload = true;
        }
    }
}
