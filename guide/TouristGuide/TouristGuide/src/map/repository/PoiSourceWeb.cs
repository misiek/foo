using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Collections;

using TouristGuide.map;
using TouristGuide.map.obj;
using TouristGuide.map.exception;
using System.IO;
using TouristGuide.map.repository.adapter;

namespace TouristGuide.map.repository
{
    class PoiSourceWeb : PoiSource
    {
        private Portal portal;
        internal Portal Portal
        {
            set
            {
                this.portal = value;
            }
        }

        #region PoiSource Members

        public List<Poi> findPois(Area area)
        {
            List<Poi> pois = new List<Poi>();
            try
            {
                String portalResponse = this.portal.getPois(area);
                // process response
                PoisXmlAdapter poisAdapter = new PoisXmlAdapter(pois);
                poisAdapter.parse(portalResponse);


                Debug.WriteLine("findPois: after parse, pois count: " + pois.Count, ToString());
                foreach (Poi poi in pois) {
                    Debug.WriteLine("findPois: poi name: " + poi.getName(), ToString());
                    Debug.WriteLine("findPois: poi media files: " + poi.getMediaFiles().Count, ToString());
                    if (poi.getMediaFiles().Count>0)
                        Debug.WriteLine("findPois: poi media file 0: " + poi.getMediaFiles()[0].getTitle(), ToString());
                    Debug.WriteLine("findPois: poi main details: " + poi.getMainDetails().Count, ToString());
                    if (poi.getMainDetails().Count>0)
                    {
                        Debug.WriteLine("findPois: poi main detail 0: " + poi.getMainDetails()[0].getTitle(), ToString());
                        Debug.WriteLine("findPois: poi mein detail 0 - media files: " + poi.getMainDetails()[0].getMediaFiles().Count, ToString());
                    }
                    Debug.WriteLine("---------------------------------", ToString());
                }
                
            }
            catch (PortalException e)
            {
                Debug.WriteLine("findPois: portal error: " + e.Message, ToString());
            }

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
            areas["Kraków"] = new NamedArea("Kraków", 50.1046, 19.8669, 50.0233, 20.0029);

            return areas;
        }
    }
}
