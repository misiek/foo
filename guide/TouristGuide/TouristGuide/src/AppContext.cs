using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;

using Gps;
using TouristGuide.map;
using TouristGuide.map.repository;
using TouristGuide.map.source;

// for test
using TouristGuide.map.obj;

namespace TouristGuide
{
    /**
     * Singleton 
     * Instantiates other classes.
     **/
    public class AppContext
    {
        private static readonly AppContext instance = new AppContext();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static AppContext() {}

        public static AppContext Instance
        {
            get
            {
                return instance;
            }
        }

        private String dirPath;
        private String mapsPath;

        private GpsDevice gpsDevice;
        private GpsSymulator gpsSymulator;

        private MapPkgRepository mapPkgRepo;
        private MapSourceMem mapSourceMem;
        private MapSourceHdd mapSourceHdd;
        private MapSourceWeb mapSourceWeb;
        private MapSourceManager mapSourceManager;

        private PoiSourceManager poiSourceManager;

        private MapPanel mapPanel;
        private MapDisplayer mapDisplayer;
        private MapManager mapManager;

        private AppContext()
        {
            // get gull path to exe file
            String fullPath = System.Reflection.Assembly.GetCallingAssembly().GetName().CodeBase;
            // get working directory path eg. '\Program Files\TouristGuide'
            this.dirPath = fullPath.Substring(0, fullPath.LastIndexOf("\\"));
            Debug.WriteLine("AppContext(): directoryPath: " + this.dirPath);
            this.mapsPath = this.dirPath + "\\maps";
            Debug.WriteLine("AppContext(): mapsPath: " + this.mapsPath);

            // gps device
            this.gpsDevice = new GpsDevice();
            Debug.WriteLine("AppContext(): GpsDevice instantiated.");

            // gps symulator
            this.gpsSymulator = new GpsSymulator(this.dirPath);
            Debug.WriteLine("AppContext(): GpsSymulator instantiated.");

            // map pkg repository
            this.mapPkgRepo = new MapPkgRepository(this.mapsPath);
            Debug.WriteLine("AppContext(): MapPkgRepository instantiated.");

            // map source memory
            this.mapSourceMem = new MapSourceMem();
            Debug.WriteLine("AppContext(): MapSourceMem instantiated.");

            // map source hard drive
            this.mapSourceHdd = new MapSourceHdd(this.mapsPath, this.mapPkgRepo);
            Debug.WriteLine("AppContext(): MapSourceHdd instantiated.");

            // map source web server
            this.mapSourceWeb = new MapSourceWeb();
            Debug.WriteLine("AppContext(): MapSourceWeb instantiated.");

            // map displayer
            this.mapDisplayer = new MapDisplayer();
            Debug.WriteLine("AppContext(): MapDisplayer instantiated.");

            // map source manager
            this.mapSourceManager = new MapSourceManager();
            this.mapSourceManager.MapDisplayer = this.mapDisplayer;
            this.mapSourceManager.MapSourceMem = this.mapSourceMem;
            this.mapSourceManager.MapSourceHdd = this.mapSourceHdd;
            this.mapSourceManager.MapSourceWeb = this.mapSourceWeb;
            Debug.WriteLine("AppContext(): MapSourceManager instantiated.");

            // poi source manager
            this.poiSourceManager = new PoiSourceManager();
            Debug.WriteLine("AppContext(): PoiSourceManager instantiated.");

            // map manager
            this.mapManager = new MapManager();
            this.mapManager.MapDisplayer = this.mapDisplayer;
            this.mapManager.MapSourceManager = this.mapSourceManager;
            this.mapManager.PoiSourceManager = this.poiSourceManager;
            Debug.WriteLine("AppContext(): MapManager instantiated.");

            //test();
        }

        private void test()
        {
            MapPackage mapPkg = this.mapSourceManager.getMapPkg(50.057, 19.933, 0);
            Debug.WriteLine("AppContext: Test: " + mapPkg.getName());

            mapPkg = this.mapSourceManager.getMapPkg(50.057, 19.933, 0);
            Debug.WriteLine("AppContext: Test: " + mapPkg.getName());

            mapPkg = this.mapSourceManager.getMapPkg(50.057, 19.933, 0);
            Debug.WriteLine("AppContext: Test: " + mapPkg.getName());
        }

        public GpsDevice getGpsDevice()
        {
            return this.gpsDevice;
        }

        public GpsSymulator getGpsSymulator()
        {
            return this.gpsSymulator;
        }
    }
}
