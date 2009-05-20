using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Collections;

using TouristGuide.map;
using TouristGuide.map.obj;

namespace TouristGuide.map.repository
{
    class PoiSourceWeb : PoiSource
    {
        internal Portal Portal
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        #region PoiSource Members

        public List<Poi> findPois(Area area)
        {
            List<Poi> pois = new List<Poi>();

            string portalResponse = this.Portal.getPois(area);
            Debug.WriteLine("findPois: portalResponse: " + portalResponse, ToString());

            return pois;
        }

        public void putPois(List<Poi> pois)
        {
            throw new Exception("The method or operation is not allowed.");
        }

        #endregion

        // TODO: should get list of areas from server
        public Hashtable findAreas()
        {
            Hashtable areas = new Hashtable();
            areas["Kraków"] = new Area("Kraków", 50.1046, 19.8669, 50.0233, 20.0029);

            return areas;
        }
    }
}
