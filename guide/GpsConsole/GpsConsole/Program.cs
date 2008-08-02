using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace GpsConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //IntPtr location = new IntPtr(1);
            //IntPtr satellite = new IntPtr(1);
            //GpsDevice gps = new GpsDevice(satellite, location);
            GpsDevice gps = new GpsDevice();
            gps.Open();
            //while (true)
            //{
            //    Console.WriteLine("location ptr: " + location.ToInt32());
            //
            //}
        }
    }
}
