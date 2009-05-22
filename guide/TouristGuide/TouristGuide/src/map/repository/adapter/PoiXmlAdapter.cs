using System;
using System.Collections.Generic;
using System.Text;
using TouristGuide.map.obj;
using System.Xml;

namespace TouristGuide.map.repository.adapter
{
    class PoiXmlAdapter : ObjXmlAdapter
    {

        public PoiXmlAdapter()
        {
            this.xmlNodeName = "poi";
        }

        public override object parse(XmlNode objNode)
        {
            // get required poi's parameters
            string name = objNode.SelectSingleNode("name").InnerText.Trim();
            double latitude = Convert.ToDouble(objNode.SelectSingleNode("latitude").InnerText.Trim());
            double longitude = Convert.ToDouble(objNode.SelectSingleNode("longitude").InnerText.Trim());
            string lang = objNode.SelectSingleNode("lang").InnerText.Trim();
            string type = objNode.SelectSingleNode("type").InnerText.Trim();
            string descr = objNode.SelectSingleNode("descr").InnerText.Trim();
            // instantiate poi
            Poi poi = new Poi(name, latitude, longitude, lang, type, descr);
            // parse media files

            MediaFilesXmlAdapter mediaFilesParser = new MediaFilesXmlAdapter(poi.getMediaFiles());
            XmlNode mediaFilesNode = objNode.SelectSingleNode(mediaFilesParser.getXmlNodeName());
            mediaFilesParser.parse(mediaFilesNode);

            MainDetailsXmlAdapter mainDetailsParser = new MainDetailsXmlAdapter(poi.getMainDetails());
            XmlNode mainDetailsNode = objNode.SelectSingleNode(mainDetailsParser.getXmlNodeName());
            mainDetailsParser.parse(mainDetailsNode);

            return poi;
        }

        public override string serialize(object obj)
        {
            StringBuilder builder = new StringBuilder();

            return builder.ToString();
        }
    }
}