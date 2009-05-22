using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace TouristGuide.map.repository.adapter
{
    public abstract class ObjXmlAdapter
    {
        protected string xmlNodeName;

        public Object parse(String xmlStr)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlStr);
            XmlNode poiNode = xmlDoc.SelectSingleNode(this.xmlNodeName);
            return parse(poiNode);
        }

        public abstract Object parse(XmlNode objNode);

        public abstract string serialize(Object obj);

        public string getXmlNodeName()
        {
            return this.xmlNodeName;
        }
    }
}
