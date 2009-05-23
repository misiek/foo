using System;
using System.Collections.Generic;
using System.Text;
using TouristGuide.map.obj;
using TouristGuide.map.repository.mapper;

namespace TouristGuide.map.repository
{
    class PoiSourceHdd : PoiSource
    {
        private List<Poi> pois;
        private PoiMapperHdd poiMapperHdd;
        private string poisDir;

        public PoiSourceHdd(string poisDir, PoiMapperHdd poiMapperHdd)
        {
            this.poisDir = poisDir;
            this.poiMapperHdd = poiMapperHdd;
            this.pois = new List<Poi>();

        }

        // loads pois without media - fast search
        private void loadPoisFromHdd()
        {
            //this.pois.Add(...
        }

        #region PoiSource Members

        public List<Poi> findPois(Area area)
        {
            List<Poi> areaPois =  this.pois.FindAll(
                delegate(Poi p) { 
                    return p.getLatitude() <= area.getTopLeftLatitude() &&
                           p.getLatitude() >= area.getBottomRightLatitude() &&
                           p.getLongitude() >= area.getTopLeftLongitude() &&
                           p.getLongitude() <= area.getBottomRightLongitude();
                }
            );
            foreach (Poi p in areaPois)
            {
                if (p.isDataFree())
                {
                    this.poiMapperHdd.loadData(p);
                }
            }

            return areaPois;
        }

        public void putPois(List<Poi> pois)
        {
            foreach (Poi p in pois)
            {
                if (!p.isDataFree())
                {
                    this.poiMapperHdd.save(p);
                }
            }
            this.pois.AddRange(pois);
        }

        #endregion


        


    }
}
