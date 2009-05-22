using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using TouristGuide.map.obj;

namespace TouristGuide.map.repository.adapter
{
    public class MediaFileXmlAdapter : ObjXmlAdapter
    {
        public MediaFileXmlAdapter()
        {
            this.xmlNodeName = "media_file";
        }

        public override object parse(XmlNode objNode)
        {
            string title = objNode.SelectSingleNode("title").InnerText.Trim();
            string descr = objNode.SelectSingleNode("descr").InnerText.Trim();
            string url = objNode.SelectSingleNode("url").InnerText.Trim();

            return new MediaFile(title, descr, new Uri(url));
        }

        public override string serialize(object obj)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
