using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod4serverwatcher
{
    internal static class Constants
    {

        /// <summary>
        /// The contact website.
        /// </summary>
        public static readonly String Website = "https://github.com/xeqlol/cod4serverwatcher";

        /// <summary>
        /// The path to the INI file.
        /// </summary>
        public static readonly String IniPath = ".\\settings.ini";

        /// <summary>
        /// The default path to the qstat.exe file.
        /// </summary>
        public static readonly String DefaultQstatExePath = ".\\qstat.exe";

        /// <summary>
        /// The default path to the CoD MP exe file.
        /// </summary>
        public static readonly String DefaultCoDMPExePath = @"E:\Games\Call of Duty 4 - Modern Warfare\iw3mp.exe";

        /// <summary>
        /// The interval (in milliseconds) of the timer used to refresh the status of the server.
        /// </summary>
        public static readonly int RefreshTimerInterval = 5000;

        /// <summary>
        /// The default server host (=[JFF]= Cracked Server #3 Hardcore).
        /// </summary>
        public static readonly String DefaultServerHost = "176.9.54.140";

        /// <summary>
        /// The default server port.
        /// </summary>
        public static readonly int DefaultServerPort = 28906;
    }
}
