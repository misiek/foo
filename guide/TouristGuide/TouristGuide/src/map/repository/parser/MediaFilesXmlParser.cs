using System;
using System.Collections.Generic;
using System.Text;
using TouristGuide.map.obj;
using System.Xml;
using System.Diagnostics;

namespace TouristGuide.map.repository.parser
{
    public class MediaFilesXmlParser
    {
        private List<MediaFile> mediaFiles;
        private MediaFileXmlParser mediaFileParser;

        public MediaFilesXmlParser(List<MediaFile> mediaFiles)
        {
            this.mediaFiles = mediaFiles;
            this.mediaFileParser = new MediaFileXmlParser();
        }

        public void parse(String xmlStr)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlStr);
            XmlNode mediaFilesNode = xmlDoc.SelectSingleNode("/media_files");
            parse(mediaFilesNode);
        }

        public void parse(XmlNode mediaFilesNode)
        {
            XmlNodeList mediaFileNodes = mediaFilesNode.SelectNodes("media_file");
            foreach (XmlNode mediaFileNode in mediaFileNodes)
            {
                MediaFile mediaFile = this.mediaFileParser.parse(mediaFileNode);
                Debug.WriteLine("parse: media_file: " + mediaFile, ToString());
                this.mediaFiles.Add(mediaFile);
            }
        }
    }
}
