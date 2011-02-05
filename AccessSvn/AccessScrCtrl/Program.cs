using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace AccessScrCtrl {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            //TODO: see http://www.codeproject.com/KB/exception/ExceptionHandling.aspx for error handling
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainFrm());
        }
    }
}
