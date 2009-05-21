using System;
using System.Collections.Generic;
using System.Text;
using TouristGuide.map.obj;
using System.Xml;

namespace TouristGuide.map.repository.parser
{
    public class MainDetailXmlParser
    {

        public MainDetail parse(String xmlStr)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlStr);
            XmlNode node = xmlDoc.SelectSingleNode("/detail");
            return parse(node);
        }

        public MainDetail parse(XmlNode node)
        {
            string title = node.SelectSingleNode("title").InnerText.Trim();
            string descr = node.SelectSingleNode("descr").InnerText.Trim();
            MainDetail mainDetail = new MainDetail(title, descr);
            // parse media files
            MediaFilesXmlParser mediaFilesParser = new MediaFilesXmlParser(mainDetail.getMediaFiles());
            mediaFilesParser.parse(node.SelectSingleNode("media_files"));

            return mainDetail;
        }
    }
}
