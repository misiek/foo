using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Xml;
using System.IO;
using System.Diagnostics;
using System.Drawing;
using TouristGuide.map.exceptions;
using TouristGuide.map.obj;

namespace TouristGuide.map.repository
{
    public class MapPkgRepository
    {
        private String mapsDir;

        public MapPkgRepository(String mapsDir)
        {
            this.mapsDir = mapsDir;
        }

        public MapPackage getWithoutImages(string pkgName)
        {
            string pathToMapXml = this.mapsDir + "\\" + pkgName + "\\map.xml";
            MapPackage mapPkg;
            try
            {
                mapPkg = parseMapXml(pkgName, pathToMapXml);
            }
            catch (Exception e)
            {
                Debug.WriteLine("MapPkgRepository: getWithoutImages: error during xml parse!");
                throw new MapPkgRepositoryException("error during xml parse", e);
            }
            
            return mapPkg;
        }

        private MapPackage parseMapXml(string pkgName, string pathToMapXml)
        {
            // load xml document
            XmlDocument docMapXml = new XmlDocument();
            docMapXml.Load(pathToMapXml);
            // get description
            XmlNode descrNode = docMapXml.SelectSingleNode("/map/description");
            string descr = "";
            // description isn't required
            if (descrNode != null)
                descr = descrNode.InnerText;
            // get top left corner latitude
            XmlNode topLeftLatitudeNode = docMapXml.SelectSingleNode(
                                                "/map/coordinates/topLeft/latitude");
            double topLeftLatitude = Convert.ToDouble(topLeftLatitudeNode.InnerText);
            // get top left corner longitude
            XmlNode topLeftLongitudeNode = docMapXml.SelectSingleNode(
                                                "/map/coordinates/topLeft/longitude");
            double topLeftLongitude = Convert.ToDouble(topLeftLongitudeNode.InnerText);
            // get bottom right corner latitude
            XmlNode bottomRightLatitudeNode = docMapXml.SelectSingleNode(
                                                "/map/coordinates/bottomRight/latitude");
            double bottomRightLatitude = Convert.ToDouble(bottomRightLatitudeNode.InnerText);
            // get bottom right corner longitude
            XmlNode bottomRightLongitudeNode = docMapXml.SelectSingleNode(
                                                "/map/coordinates/bottomRight/longitude");
            double bottomRightLongitude = Convert.ToDouble(bottomRightLongitudeNode.InnerText);
            // get parts image format
            XmlNode partsFormatNode = docMapXml.SelectSingleNode("/map/parts/format");
            string partsFormat = "";
            // parts format aren't required
            if (partsFormatNode != null)
                partsFormat = partsFormatNode.InnerText;
            MapPackage mapPkg = new MapPackage(pkgName, topLeftLatitude, topLeftLongitude,
                                    bottomRightLatitude, bottomRightLongitude);
            if (partsFormat != "")
                mapPkg.setPartsFormat(partsFormat);
            return mapPkg;
        }

        public void loadImages(MapPackage pkg)
        {
            Hashtable parts = new Hashtable();
            try
            {
                string pathToParts = this.mapsDir + "\\" + pkg.getName() + "\\parts";
                DirectoryInfo partsDirInfo = new DirectoryInfo(pathToParts);
                string searchPattern = "*";
                if (pkg.getPartsFormat() != "")
                    searchPattern += "." + pkg.getPartsFormat();
                foreach (FileInfo partFileInfo in partsDirInfo.GetFiles(searchPattern))
                {
                    String partFileName = partFileInfo.Name;
                    // remove extension
                    char[] s = ".".ToCharArray();
                    String name = partFileName.Split(s)[0];
                    // get part coordinates (not geografical)
                    s = "_".ToCharArray();
                    String[] pointStr = name.Split(s);
                    Point p = new Point(Convert.ToInt32(pointStr[0]), Convert.ToInt32(pointStr[1]));
                    Bitmap img = new Bitmap(partFileInfo.ToString());
                    parts[p] = img;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("MapPkgRepository: loadImages: error during images load!");
                throw new MapPkgRepositoryException("error during images load", e);
            }
            try
            {
                // after successfully loading parts set them in map pkg
                foreach (DictionaryEntry part in parts)
                {
                    pkg.setPart((Point)part.Key, (Bitmap)part.Value);
                }
            }
            catch (Exception e)
            {
                pkg.freeParts();
                Debug.WriteLine("MapPkgRepository: loadImages: error during images put to map pkg!");
                throw new MapPkgRepositoryException("error during images put to map pkg", e);
            }
        }

        public void save(MapPackage pkg)
        {
            throw new System.NotImplementedException();
        }
    }
}
