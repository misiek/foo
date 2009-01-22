using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using TouristGuide.exceptions;

namespace TouristGuide
{
    public class MapSourceHdd : MapSource
    {
        private string mapsDir;
        private MapPkgRepository mapPkgRepository;
        private List<MapPackage> availableMapPkgs;

        public MapSourceHdd(string mapsDir, MapPkgRepository mapPkgRepository)
        {
            this.mapsDir = mapsDir;
            this.mapPkgRepository = mapPkgRepository;
            this.availableMapPkgs = new List<MapPackage>();
            readMapsDir();
        }

        /*
         * Create available map packages list
         * by reading maps directory.
         */
        public void readMapsDir()
        {
            DirectoryInfo dirInfo = new DirectoryInfo(this.mapsDir);
            DirectoryInfo[] dirs = dirInfo.GetDirectories();
            foreach (DirectoryInfo dir in dirs)
            {
                FileInfo[] mapFileInfo = dir.GetFiles("map.xml");
                if (mapFileInfo.Length > 0)
                {
                    Debug.WriteLine("MapSourceHdd: readMapsDir(): found map package, dir: " + dir.Name);
                    string mapPkgName = dir.Name;
                    try
                    {
                        MapPackage mapPkg = this.mapPkgRepository.getWithoutImages(mapPkgName);
                        this.availableMapPkgs.Add(mapPkg);
                        Debug.WriteLine("MapSourceHdd: readMapsDir: added map packege '" + mapPkgName + "'");
                    }
                    catch (MapPkgRepositoryException e) 
                    {
                        Debug.WriteLine("MapSourceHdd: readMapsDir: couldn't add map packege '" 
                                + mapPkgName + "' due to error: " + e.Message);
                    }
                }
            }
        }

        #region MapSource Members

        /// <summary>
        /// Find map package by coordinates in the source.
        /// </summary>
        /// <returns>MapPackage instance.</returns>
        public MapPackage findMapPkg(double latitude, double longitude)
        {
            foreach (MapPackage mapPkg in availableMapPkgs)
            {
                if (mapPkg.coordinatesMatches(latitude, longitude))
                {
                    if (mapPkg.isPartsFree())
                        this.mapPkgRepository.loadImages(mapPkg);
                    return mapPkg;
                }
            }
            return null;
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
