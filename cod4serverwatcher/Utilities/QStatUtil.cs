using System;
using System.Diagnostics;
using System.IO;
using cod4serverwatcher.Ini;

namespace cod4serverwatcher.Utilities
{
    internal static class QStatUtil
    {
        public static String GetQStatOutput(String host, int port)
        {
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = IniValues.QStatExePath;
            psi.Arguments = "-cod4s " + host + ":" + port + " -P -R -xml";
            psi.CreateNoWindow = true;
            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;

            Process process = Process.Start(psi);
            StreamReader reader = process.StandardOutput;
            String output = reader.ReadToEnd();

            return output;
        }
    }
}
