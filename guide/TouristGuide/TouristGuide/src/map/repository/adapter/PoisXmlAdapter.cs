using System;
using System.Collections.Generic;
using System.Text;

using TouristGuide.map.obj;

namespace TouristGuide.map.repository.adapter
{
    class PoisXmlAdapter : ListXmlAdapter
    {
        public PoisXmlAdapter(List<Poi> pois)
        {
            this.list = pois;
            this.objAdapter = new PoiXmlAdapter();
        }
    }
}
