using System;
using System.Collections.Generic;
using System.Text;
using TouristGuide.map.obj;
using System.Xml;

namespace TouristGuide.map.repository.parser
{
    class PoiXmlParser
    {

        public Poi parse(String xmlStr)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlStr);
            XmlNode poiNode = xmlDoc.SelectSingleNode("/poi");
            return parse(poiNode);
        }

        public Poi parse(XmlNode poiNode)
        {
            // get required poi's parameters
            string name = poiNode.SelectSingleNode("name").InnerText.Trim();
            double latitude = Convert.ToDouble(poiNode.SelectSingleNode("latitude").InnerText.Trim());
            double longitude = Convert.ToDouble(poiNode.SelectSingleNode("longitude").InnerText.Trim());
            string lang = poiNode.SelectSingleNode("lang").InnerText.Trim();
            string type = poiNode.SelectSingleNode("type").InnerText.Trim();
            string descr = poiNode.SelectSingleNode("descr").InnerText.Trim();
            // instantiate poi
            Poi poi = new Poi(name, latitude, longitude, lang, type, descr);
            // parse media files
            MediaFilesXmlParser mediaFilesParser = new MediaFilesXmlParser(poi.getMediaFiles());
            mediaFilesParser.parse(poiNode.SelectSingleNode("media_files"));

            MainDetailsXmlParser mainDetailsParser = new MainDetailsXmlParser(poi.getMainDetails());
            mainDetailsParser.parse(poiNode.SelectSingleNode("details"));

            return poi;
        }
    }
}
