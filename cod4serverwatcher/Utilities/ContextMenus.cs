using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace cod4serverwatcher
{
    /// <summary>
    /// 
    /// </summary>
    class ContextMenus
    {
        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <returns>ContextMenuStrip</returns>
        public ContextMenuStrip Create()
        {
            // Add the default menu options.
            ContextMenuStrip menu = new ContextMenuStrip();
            ToolStripMenuItem item;
            ToolStripSeparator sep;

            // Start game.
            item = new ToolStripMenuItem();
            item.Text = "Start Call of Duty 4";
            item.Click += new EventHandler(Start_Click);
            menu.Items.Add(item);

            // Separator.
            sep = new ToolStripSeparator();
            menu.Items.Add(sep);

            // Show settings in notepad.
            item = new ToolStripMenuItem();
            item.Text = "Settings";
            item.Click += new EventHandler(Settings_Click);
            menu.Items.Add(item);

            // Restart.
            item = new ToolStripMenuItem();
            item.Text = "Restart";
            item.Click += new EventHandler(Restart_Click);
            menu.Items.Add(item);

            // Show about in notepad.
            item = new ToolStripMenuItem();
            item.Text = "About";
            item.Click += new EventHandler(About_Click);
            menu.Items.Add(item);

            // Separator.
            sep = new ToolStripSeparator();
            menu.Items.Add(sep);

            // Exit.
            item = new ToolStripMenuItem();
            item.Text = "Exit";
            item.Click += new System.EventHandler(Exit_Click);
            menu.Items.Add(item);

            return menu;
        }
        void Start_Click(object sender, EventArgs e)
        {
            if (Program.Server.FreeSlot)
            {
                if (!Program.Server.Connect())
                {
                    Program.NIcon.BalloonTipIcon = ToolTipIcon.Error;
                    Program.NIcon.BalloonTipTitle = "Error!";
                    Program.NIcon.BalloonTipText = "An error occured. Make sure the path to the Call of Duty exe " +
                                            "file registered in the configuration file (" + Path.GetFullPath(Constants.IniPath) + ") is correct. Could not start Call of Duty";
                    Program.NIcon.ShowBalloonTip(30000);
                }
            }
            else
            {
                Program.NIcon.BalloonTipIcon = ToolTipIcon.Error;
                Program.NIcon.BalloonTipTitle = Program.Server.Name;
                Program.NIcon.BalloonTipText = string.Format("No free slots ({0}/{1})", Program.Server.PlayersCount, (Program.Server.MaxPlayers - Program.Server.PrivateClients));
                Program.NIcon.ShowBalloonTip(30000);
            }
        }

        void Settings_Click(object sender, EventArgs e)
        {
            Process.Start("notepad.exe", "settings.ini");
        }

        void Restart_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Application.ExecutablePath);
            Application.Exit();
        }
        void About_Click(object sender, EventArgs e)
        {
            Process.Start("notepad.exe", "about.txt");
        }
        void Exit_Click(object sender, EventArgs e)
        {
            // Quit without further ado.
            Application.Exit();
        }
    }
}