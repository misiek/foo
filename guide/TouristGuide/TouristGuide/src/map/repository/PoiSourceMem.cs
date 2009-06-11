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
            if (this.cachedPois != null)
                for (int i = 0; i < this.cachedPois.Count; i++)
                {
                    Poi poi = this.cachedPois[i];
                    if (!this.cacheArea.contains(poi))
                    {
                        poi.free();
                        this.cachedPois.Remove(poi);
                    }
                }
            else
                Debug.WriteLine("setCacheArea: NULLLLLLLLLLLL", ToString());

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
                if (!this.cachedPois.Contains(poi))
                {
                    this.cachedPois.Add(poi);
                }
                else
                {
                    poi.free();
                    Debug.WriteLine("putPois: poi already cached: " + poi, ToString());
                }
            }
        }

        #endregion
    }
}
