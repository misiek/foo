using System;
using System.Collections.Generic;
using System.Text;

namespace TouristGuide.map.obj
{
    public class MediaFile
    {
        private string title;
        private string descr;
        private Uri url;

        public MediaFile(string title, string descr, Uri url)
        {
            this.title = title;
            this.descr = descr;
            this.url = url;
        }

        public override string ToString()
        {
            return "<media_file " + this.title + ">";
        }

        public string getTitle()
        {
            return this.title;
        }

        public string getDescr()
        {
            return this.descr;
        }

        public Uri getUrl()
        {
            return this.url;
        }
    }
}
