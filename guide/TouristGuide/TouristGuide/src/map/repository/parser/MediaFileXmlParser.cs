using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using TouristGuide.map.obj;

namespace TouristGuide.map.repository.parser
{
    public class MediaFileXmlParser
    {

        public MediaFile parse(String xmlStr)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlStr);
            XmlNode mediaFileNode = xmlDoc.SelectSingleNode("/media_file");
            return parse(mediaFileNode);
        }

        public MediaFile parse(XmlNode mediaFileNode)
        {
            string title = mediaFileNode.SelectSingleNode("title").InnerText.Trim();
            string descr = mediaFileNode.SelectSingleNode("descr").InnerText.Trim();
            string url = mediaFileNode.SelectSingleNode("url").InnerText.Trim();

            return new MediaFile(title, descr, new Uri(url));
        }
    }
}
