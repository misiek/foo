using System;
using System.Collections.Generic;
using System.Text;

using TouristGuide.map.repository;

namespace TouristGuide.map
{
    public class MapManager
    {
        private int currentMapPkg;

        private MapDisplayer mapDisplayer;
        public MapDisplayer MapDisplayer
        {
            set
            {
                this.mapDisplayer = value;
            }
        }

        private MapPkgRepository mapPkgRepository;
        public MapPkgRepository MapPkgRepository
        {
            set
            {
                this.mapPkgRepository = value;
            }
        }
        
       
        private PoiRepository poiRepository;
        public PoiRepository PoiRepository
        {
            set
            {
                this.poiRepository = value;
            }
        }      


        public void newPosition()
        {
            throw new System.NotImplementedException();
        }

    



       
    }
}
