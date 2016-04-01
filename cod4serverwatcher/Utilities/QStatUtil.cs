using System;
using System.Diagnostics;
using System.IO;

namespace cod4serverwatcher.Utilities
{
    public static String GetQStatOutput(String host, int port)
    {
        ProcessStartInfo psi = new ProcessStartInfo();
        psi.FileName = IniValues.QStatExePath;
        psi.Arguments = "-cods " + host + ":" + port + " -P -R -xml"; // -P:players -R:server rules
        psi.CreateNoWindow = true;
        psi.UseShellExecute = false;
        psi.RedirectStandardOutput = true;

        Process process = Process.Start(psi);
        StreamReader reader = process.StandardOutput;
        String output = reader.ReadToEnd();

        return output;
    }
}
