using System;
using System.Collections.Generic;
using System.Text;
using TouristGuide.map.obj;
using System.Xml;

namespace TouristGuide.map.repository.adapter
{
    public class MainDetailXmlAdapter : ObjXmlAdapter
    {
        public MainDetailXmlAdapter()
        {
            this.xmlNodeName = "detail";
        }

        public override object parse(XmlNode objNode)
        {
            string title = objNode.SelectSingleNode("title").InnerText.Trim();
            string descr = objNode.SelectSingleNode("descr").InnerText.Trim();
            MainDetail mainDetail = new MainDetail(title, descr);
            // parse media files
            MediaFilesXmlAdapter mediaFilesParser = new MediaFilesXmlAdapter(mainDetail.getMediaFiles());
            XmlNode mediaFilesNode = objNode.SelectSingleNode(mediaFilesParser.getXmlNodeName());
            mediaFilesParser.parse(mediaFilesNode);

            return mainDetail;
        }

        public override string serialize(object obj)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
