using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;

using TouristGuide.map.exception;
using TouristGuide.map.repository.mapper;
using TouristGuide.map.obj;

namespace TouristGuide.map.repository
{
    public class MapSourceHdd : MapSource
    {
        private string mapsDir;
        private MapPkgMapperHdd mapPkgMapperHdd;
        private List<MapPackage> availableMapPkgs;

        private int zoom;

        /// <summary>
        /// When loading map takes long time (source web or hdd),
        /// set this event (eg. map displayer can show loading box)
        /// </summary>
        public event LoadingMapPkg loadingMapPkgEvent;
        public delegate void LoadingMapPkg(string msg);

        public MapSourceHdd(string mapsDir, MapPkgMapperHdd mapPkgMapperHdd)
        {
            this.mapsDir = mapsDir;
            this.mapPkgMapperHdd = mapPkgMapperHdd;
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
                        MapPackage mapPkg = this.mapPkgMapperHdd.getWithoutImages(mapPkgName, this.zoom);
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
                //Debug.WriteLine("MapSourceHdd: findMapPkg: checking: " + mapPkg);
                if (mapPkg.coordinatesMatches(latitude, longitude))
                {
                    if (loadingMapPkgEvent != null)
                        loadingMapPkgEvent("Loading map. Please wait...");
                    if (mapPkg.isPartsFree())
                        this.mapPkgMapperHdd.loadImages(mapPkg);
                    //Debug.WriteLine("MapSourceHdd: findMapPkg: found map pkg: " + mapPkg);
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
