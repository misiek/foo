using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Diagnostics;
using System.Collections;

namespace TouristGuide.map.repository.adapter
{
    public abstract class ListXmlAdapter
    {
        protected IList list;
        protected ObjXmlAdapter objAdapter;

        public void parse(String xmlStr)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlStr);
            XmlNode listNode = xmlDoc.SelectSingleNode(getXmlNodeName());
            parse(listNode);
        }

        public void parse(XmlNode listNode)
        {
            //Debug.WriteLine("parse: looking for elemtnts: " + this.objAdapter.getXmlNodeName(), ToString());
            XmlNodeList objNodes = listNode.SelectNodes(this.objAdapter.getXmlNodeName());
            //Debug.WriteLine("parse: found nodes: " + objNodes.Count, ToString());
            foreach (XmlNode objNode in objNodes)
            {
                Object obj = this.objAdapter.parse(objNode);
                //Debug.WriteLine("parse: obj: " + obj, ToString());
                this.list.Add(obj);
            }
        }

        public string serialize()
        {
            StringBuilder builder = new StringBuilder();
            // list opening tag
            builder.Append("<" + getXmlNodeName() + ">");
            foreach (Object obj in this.list)
            {
                string objXml = this.objAdapter.serialize(obj);
                builder.Append(objXml);
            }
            // list closing tag
            builder.Append("</" + getXmlNodeName() + ">");
            return builder.ToString();
        }

        public string getXmlNodeName()
        {
            return this.objAdapter.getXmlNodeName() + "s";
        }
    }
}
