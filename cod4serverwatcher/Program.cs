using System;
using System.Windows.Forms;
using cod4serverwatcher.CoD4_Objects;
using cod4serverwatcher.Ini;

namespace cod4serverwatcher
{
    static class Program
    {
        public static NotifyIcon NIcon;
        public static Server Server;
        /// <summary>
        /// Entry point of application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            // Load settings from ini file.
            IniUtils.CreateKeys();
            IniValues.LoadFromFile();

            // Initialize watched server.
            Server = new Server(IniValues.Host, IniValues.Port);
            // Same with tray icon.
            NIcon = new NotifyIcon();

            using (ProcessIcon pi = new ProcessIcon())
            {
                pi.Display();

                Application.Run();
            }
        }
    }
}
