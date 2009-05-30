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
            MediaFile mf = (MediaFile)obj;
            StringBuilder builder = new StringBuilder();
            // opening tag eg. <media_file>
            builder.Append("<");
            builder.Append(getXmlNodeName());
            builder.Append(">");
            // title
            builder.Append("<title>");
            builder.Append(mf.getTitle());
            builder.Append("</title>");
            // description
            builder.Append("<descr><![CDATA[");
            builder.Append(mf.getDescr());
            builder.Append("]]></descr>");
            // url
            builder.Append("<url><![CDATA[");
            builder.Append(mf.getUrl());
            builder.Append("]]></url>");
            // closing tag eg. </media_file>
            builder.Append("</");
            builder.Append(getXmlNodeName());
            builder.Append(">");
            return builder.ToString();
        }
    }
}
