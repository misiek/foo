using System;
using System.Collections.Generic;
using System.Text;
using TouristGuide.map.obj;
using TouristGuide.map.repository.exception;
using System.Diagnostics;

namespace TouristGuide.map.repository
{
    class PoiSourceMem : PoiSource
    {
        private Area cacheArea;
        private List<Poi> cachedPois;

        public PoiSourceMem()
        {
            this.cachedPois = new List<Poi>();
        }

        public void setCacheArea(Area cacheArea)
        {
            this.cacheArea = cacheArea;
            foreach (Poi poi in this.cachedPois)
            {
                if (!this.cacheArea.contains(poi))
                {
                    poi.free();
                    this.cachedPois.Remove(poi);
                }
            }
        }

        #region PoiSource Members

        public List<Poi> findPois(Area area)
        {
            if (this.cacheArea == null)
            {
                return null;
            }
            List<Poi> areaPois = null;
            if (this.cacheArea.contains(area))
            {
                areaPois = this.cachedPois.FindAll(
                    delegate(Poi p)
                    {
                        return area.contains(p);
                    }
                );
            }
            return areaPois;
        }

        public void putPois(List<Poi> pois)
        {
            if (this.cacheArea == null)
            {
                throw new PoiSourceException("Set cache area first.");
            }
            foreach (Poi poi in pois)
            {
                if (this.cacheArea.contains(poi) && !this.cachedPois.Contains(poi))
                {
                    this.cachedPois.Add(poi);
                }
                else
                {
                    poi.free();
                    Debug.WriteLine("putPois: poi outside cache area: " + poi, ToString());
                }
            }
        }

        #endregion
    }
}
