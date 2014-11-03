using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace wine_shop_on_shop__client
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
            Application.Run(new user_control_panel());
        }
    }
}
