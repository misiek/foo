using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;

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

        private String mapsDir;
        private MapPkgRepository mapPkgRepo;
        private MapSourceHdd mSourceHdd;

        private AppContext()
        {
            // get gull path to exe file
            String fullPath = System.Reflection.Assembly.GetCallingAssembly().GetName().CodeBase;
            // get working directory path eg. '\Program Files\TouristGuide'
            String directoryPath = fullPath.Substring(0, fullPath.LastIndexOf("\\"));
            Debug.WriteLine("AppContext(): directoryPath: " + directoryPath);
            this.mapsDir = directoryPath + "\\maps";

            this.mapPkgRepo = new MapPkgRepository(this.mapsDir);
            Debug.WriteLine("AppContext(): MapPkgRepository instantiated.");

            this.mSourceHdd = new MapSourceHdd(this.mapsDir, this.mapPkgRepo);
            Debug.WriteLine("AppContext(): MapSourceHdd instantiated.");
        }

        public MapSourceHdd getMapSourceHdd()
        {
            return this.mSourceHdd;
        }

    }
}
