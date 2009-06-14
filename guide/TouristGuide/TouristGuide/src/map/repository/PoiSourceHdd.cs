using System;
using System.Collections.Generic;
using System.Text;
using TouristGuide.map.obj;
using TouristGuide.map.repository.mapper;
using TouristGuide.map.repository.exception;
using System.IO;
using System.Diagnostics;

namespace TouristGuide.map.repository
{
    class PoiSourceHdd : PoiSource
    {
        private List<Poi> pois;
        private PoiMapperHdd poiMapperHdd;
        private string poisDir;
        private NamedArea currentNamedArea;

        public PoiSourceHdd(string poisDir, PoiMapperHdd poiMapperHdd)
        {
            this.poisDir = poisDir;
            this.poiMapperHdd = poiMapperHdd;
        }

        // loads pois without media -> fast search
        private void readPoisDir()
        {
            this.pois = new List<Poi>();
            string poisAreaDir = this.poisDir + "\\" + currentNamedArea.getName();
            // get poi dirs
            DirectoryInfo[] dirs;
            try
            {
                DirectoryInfo dirInfo = new DirectoryInfo(poisAreaDir);
                dirs = dirInfo.GetDirectories();
            }
            catch (Exception e)
            {
                Debug.WriteLine("readPoisDir: can't read dir: " + poisAreaDir, ToString());
                return;
            }
            // read each poi
            foreach (DirectoryInfo dir in dirs)
            {
                FileInfo[] poiFileInfo = dir.GetFiles("poi.xml");
                if (poiFileInfo.Length > 0)
                {
                    Debug.WriteLine("readPoisDir(): found poi, dir: " + dir.Name, ToString());
                    try
                    {
                        string poiSubDir = getPoiSubDir(dir.Name, this.currentNamedArea);
                        Poi poi = this.poiMapperHdd.getEmpty(poiSubDir);
                        this.pois.Add(poi);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine("readPoisDir: couldn't add poi from dir '"
                                + dir.Name + "' due to error: " + e.Message);
                    }
                }
            }
        }

        #region PoiSource Members

        public List<Poi> findPois(Area area)
        {
            if (this.pois == null)
            {
                readPoisDir();
            }
            List<Poi> areaPois =  this.pois.FindAll(
                delegate(Poi p) { 
                    return area.contains(p);
                }
            );
            foreach (Poi p in areaPois)
            {
                if (p.isDataFree())
                {
                    this.poiMapperHdd.loadData(p, getPoiSubDir(p, this.currentNamedArea));
                }
            }

            return areaPois;
        }

        public void putPois(List<Poi> pois, NamedArea namedArea)
        {
            foreach (Poi p in pois)
            {
                put(p, namedArea);
            }
        }

        #endregion

        internal void put(Poi p, NamedArea namedArea)
        {
            if (this.pois == null)
            {
                this.pois = new List<Poi>();
            }
            if (!p.isDataFree())
            {
                this.poiMapperHdd.save(p, getPoiSubDir(p, namedArea));
                p.free();
                if (!this.pois.Contains(p))
                {
                    this.pois.Add(p);
                }
            }
        }

        /**
         * Returns poi sub dir by poi and named area
         */
        private string getPoiSubDir(Poi p, NamedArea namedArea)
        {
            string dirName = p.getName() + "_" + p.getLatitude() + "_" + p.getLongitude();
            return getPoiSubDir(dirName, namedArea);
        }

        /**
         * Returns poi sub dir by dir name and named area
         */
        private string getPoiSubDir(string dirName, NamedArea namedArea)
        {
            return namedArea.getName() + "\\" + dirName;
        }

        public void setCurrentArea(NamedArea namedArea)
        {
            this.currentNamedArea = namedArea;
            readPoisDir();
        }

        internal void loadData(Poi poi)
        {
            if (poi.isDataFree())
                this.poiMapperHdd.loadData(poi, getPoiSubDir(poi, this.currentNamedArea));
        }

        internal void loadMedia(Poi poi)
        {
            if (poi.isMediaFree())
                this.poiMapperHdd.loadMedia(poi, getPoiSubDir(poi, this.currentNamedArea));
        }

        internal List<Poi> allPois()
        {
            if (this.pois == null)
            {
                readPoisDir();
            }
            return this.pois;
        }
    }
}
