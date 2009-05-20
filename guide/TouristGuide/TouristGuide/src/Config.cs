using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace TouristGuide
{
    public class Config
    {
        private Hashtable configHash;

        public Config()
        {
            // TODO: move it to file
            configHash["mobappid"] = "45435kj34l5j3l45j3l5";
            configHash["portal_url"] = "http://192.168.1.6:8080/tgportalws";
        }

        public string get(string key)
        {
            return (string)configHash[key];
        }
    }
}
