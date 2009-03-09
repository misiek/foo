using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

using TouristGuide.map.obj;

namespace TouristGuide.map
{
    public class MapDisplayer
    {
        public MapPanel MapPanel
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        /// <summary>
        /// Displays loading message in map panel.
        /// </summary>
        public void loadingMap()
        {
            throw new System.NotImplementedException();
        }

        public void displayView(MapView mapView)
        {
            Debug.WriteLine("new MapView to display", this.ToString());

        }
    }
}
