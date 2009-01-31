using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace TouristGuide
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [MTAThread]
        static void Main()
        {
            // initialize components
            AppContext appContext = AppContext.Instance;
            // run form
            Application.Run(appContext.getMainWindow());
        }
    }
}