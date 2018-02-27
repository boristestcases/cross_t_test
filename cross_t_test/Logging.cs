using System;
using System.Globalization;
using System.IO;

namespace cross_t_test
{
    public static class Logging
    {
        public static void WriteLog(string message)
        {
            StreamWriter log = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\log.txt", true);
            var line = $"{DateTime.Now.ToString(CultureInfo.InvariantCulture)},{message}";
            log.WriteLine(line);
            log.Flush();
            log.Close();
        }
    }
}
