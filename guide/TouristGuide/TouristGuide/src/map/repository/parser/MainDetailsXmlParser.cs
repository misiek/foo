using System;
using System.Collections.Generic;
using System.Text;
using TouristGuide.map.obj;
using System.Xml;
using System.Diagnostics;

namespace TouristGuide.map.repository.parser
{
    public class MainDetailsXmlParser
    {
        private List<MainDetail> mainDetails;
        private MainDetailXmlParser mainDetailParser;

        public MainDetailsXmlParser(List<MainDetail> mainDetails)
        {
            this.mainDetails = mainDetails;
            this.mainDetailParser = new MainDetailXmlParser();
        }

        public void parse(String xmlStr)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlStr);
            XmlNode node = xmlDoc.SelectSingleNode("/details");
            parse(node);
        }

        public void parse(XmlNode node)
        {
            XmlNodeList nodes = node.SelectNodes("detail");
            foreach (XmlNode childNode in nodes)
            {
                MainDetail mainDetail = this.mainDetailParser.parse(childNode);
                Debug.WriteLine("parse: detail: " + mainDetail, ToString());
                this.mainDetails.Add(mainDetail);
            }
        }
    }
}
