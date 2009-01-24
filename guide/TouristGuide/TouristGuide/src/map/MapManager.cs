using System;
using System.Collections.Generic;
using System.Text;

using TouristGuide.map.source;

namespace TouristGuide.map
{
    public class MapManager
    {
        private int currentMapPkg;

        private PoiSourceManager poiSourceManager;
        public PoiSourceManager PoiSourceManager
        {
            set
            {
                this.poiSourceManager = value;
            }
        }

        private MapDisplayer mapDisplayer;
        public MapDisplayer MapDisplayer
        {
            set
            {
                this.mapDisplayer = value;
            }
        }

        private MapSourceManager mapSourceManager;
        public MapSourceManager MapSourceManager
        {
            set
            {
                this.mapSourceManager = value;
            }
        }

        public void newPosition()
        {
            throw new System.NotImplementedException();
        }
    }
}
