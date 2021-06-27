using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace miosync
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());


            /*merge m = new merge();

            m.FixTime("l:\\20130922080552.gpx", "c:\\temp\\text.txt", "c:\\temp\\strava.txt");*/

        }
    }
}
