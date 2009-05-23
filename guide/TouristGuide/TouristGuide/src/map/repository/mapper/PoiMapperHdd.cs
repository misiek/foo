using System;
using System.Collections.Generic;
using System.Text;
using TouristGuide.map.obj;
using System.Diagnostics;

namespace TouristGuide.map.repository.mapper
{
    class PoiMapperHdd
    {

        private string poisDir;

        public PoiMapperHdd(string poisDir)
        {
            this.poisDir = poisDir;
        }

        public Poi get(string poiSubDir)
        {
            Poi poi = getEmpty(poiSubDir);
            loadData(poi);
            loadMedia(poi);
            return poi;
        }

        public Poi getEmpty(string poiSubDir)
        {
            return null;
        }

        public void loadData(Poi poi)
        {

        }

        public void loadMedia(Poi poi)
        {

        }

        public void save(Poi poi, string poiSubDir)
        {
            Debug.WriteLine("save: poi: " + poi + ", dir: " + poiSubDir, ToString());
        }
    }
}
