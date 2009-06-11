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
using System.Drawing;

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
            }
            catch (PortalException e)
            {
                Debug.WriteLine("findPois: portal error: " + e.Message, ToString());
            }

            return pois;
        }

        public void downloadPoiMedia(Poi p)
        {
            foreach (MediaFile m in p.getAllMediaFiles())
            {
                Debug.WriteLine("downloading media file: uri: " + m.getUrl().ToString(), ToString());
                try
                {
                    byte[] mediaData = this.portal.download(m.getUrl());
                    m.setMedia(mediaData);
                }
                catch (PortalException e)
                {
                    Debug.WriteLine("loadPoiMedia: portal error: " + e.Message, ToString());
                }
            }
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
            areas["Kraków"] = new NamedArea("Krakow", 50.1046, 19.8669, 50.0233, 20.0029);

            return areas;
        }
    }
}
