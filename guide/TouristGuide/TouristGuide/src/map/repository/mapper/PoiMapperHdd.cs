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

        public void save(Poi poi, string poiSubDir)
        {
            string poiDir = this.poisDir + "\\" + poiSubDir;

            if (!Directory.Exists(poiDir))
            {
                Directory.CreateDirectory(poiDir);
            }

            string pathToFile = poiDir  + "\\poi.xml";

            Debug.WriteLine("save: poi: " + poi + ", path to file: " + pathToFile, ToString());

            // serialize poi
            PoiXmlAdapter pxa = new PoiXmlAdapter();
            string poiXml = pxa.serialize(poi);

            saveXml(poiXml, pathToFile);
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
