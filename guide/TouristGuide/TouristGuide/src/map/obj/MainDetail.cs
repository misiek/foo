using System;
using System.Collections.Generic;
using System.Text;

namespace TouristGuide.map.obj
{
    public class MainDetail : Detail
    {
        private List<SubDetail> subDetails;

        public MainDetail(string title, string descr) : base(title, descr)
        {
            subDetails = new List<SubDetail>();
        }

        public override string ToString()
        {
            return "<detail " + this.title + ">";
        }
    }
}
