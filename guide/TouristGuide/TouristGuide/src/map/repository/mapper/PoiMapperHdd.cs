using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using TouristGuide.map.obj;
using System.Diagnostics;
using TouristGuide.map.repository.adapter;

namespace TouristGuide.map.repository.mapper
{
    class PoiMapperHdd
    {

        private string poisDir;

        private static string xmlEnvelope = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";

        public PoiMapperHdd(string poisDir)
        {
            this.poisDir = poisDir;
        }

        public Poi get(string poiSubDir)
        {
            Poi poi = getEmpty(poiSubDir);
            loadData(poi);
            loadMedia(poi);
            return poi;
        }

        public Poi getEmpty(string poiSubDir)
        {
            return null;
        }

        public void loadData(Poi poi)
        {

        }

        public void loadMedia(Poi poi)
        {

        }

        private string createPoiXmlPath(string poiSubDir)
        {
            string poiDir = this.poisDir + "\\" + poiSubDir;

            if (!Directory.Exists(poiDir))
            {
                Directory.CreateDirectory(poiDir);
            }

            return poiDir + "\\poi.xml";
        }

        private string createPoiMediaFilesPath(string poiSubDir)
        {
            string mediaFilesDir = this.poisDir + "\\" + poiSubDir + "\\media_files";

            if (!Directory.Exists(mediaFilesDir))
            {
                Directory.CreateDirectory(mediaFilesDir);
            }

            return mediaFilesDir;
        }

        public void save(Poi poi, string poiSubDir)
        {
            Debug.WriteLine("save: poi: " + poi, ToString());
            string poiXmlFilePath = createPoiXmlPath(poiSubDir);
            string poiMediaFilesPath = createPoiMediaFilesPath(poiSubDir);
            // serialize poi
            PoiXmlAdapter pxa = new PoiXmlAdapter();
            string poiXml = pxa.serialize(poi);
            // save poi's xml
            saveXml(poiXml, poiXmlFilePath);
            // save poi's media
            saveMediaFiles(poi.getAllMediaFiles(), poiMediaFilesPath);
        }

        private void saveMediaFiles(List<MediaFile> mediaFiles, string poiMediaFilesPath)
        {
            foreach (MediaFile m in mediaFiles)
            {
                byte[] mediaBytes = m.getMedia();
                string mediaFilePath = poiMediaFilesPath + "\\" + m.getFileName();
                FileStream f = File.Create(mediaFilePath);
                f.Write(mediaBytes, 0, mediaBytes.Length);
                f.Flush();
                f.Close();
            }
        }

        private void saveXml(string xml, string pathToFile)
        {
            StreamWriter sw = File.CreateText(pathToFile);            
            sw.WriteLine(xmlEnvelope);
            sw.Write(xml);
            sw.WriteLine();
            sw.Flush();
            sw.Close();
        }
    }
}
