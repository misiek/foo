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
        private string poisPath;

        private GpsDeviceCSharp gpsDevice;
        private GpsSymulator gpsSymulator;

        private Portal portal;

        private MapPkgMapperHdd mapPkgMapperHdd;
        private MapSourceMem mapSourceMem;
        private MapSourceHdd mapSourceHdd;
        private MapSourceWeb mapSourceWeb;
        private MapPkgRepository mapPkgRepository;

        private PoiRepository poiRepository;
        private PoiSourceMem poiSourceMem;
        private PoiSourceHdd poiSourceHdd;
        private PoiSourceWeb poiSourceWeb;
        private PoiMapperHdd poiMapperHdd;

        private MapDisplayer mapDisplayer;
        private Targets targets;
        private GpsDataAnalyzer gpsDataAnalyzer;
        private MapViewBuilder mapViewBuilder;
        private MapManager mapManager;

        private MainWindow mainWindow;
        private PoiBrowser poiBrowser;

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
            this.poisPath = this.dirPath + "\\pois";

            // config
            this.config = new Config();
            Debug.WriteLine("AppContext(): Config instantiated.");

            // main window
            this.mainWindow = new MainWindow();
            Debug.WriteLine("AppContext(): MainWindow instantiated.");

            // gps device
            this.gpsDevice = new GpsDeviceCSharp();
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
            this.mapDisplayer = new MapDisplayer(this.mainWindow.MapPanel, this.mainWindow.MapMessageBox);
            Debug.WriteLine("AppContext(): MapDisplayer instantiated.");

            // map pkg repository
            this.mapPkgRepository = new MapPkgRepository();
            this.mapPkgRepository.MapSourceMem = this.mapSourceMem;
            this.mapPkgRepository.MapSourceHdd = this.mapSourceHdd;
            this.mapPkgRepository.MapSourceWeb = this.mapSourceWeb;
            Debug.WriteLine("AppContext(): MapPkgRepository instantiated.");

            // portal
            this.portal = new Portal();
            this.portal.Config = this.config;

            // poi source web
            this.poiSourceWeb = new PoiSourceWeb();
            this.poiSourceWeb.Portal = this.portal;

            // poi source mem
            this.poiSourceMem = new PoiSourceMem();

            // poi mapper hdd
            this.poiMapperHdd = new PoiMapperHdd(this.poisPath);

            // poi source hdd
            this.poiSourceHdd = new PoiSourceHdd(this.poisPath, this.poiMapperHdd);

            // poi repository
            this.poiRepository = new PoiRepository();
            this.poiRepository.PoiSourceMem = this.poiSourceMem;
            this.poiRepository.PoiSourceHdd = this.poiSourceHdd;
            this.poiRepository.PoiSourceWeb = this.poiSourceWeb;
            Debug.WriteLine("AppContext(): PoiRepository instantiated.");

            // map targets
            this.targets = new Targets();
            Debug.WriteLine("AppContext(): Targets instantiated.");

            // gps data analyzer
            this.gpsDataAnalyzer = new GpsDataAnalyzer();

            // map view builder
            this.mapViewBuilder = new MapViewBuilder();
            this.mapViewBuilder.MapPkgRepository = this.mapPkgRepository;
            this.mapViewBuilder.PoiRepository = this.poiRepository;
            this.mapViewBuilder.Targets = this.targets;
            this.mapViewBuilder.GpsDataAnalyzer = this.gpsDataAnalyzer;
            Debug.WriteLine("AppContext(): MapViewBuilder instantiated.");

            // map manager
            this.mapManager = new MapManager();
            this.mapManager.MapDisplayer = this.mapDisplayer;
            this.mapManager.MapPkgRepository = this.mapPkgRepository;
            this.mapManager.PoiRepository = this.poiRepository;
            this.mapManager.GpsDataAnalyzer = this.gpsDataAnalyzer;
            this.mapManager.MapViewBuilder = this.mapViewBuilder;
            Debug.WriteLine("AppContext(): MapManager instantiated.");

            this.poiBrowser = new PoiBrowser();
            this.poiBrowser.PoiRepository = this.poiRepository;
            this.poiBrowser.Targets = this.targets;
            Debug.WriteLine("AppContext(): PoiBrowser instantiated.");

            // application events
            this.appEvents = new AppEvents(this.mainWindow, this.mapManager, this.mapPkgRepository);
        }

        public GpsDeviceCSharp getGpsDevice()
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

        public MapManager getMapManager()
        {
            return this.mapManager;
        }

        public PoiBrowser getPoiBrowser()
        {
            return this.poiBrowser;
        }

    }
}
