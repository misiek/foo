using System;
using System.Collections.Generic;
using System.Text;
using TouristGuide.map.obj;
using System.Xml;
using System.Diagnostics;

namespace TouristGuide.map.repository.adapter
{
    public class MediaFilesXmlAdapter: ListXmlAdapter
    {
        public MediaFilesXmlAdapter(List<MediaFile> mediaFiles)
        {
            this.list = mediaFiles;
            this.objAdapter = new MediaFileXmlAdapter();
        }
    }
}
