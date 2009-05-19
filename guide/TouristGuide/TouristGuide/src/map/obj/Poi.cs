using System;
using System.Collections.Generic;
using System.Text;

namespace TouristGuide.map.obj
{
    public class Poi
    {
        private string lang;
        private double latitude;
        private double longitude;
        private string type;
        private string name;
        private string descr;
        private List<MediaFile> mediaFiles = new List<MediaFile>();
        private List<MainDetail> mainDetails = new List<MainDetail>();
        // to be able to update poi from portal
        private DateTime updated;
    }
}
