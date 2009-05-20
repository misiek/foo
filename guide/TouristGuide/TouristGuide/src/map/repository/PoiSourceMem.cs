using System;
using System.Collections.Generic;
using System.Text;
using TouristGuide.map.obj;

namespace TouristGuide.map.repository
{
    class PoiSourceMem : PoiSource
    {
        #region PoiSource Members

        public List<Poi> findPois(Area area)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void putPois(List<Poi> pois)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
