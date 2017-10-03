using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using cod4serverwatcher.CoD4_Objects;
using cod4serverwatcher.Ini;
using Microsoft.Win32;

namespace cod4serverwatcher
{
    static class Program
    {
        public static NotifyIcon NIcon;
        public static Server Server;
        public static Form Invoker;
        private static System.Timers.Timer RefreshTimer;

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

            using (NIcon = new NotifyIcon())
            {
                RefreshTimer = new System.Timers.Timer(Constants.RefreshTimerInterval);
                RefreshTimer.AutoReset = true;
                RefreshTimer.Elapsed += refreshTimer_Elapsed;
                RefreshTimer.Start();

                // Hint for invokes. ;_;
                Invoker = new Form();
                var hWnd = Invoker.Handle;

                NIcon.Visible = true;
                NIcon.MouseClick += new MouseEventHandler(NI_MouseClick);
                NIcon.MouseDoubleClick += new MouseEventHandler(NI_MouseDoubleClick);
                // Standart icon from Call of Duty 4 folder.
                NIcon.Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location); ;
                NIcon.Text = "Call of Duty 4 Server Watcher";
                NIcon.Visible = true;
                NIcon.ContextMenuStrip = new ContextMenus().Create();

                // Task for instant refresh.
                Task.Factory.StartNew(RefreshDisplay);

                Application.Run();
            }
        }
        private static void refreshTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            // Stop the timer while we work.
            RefreshTimer.Enabled = false;

            // Save the host and the port in the INI file if they have changed.
            if (IniValues.Host != Program.Server.Host)
            {
                IniFile iniFile = new IniFile(Constants.IniPath);
                iniFile.WriteKey("Server", "Host", Program.Server.Host);
                IniValues.Host = Program.Server.Host;
            }
            if (IniValues.Port != Program.Server.Port)
            {
                IniFile iniFile = new IniFile(Constants.IniPath);
                iniFile.WriteKey("Server", "Port", Program.Server.Port.ToString());
                IniValues.Port = Program.Server.Port;
            }

            // Refresh the display.
            RefreshDisplay();
            RefreshTimer.Enabled = true;
        }
        private static void RefreshDisplay()
        {
            try
            {
                if (Program.Server.Port == 0)
                {
                    // Error!
                    Invoker.Invoke((MethodInvoker)delegate 
                    {
                        NIcon.BalloonTipTitle = "Error!";
                        NIcon.BalloonTipText = "Port must be > 0!";
                        NIcon.BalloonTipIcon = ToolTipIcon.Error;
                        NIcon.ShowBalloonTip(10000);
                        NIcon.Text = "Port must be > 0!";
                    });
                }
                else
                {
                    Program.Server.Refresh();
                    if (Program.Server.Status == ServerStatus.Up)
                    {
                        // Server is up.
                        if (Program.Server.FreeSlot)
                        {
                            Invoker.Invoke((MethodInvoker) delegate
                            {
                                NIcon.Text = Server.TrimLongServerName(Program.Server.Name) + " - " +
                                Program.Server.Map.DisplayName + " (" + Program.Server.PlayersCount + "/" +
                                ( Program.Server.MaxPlayers - Program.Server.PrivateClients ) + ")";
                            });
                        }
                        else
                        {
                            // No free slot.
                            Invoker.Invoke((MethodInvoker) delegate
                            {
                                NIcon.Text = Server.TrimLongServerName(Program.Server.Name) + " - " +
                                Program.Server.Map.DisplayName + " (" + Program.Server.PlayersCount + "/" +
                                (Program.Server.MaxPlayers - Program.Server.PrivateClients) + ")";
                            });
                        }

                        Invoker.Invoke((MethodInvoker)delegate 
                        {
                            NIcon.Text = Server.TrimLongServerName(Program.Server.Name) + " - " +
                                Program.Server.Map.DisplayName + " (" + Program.Server.PlayersCount + "/" +
                                (Program.Server.MaxPlayers - Program.Server.PrivateClients) + ")";
                        });

                    }
                    else
                    {
                        // Server is not reachable.
                        Invoker.Invoke((MethodInvoker)delegate 
                        {
                            NIcon.Text = Program.Server.Host + ":" + Program.Server.Port + " not reachable.";
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex is ObjectDisposedException || ex is InvalidOperationException)
                {
                    // Form has been closed (more likely by the user). Stop process.
                    return;
                }
                else
                {
                    throw;
                }
            }
        }
        static void NI_MouseClick(object sender, MouseEventArgs e)
        {
            // Handle mouse button clicks.
            if (e.Button == MouseButtons.Right)
            {
                // Start Windows Explorer.
                NIcon.ContextMenuStrip.Show();
            }
        }

        static void NI_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (Program.Server.FreeSlot)
            {
                if (!Program.Server.Connect())
                {
                    Program.NIcon.BalloonTipIcon = ToolTipIcon.Error;
                    Program.NIcon.BalloonTipTitle = "Error!";
                    Program.NIcon.BalloonTipText = "An error occured. Make sure the path to the Call of Duty exe " +
                                            "file registered in the configuration file (" + Path.GetFullPath(Constants.IniPath) + ") is correct. Could not start Call of Duty";
                    Program.NIcon.ShowBalloonTip(10000);
                }
            }
            else
            {
                Program.NIcon.BalloonTipIcon = ToolTipIcon.Error;
                Program.NIcon.BalloonTipTitle = Program.Server.Name;
                Program.NIcon.BalloonTipText = string.Format("No free slots ({0}/{1})", Program.Server.PlayersCount, (Program.Server.MaxPlayers - Program.Server.PrivateClients));
                Program.NIcon.ShowBalloonTip(10000);
            }
        }
    }
}
