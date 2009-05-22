using System;
using System.Collections.Generic;
using System.Text;
using TouristGuide.map.obj;
using System.Xml;
using System.Diagnostics;

namespace TouristGuide.map.repository.adapter
{
    public class MainDetailsXmlAdapter : ListXmlAdapter
    {
        public MainDetailsXmlAdapter(List<MainDetail> mainDetails)
        {
            this.list = mainDetails;
            this.objAdapter = new MainDetailXmlAdapter();
        }
    }
}
