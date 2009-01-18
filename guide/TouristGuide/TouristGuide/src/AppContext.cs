using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace TouristGuide
{
    public class AppContext
    {
        private MapSourceHdd mSourceHdd;

        public AppContext()
        {
            // get gull path to exe file
            String fullPath = System.Reflection.Assembly.GetCallingAssembly().GetName().CodeBase;
            // get working directory path eg. '\Program Files\TouristGuide'
            String directoryPath = fullPath.Substring(0, fullPath.LastIndexOf("\\"));
            Debug.WriteLine("AppContext(): directoryPath: " + directoryPath);
            string mapsDir = directoryPath + "\\maps";

            MapPkgRepository mapPkgRepo = new MapPkgRepository();
            Debug.WriteLine("AppContext(): MapPkgRepository instantiated.");
            this.mSourceHdd = new MapSourceHdd(mapsDir, mapPkgRepo);
            Debug.WriteLine("AppContext(): MapSourceHdd instantiated.");
        }

        public MapSourceHdd getMapSourceHdd()
        {
            return this.mSourceHdd;
        }

    }
}
