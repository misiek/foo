using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;

using Gps;
using TouristGuide.gui;
using TouristGuide.map;
using TouristGuide.map.repository;
using TouristGuide.map.repository.mapper;

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

        private MapPkgMapperHdd mapPkgMapperHdd;
        private MapSourceMem mapSourceMem;
        private MapSourceHdd mapSourceHdd;
        private MapSourceWeb mapSourceWeb;
        private MapPkgRepository mapPkgRepository;

        private PoiRepository poiRepository;

        private MapDisplayer mapDisplayer;
        private MapManager mapManager;

        private MainWindow mainWindow;
        private AppEvents appEvents;

        private Config config;

        private AppContext()
        {
            // get gull path to exe file
            String fullPath = System.Reflection.Assembly.GetCallingAssembly().GetName().CodeBase;
            // get working directory path eg. '\Program Files\TouristGuide'
            this.dirPath = fullPath.Substring(0, fullPath.LastIndexOf("\\"));
            Debug.WriteLine("AppContext(): directoryPath: " + this.dirPath);
            this.mapsPath = this.dirPath + "\\maps";
            Debug.WriteLine("AppContext(): mapsPath: " + this.mapsPath);

            // config
            this.config = new Config();
            Debug.WriteLine("AppContext(): Config instantiated.");

            // main window
            this.mainWindow = new MainWindow();
            Debug.WriteLine("AppContext(): MainWindow instantiated.");

            // gps device
            this.gpsDevice = new GpsDevice();
            Debug.WriteLine("AppContext(): GpsDevice instantiated.");

            // gps symulator
            this.gpsSymulator = new GpsSymulator(this.dirPath);
            Debug.WriteLine("AppContext(): GpsSymulator instantiated.");

            // map pkg repository
            this.mapPkgMapperHdd = new MapPkgMapperHdd(this.mapsPath);
            Debug.WriteLine("AppContext(): MapPkgMapperHdd instantiated.");

            // map source memory
            this.mapSourceMem = new MapSourceMem();
            Debug.WriteLine("AppContext(): MapSourceMem instantiated.");

            // map source hard drive
            this.mapSourceHdd = new MapSourceHdd(this.mapsPath, this.mapPkgMapperHdd);
            Debug.WriteLine("AppContext(): MapSourceHdd instantiated.");

            // map source web server
            this.mapSourceWeb = new MapSourceWeb();
            Debug.WriteLine("AppContext(): MapSourceWeb instantiated.");

            // map displayer
            this.mapDisplayer = new MapDisplayer(this.mainWindow.MapPanel);
            Debug.WriteLine("AppContext(): MapDisplayer instantiated.");

            // map source manager
            this.mapPkgRepository = new MapPkgRepository();
            this.mapPkgRepository.MapSourceMem = this.mapSourceMem;
            this.mapPkgRepository.MapSourceHdd = this.mapSourceHdd;
            this.mapPkgRepository.MapSourceWeb = this.mapSourceWeb;
            Debug.WriteLine("AppContext(): MapPkgRepository instantiated.");

            // poi source manager
            this.poiRepository = new PoiRepository();
            Debug.WriteLine("AppContext(): PoiRepository instantiated.");

            // map manager
            this.mapManager = new MapManager();
            this.mapManager.MapDisplayer = this.mapDisplayer;
            this.mapManager.MapPkgRepository = this.mapPkgRepository;
            this.mapManager.PoiRepository = this.poiRepository;
            Debug.WriteLine("AppContext(): MapManager instantiated.");

            // application events
            this.appEvents = new AppEvents(this.mainWindow, this.mapManager, this.mapPkgRepository);
        }

        public GpsDevice getGpsDevice()
        {
            return this.gpsDevice;
        }

        public GpsSymulator getGpsSymulator()
        {
            return this.gpsSymulator;
        }

        public MainWindow getMainWindow()
        {
            return this.mainWindow;
        }

        public AppEvents getAppEvents()
        {
            return this.appEvents;
        }

        public Config getConfig()
        {
            return this.config;
        }
    }
}
