using System;
using System.Collections.Generic;
using System.Text;

using TouristGuide.map.source;

namespace TouristGuide.map
{
    public class MapManager
    {
        private int currentMapPkg;
    
        public PoiSourceManager PoiSourceManager
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public MapDisplayer MapDisplayer
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public TouristGuide.map.source.MapSourceManager MapSourceManager
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public void newPosition()
        {
            throw new System.NotImplementedException();
        }
    }
}
