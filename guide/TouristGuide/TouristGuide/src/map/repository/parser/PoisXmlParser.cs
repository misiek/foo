using System;
using System.Collections.Generic;
using System.Text;
using TouristGuide.map.obj;
using System.Xml;
using System.IO;
using System.Diagnostics;

namespace TouristGuide.map.repository.parser
{
    class PoisXmlParser
    {
        private List<Poi> pois;
        private PoiXmlParser poiParser;

        public PoisXmlParser(List<Poi> pois)
        {
            this.pois = pois;
            this.poiParser = new PoiXmlParser();
        }

        public void parse(String xmlStr)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlStr);

            XmlNodeList poiNodes = xmlDoc.SelectNodes("/pois/poi");
            foreach (XmlNode poiNode in poiNodes) {
                Poi poi = this.poiParser.parse(poiNode);
                Debug.WriteLine("parse: poi: " + poi, ToString());
                this.pois.Add(poi);
            }
        }
    }
}
