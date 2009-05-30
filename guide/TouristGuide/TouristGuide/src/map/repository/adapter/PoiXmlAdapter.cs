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
            Poi p = (Poi)obj;
            StringBuilder builder = new StringBuilder();
            // opening tag
            builder.Append("<");
            builder.Append(getXmlNodeName());
            builder.Append(">");
            // lang
            builder.Append("<lang>");
            builder.Append(p.getLang());
            builder.Append("</lang>");
            // name
            builder.Append("<name>");
            builder.Append(p.getName());
            builder.Append("</name>");
            // latitude
            builder.Append("<latitude>");
            builder.Append(p.getLatitude());
            builder.Append("</latitude>");
            // longitude
            builder.Append("<longitude>");
            builder.Append(p.getLongitude());
            builder.Append("</longitude>");
            // type
            builder.Append("<type>");
            builder.Append(p.getType());
            builder.Append("</type>");
            // description
            builder.Append("<descr><![CDATA[");
            builder.Append(p.getDescr());
            builder.Append("]]></descr>");
            // media files
            MediaFilesXmlAdapter mfxa = new MediaFilesXmlAdapter(p.getMediaFiles());
            string mediaFilesXml = mfxa.serialize();
            builder.Append(mediaFilesXml);
            // main details
            MainDetailsXmlAdapter mdxa = new MainDetailsXmlAdapter(p.getMainDetails());
            string mainDetailsXml = mdxa.serialize();
            builder.Append(mainDetailsXml);
            // closing tag
            builder.Append("</");
            builder.Append(getXmlNodeName());
            builder.Append(">");
            return builder.ToString();
        }
    }
}