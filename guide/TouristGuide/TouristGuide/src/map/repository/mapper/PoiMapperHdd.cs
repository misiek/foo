using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using TouristGuide.map.obj;
using System.Diagnostics;
using TouristGuide.map.repository.adapter;
using System.Xml;

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
            loadData(poi, poiSubDir);
            loadMedia(poi, poiSubDir);
            return poi;
        }

        /*
         * Creates new Poi instance from xml file.
         * Only name, latitude and longitude are read.
         */
        public Poi getEmpty(string poiSubDir)
        {
            Debug.WriteLine("getEmpty: poi sub dir: " + poiSubDir, ToString());
            string name = "";
            double latitude = 0;
            double longitude = 0;
            // load xml document
            XmlDocument poiXmlDoc = new XmlDocument();
            string poiXmlPath = getPoiXmlPath(poiSubDir);
            poiXmlDoc.Load(poiXmlPath);
            // get name
            XmlNode nameNode = poiXmlDoc.SelectSingleNode("/poi/name");
            name = nameNode.InnerText;
            // get latitude
            XmlNode latitudeNode = poiXmlDoc.SelectSingleNode("/poi/latitude");
            latitude = Convert.ToDouble(latitudeNode.InnerText);
            // get longitude
            XmlNode longitudeNode = poiXmlDoc.SelectSingleNode("/poi/longitude");
            longitude = Convert.ToDouble(longitudeNode.InnerText);
            // return poi instance
            return new Poi(name, latitude, longitude);
        }

        /**
         * Loads Poi basic data: descr, lang and type from xml file
         */
        public void loadData(Poi poi, string poiSubDir)
        {
            Debug.WriteLine("loadData: poi sub dir: " + poiSubDir, ToString());
            string descr = "";
            string lang = "";
            string type = "";
            // load xml document
            XmlDocument poiXmlDoc = new XmlDocument();
            string poiXmlPath = getPoiXmlPath(poiSubDir);
            poiXmlDoc.Load(poiXmlPath);
            // get descr
            XmlNode descrNode = poiXmlDoc.SelectSingleNode("/poi/descr");
            descr = descrNode.InnerText;
            // get lang
            XmlNode langNode = poiXmlDoc.SelectSingleNode("/poi/lang");
            lang = langNode.InnerText;
            // get type
            XmlNode typeNode = poiXmlDoc.SelectSingleNode("/poi/type");
            type = typeNode.InnerText;
            // insert data into poi
            poi.insertData(descr, lang, type);
        }

        /**
         * Loads Poi media data: media files and main details.
         */
        public void loadMedia(Poi poi, string poiSubDir)
        {
            Debug.WriteLine("loadMedia: poi sub dir: " + poiSubDir, ToString());
            // load xml document
            XmlDocument poiXmlDoc = new XmlDocument();
            string poiXmlPath = getPoiXmlPath(poiSubDir);
            poiXmlDoc.Load(poiXmlPath);
            // get media files
            XmlNode mediaFilesNode = poiXmlDoc.SelectSingleNode("/poi/media_files");
            if (mediaFilesNode != null)
            {
                MediaFilesXmlAdapter mfxa = new MediaFilesXmlAdapter(poi.getMediaFiles());
                mfxa.parse(mediaFilesNode);
            }
            // get main details
            XmlNode mainDetailsNode = poiXmlDoc.SelectSingleNode("/poi/details");
            if (mainDetailsNode != null)
            {
                MainDetailsXmlAdapter mdxa = new MainDetailsXmlAdapter(poi.getMainDetails());
                mdxa.parse(mainDetailsNode);
            }
            string poiMediaFilesPath = getPoiMediaFilesPath(poiSubDir);
            if (poiMediaFilesPath != null)
            {
                loadMediaFiles(poi.getAllMediaFiles(), poiMediaFilesPath);
            }
        }

        private string getPoiXmlPath(string poiSubDir)
        {
            return this.poisDir + "\\" + poiSubDir + "\\poi.xml";
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

        private string getPoiMediaFilesPath(string poiSubDir)
        {
            string mediaFilesDir = this.poisDir + "\\" + poiSubDir + "\\media_files";
            if (Directory.Exists(mediaFilesDir))
            {
                return mediaFilesDir;
            }
            return null;
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

        private void loadMediaFiles(List<MediaFile> mediaFiles, string poiMediaFilesPath)
        {
            foreach (MediaFile m in mediaFiles)
            {
                string mediaFilePath = poiMediaFilesPath + "\\" + m.getFileName();
                if (File.Exists(mediaFilePath))
                {
                    FileStream fs = File.OpenRead(mediaFilePath);
                    byte[] buffer = new byte[1024];
                    MemoryStream memStream = new MemoryStream();
                    int bytesRead = 0;
                    while ((bytesRead = fs.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        memStream.Write(buffer, 0, bytesRead);
                    }
                    byte[] mediaBytes = memStream.ToArray();
                    m.setMedia(mediaBytes);
                }
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
