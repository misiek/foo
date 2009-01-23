using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;

using TouristGuide.map.exceptions;
using TouristGuide.map.repository;
using TouristGuide.map.obj;

namespace TouristGuide.map.source
{
    public class MapSourceHdd : MapSource
    {
        private string mapsDir;
        private MapPkgRepository mapPkgRepository;
        private List<MapPackage> availableMapPkgs;

        private int zoom;

        public MapSourceHdd(string mapsDir, MapPkgRepository mapPkgRepository)
        {
            this.mapsDir = mapsDir;
            this.mapPkgRepository = mapPkgRepository;
            this.zoom = 0;
            readMapsDir();
        }

        /*
         * Create available map packages list
         * by reading maps directory.
         */
        public void readMapsDir()
        {
            this.availableMapPkgs = new List<MapPackage>();
            string zoomMapsDir = this.mapsDir + "\\zoom_" + this.zoom;
            DirectoryInfo[] dirs;
            try
            {
                DirectoryInfo dirInfo = new DirectoryInfo(zoomMapsDir);
                dirs = dirInfo.GetDirectories();
            }
            catch (Exception e)
            {
                Debug.WriteLine("MapSourceHdd: readMapsDir: can't read zoom maps dir: " + zoomMapsDir);
                throw new MapSourceException("error reading zoom maps dir", e);
            }
            foreach (DirectoryInfo dir in dirs)
            {
                FileInfo[] mapFileInfo = dir.GetFiles("map.xml");
                if (mapFileInfo.Length > 0)
                {
                    Debug.WriteLine("MapSourceHdd: readMapsDir(): found map package, dir: " + dir.Name);
                    string mapPkgName = dir.Name;
                    try
                    {
                        MapPackage mapPkg = this.mapPkgRepository.getWithoutImages(mapPkgName, this.zoom);
                        this.availableMapPkgs.Add(mapPkg);
                        Debug.WriteLine("MapSourceHdd: readMapsDir: added map packege '" + mapPkgName + "'");
                    }
                    catch (MapPkgRepositoryException e) 
                    {
                        Debug.WriteLine("MapSourceHdd: readMapsDir: couldn't add map packege '" 
                                + mapPkgName + "' due to error: " + e.Message);
                        throw new MapSourceException("error adding map package", e);
                    }
                }
            }
        }

        #region MapSource Members

        /// <summary>
        /// Find map package by coordinates in the source.
        /// </summary>
        /// <returns>MapPackage instance.</returns>
        public MapPackage findMapPkg(double latitude, double longitude, int zoom)
        {
            if (this.zoom != zoom)
                changeZoom(zoom);
            foreach (MapPackage mapPkg in availableMapPkgs)
            {
                Debug.WriteLine("MapSourceHdd: findMapPkg: checking: " + mapPkg);
                if (mapPkg.coordinatesMatches(latitude, longitude))
                {
                    if (mapPkg.isPartsFree())
                        this.mapPkgRepository.loadImages(mapPkg);
                    return mapPkg;
                }
            }
            Debug.WriteLine("MapSourceHdd: findMapPkg: not found pkg for: ("
                + latitude + "; " + longitude + "), zoom: " + zoom);
            return null;
        }

        private void changeZoom(int zoom)
        {
            this.zoom = zoom;
            readMapsDir();
        }

        /// <summary>
        /// Put map package to the source.
        /// </summary>
        public void putMapPkg(MapPackage mapPkg)
        {
            // will be useful when map downloaded from web
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
