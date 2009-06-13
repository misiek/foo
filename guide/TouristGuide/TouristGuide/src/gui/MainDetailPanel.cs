using System;
using System.Collections.Generic;
using System.Text;
using TouristGuide.map.obj;

namespace TouristGuide.gui
{
    class MainDetailPanel : DialogPanel
    {
        private MainDetail detail;

        public MainDetailPanel(MainDetail detail)
        {
            this.detail = detail;
        }

    }
}
